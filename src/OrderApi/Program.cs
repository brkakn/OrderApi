global using System;
global using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructrues.Database;
using Order.Services.BusServices.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<OrderDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "OrderDb"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRabbitMQ(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await SeedDateGenerator.Initialize(scope.ServiceProvider);
}

app.Run();
