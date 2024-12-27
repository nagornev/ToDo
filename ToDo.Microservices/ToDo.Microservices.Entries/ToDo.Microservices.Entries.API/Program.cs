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
services.AddMessageQueue();
services.AddCache(configuration);
services.AddIdentity();
services.AddIdentityChecker<EntriesIdentityChecker>();
services.AddGlobalExceptionHandler(options => options.ServiceName = nameof(ToDo.Microservices.Entries));

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        //policy.WithOrigins("http://identity_microservice:7000",
        //                   "http://entries_microservice:7001",
        //                   "http://categories_microservice:7002",
        //                   "http://localhost");

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
app.UseIdentity<EntriesIdentityMiddleware>();
app.MapControllers();

app.Run();
