using CNC.Services;
using Common.Protocol;
using Common.Protocol.Abstraction;
using Common.Serializer;
using Common.Serializer.Abstraction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<UsersStorageService>();
builder.Services.AddSingleton<IProtocol, DefaultProtocol>();
builder.Services.AddSingleton<ISerializer, JsonSerializer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
