global using System;
global using System.Collections.Generic;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructures.Database;
using Order.Services.AutoMapper;
using Order.Services.BusServices.Extensions;
using OrderApi.Infrastructures.Cache.Redis;
using OrderApi.Infrastructures.MessageBroker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<OrderDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "OrderDb"));
builder.Services.AddControllers()
	.AddNewtonsoftJson()
	.AddFluentValidation(opt =>
	{
		opt.ImplicitlyValidateChildProperties = true;
		opt.ImplicitlyValidateRootCollectionElements = true;

		List<Assembly> assemblies = new();
		assemblies.Add(Assembly.GetExecutingAssembly());
		opt.RegisterValidatorsFromAssemblies(assemblies);
	});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRedisManager(builder.Configuration);
builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddHandlers(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

await app.SubscribeQueues();

app.Run();
