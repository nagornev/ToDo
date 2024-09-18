using ToDo.Microservices.Entries.API.Middlewares;
using ToDo.Microservices.Middleware.Identities;
using ToDo.Microservices.Entries.API.Extensions.Startup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOptions(configuration);
services.AddContexts(configuration);
services.AddRepositories();
services.AddServices();
services.AddProviders();
services.AddValidators();
services.AddIdentity();

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
app.UseIdentity<EntriesIdentityMiddleware>();
app.MapControllers();

app.Run();
