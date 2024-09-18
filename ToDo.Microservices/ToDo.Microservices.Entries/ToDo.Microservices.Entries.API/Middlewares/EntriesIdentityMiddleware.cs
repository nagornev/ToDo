﻿using ToDo.Domain.Results;
using ToDo.Microservices.Entries.UseCases.Services;
using ToDo.Microservices.Middleware.Identities;
namespace ToDo.Microservices.Entries.API.Middlewares
{
    public class EntriesIdentityMiddleware : IdentityMiddleware
    {
        private IUserService _userService;

        public EntriesIdentityMiddleware(RequestDelegate next, IUserService userService) 
            : base(next)
        {
            _userService = userService;
        }

        protected override bool TryGetIdentity(HttpContext context, out IdentityAttribute? attribute)
        {
            attribute = context.GetEndpoint()?
                               .Metadata
                               .GetMetadata<IdentityAttribute>();

            return attribute is not null;
        }

        protected override async Task<Result> Check(Guid userId)
        {
            return await _userService.GetUser(userId);
        }
    }
}
