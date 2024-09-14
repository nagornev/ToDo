using ToDo.Microservices.Entries.API.Middlewares;
using ToDo.Microservices.Middleware.Identities;
using ToDo.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddRedirectableQuererHttpClientFactory();

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
