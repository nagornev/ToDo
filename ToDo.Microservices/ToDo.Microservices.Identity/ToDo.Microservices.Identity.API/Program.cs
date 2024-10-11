using ToDo.Microservices.Identity.API.Extensions.Startup;
using ToDo.Microservices.Middleware.Exceptions;
using ToDo.Microservices.MQ;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOptions(configuration);
services.AddContexts(configuration);
services.AddRepositories(configuration);
services.AddServices(configuration);
services.AddMessageQueue();
services.AddProviders();
services.AddValidators();
services.AddGlobalExceptionHandler(options => options.ServiceName = nameof(ToDo.Microservices.Identity));

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
app.MapControllers();

app.Run();
