using ToDo.Microservices.Categories.API.Extensions.Startup;
using ToDo.Microservices.Categories.API.Middlewares;
using ToDo.Microservices.Middleware.Exceptions;
using ToDo.Microservices.Middleware.Identities;
using ToDo.Microservices.Middleware.Users;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddContexts(configuration);
services.AddRepositories();
services.AddServices();
services.AddValidators();
services.AddMessageQueue();
services.AddCache(configuration);
services.AddIdentity();
services.AddUserValidator<CategoriesUserValidator>();
services.AddGlobalExceptionHandler(options => options.ServiceName = nameof(ToDo.Microservices.Categories));

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("Default");
app.UseIdentity();
app.UseUserValidator();
app.MapControllers();

app.Run();
