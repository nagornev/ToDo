﻿using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Database.Contexts;
using ToDo.Microservices.Identity.Database.Entities;
using ToDo.Microservices.Identity.Database.Extensions;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Publishers;
using ToDo.Microservices.Identity.UseCases.Repositories;

namespace ToDo.Microservices.Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IdentityContext _context;

        private IUserPublisher _userPublisher;

        public UserRepository(IdentityContext context,
                              IUserPublisher userPublisher)
        {
            _context = context;
            _userPublisher = userPublisher;
        }

        public async Task<Result<User>> Get(Guid userId)
        {
            Result<User> userResult = await GetUser(x => x.Id == userId);

            return userResult.Success ?
                      userResult :
                      Result<User>.Failure(error => error.NullOrEmpty($"The user {userId} was not found."));
        }

        public async Task<Result<User>> Get(string email)
        {
            Result<User> userResult = await GetUser(x => x.Email == email);

            return userResult.Success ?
                    userResult :
                    Result<User>.Failure(error => error.NullOrEmpty($"The user {email} was not found."));
        }

        public async Task<Result> Create(User user)
        {
            UserEntity userEntity = user.GetEntity();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Users.AddAsync(userEntity);

                    if (await _context.SaveChangesAsync() > 0 &&
                        await _userPublisher.New(user))
                    {
                        await transaction.CommitAsync();

                        return Result.Successful();
                    }

                    await transaction.RollbackAsync();
                    return Result.Failure(error => error.InternalServer($"The user {user.Email} was not created."));
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Result> Update(User user)
        {
            return (await _context.Users.Where(x => x.Id == user.Id)
                                            .ExecuteUpdateAsync(p => p.SetProperty(x => x.Email, user.Email)
                                                                      .SetProperty(x => x.RoleId, (int)user.Access.Role))) > 0 ?
                          Result.Successful() :
                          Result.Failure(error => error.NullOrEmpty($"The user {user.Id} was not found."));
        }

        public async Task<Result> Delete(Guid userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                    if (userEntity is null)
                        return Result.Failure(error => error.NullOrEmpty($"The user {userId} was not found."));

                    _context.Users.Remove(userEntity);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        //TODO: create a message for mq, if the user is able to delete his account
                    }
                    else
                    {
                        await transaction.RollbackAsync();

                        return Result.Failure(error => error.InternalServer("Sign up error. Please try again later."));
                    }

                    await transaction.CommitAsync();

                    return Result.Successful();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task<Result<User>> GetUser(Expression<Func<UserEntity, bool>> predicate)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(predicate);

            return userEntity is not null ?
                    userEntity.GetDomain() :
                    Result<User>.Failure(error => error.NullOrEmpty("The user was not found."));
        }
    }
}
