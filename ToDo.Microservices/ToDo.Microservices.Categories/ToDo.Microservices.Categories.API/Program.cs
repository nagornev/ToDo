using ToDo.Microservices.Categories.API.Extensions.Startup;
using ToDo.Microservices.Categories.API.Middlewares;
using ToDo.Microservices.Middleware.Exceptions;
using ToDo.Microservices.Middleware.Identities;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddContexts(configuration);
services.AddRepositories();
services.AddServices();
services.AddValidators();
services.AddToDoMessageQueue();
services.AddIdentity();
services.AddIdentityChecker<CategoriesIdentityChecker>();
services.AddGlobalExceptionHandler(options => options.ServiceName = nameof(ToDo.Microservices.Categories));

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
app.UseIdentity<CategoriesIdentityMiddleware>();
app.MapControllers();

app.Run();
