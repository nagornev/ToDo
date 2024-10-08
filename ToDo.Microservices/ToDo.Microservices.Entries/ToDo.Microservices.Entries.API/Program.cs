using ToDo.Microservices.Entries.API.Extensions.Startup;
using ToDo.Microservices.Entries.API.Middlewares;
using ToDo.Microservices.Middleware.Exceptions;
using ToDo.Microservices.Middleware.Identities;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOptions(configuration);
services.AddContexts(configuration);
services.AddRepositories();
services.AddServices();
services.AddProviders();
services.AddValidators();
services.AddToDoMessageQueue();
services.AddIdentity();
services.AddIdentityChecker<EntriesIdentityChecker>();
services.AddGlobalExceptionHandlerConfiguration(options => options.ServiceName = nameof(ToDo.Microservices.Entries));

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
app.UseIdentity<EntriesIdentityMiddleware>();
app.MapControllers();

app.Run();
