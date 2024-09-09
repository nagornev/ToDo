using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using ToDo.Microservices.Identity.API.Contracts.Sign;
using ToDo.Microservices.Identity.Database.Contexts;
using ToDo.Microservices.Identity.Infrastructure.Providers;
using ToDo.Microservices.Identity.Infrastructure.Repositories;
using ToDo.Microservices.Identity.Infrastructure.Services;
using ToDo.Microservices.Identity.UseCases.Providers;
using ToDo.Microservices.Identity.UseCases.Repositories;
using ToDo.Microservices.Identity.UseCases.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

#region Options

IOptions<PasswordHashProviderOptions> passwordOptions = Options.Create(configuration.GetSection(nameof(PasswordHashProviderOptions))
                                                                                    .Get<PasswordHashProviderOptions>()!);

IOptions<JwtTokenProviderOptions> jwtOptions = Options.Create(configuration.GetSection(nameof(JwtTokenProviderOptions))
                                                                            .Get<JwtTokenProviderOptions>()!);

#endregion

#region Database

services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(IdentityContext))));

#endregion

#region Services

services.AddScoped<IUserService, UserService>();

#endregion

#region Repositories

services.AddScoped<IUserRepository, UserRepository>();

#endregion

#region Providers

services.AddSingleton(passwordOptions);
services.AddScoped<IHashProvider, PasswordHashProvider>();

services.AddSingleton(jwtOptions);
services.AddScoped<ITokenProvider, JwtTokenProvider>();

#endregion

#region Validators

services.AddScoped<IValidator<SignContractUp>, SignContractUpValidator>();
services.AddScoped<IValidator<SignContractIn>, SignContractInValidator>();
services.AddScoped<IValidator<SignContractAccess>, SignContractAccessValidator>();

#endregion

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
