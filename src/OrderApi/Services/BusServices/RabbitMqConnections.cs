using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Order.Services.BusServices.Helpers;
using Order.Services.BusServices.Models;
using RabbitMQ.Client;

namespace Order.Services.BusServices;

public class RabbitMqConnections : IPooledObjectPolicy<IModel>
{
	private readonly RabbitMqConfigModel _configuration;
	private readonly IConnection _connection;

	public RabbitMqConnections(IOptions<RabbitMqConfigModel> configuration)
	{
		_configuration = configuration.Value;
		_connection = GetConnection();
	}

	private IConnection GetConnection()
	{
		var factory = new ConnectionFactory()
		{
			HostName = _configuration.Hostname,
			UserName = _configuration.Username,
			Password = _configuration.Password,
			Port = _configuration.Port,
			VirtualHost = VirtualHostSelector.GetVirtualHostByOS(_configuration.VHost),
			DispatchConsumersAsync = true
		};

		return factory.CreateConnection();
	}

	public IModel Create()
	{
		return _connection.CreateModel();
	}

	public bool Return(IModel obj)
	{
		if (obj.IsOpen)
		{
			return true;
		}
		else
		{
			obj?.Dispose();
			return false;
		}
	}
}
