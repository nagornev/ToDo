using ToDo.Microservices.Entries.API.Middlewares;
using ToDo.Microservices.Middleware.Identities;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddIdentity<EntriesIdentityMiddleware>();

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

app.UseIdentity();

app.MapControllers();

app.Run();
