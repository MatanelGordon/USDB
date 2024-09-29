using CNC.Communicators;
using CNC.Communicators.Abstraction;
using CNC.Services;
using Common.Compression;
using Common.Compression.Abstraction;
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
builder.Services.AddSingleton<ICommunicator, TcpCommunicator>();
builder.Services.AddSingleton<ICompression, ZstdCompression>();


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
