using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Order.Services.BusServices.Messages;
using OrderApi.Services.BusServices.Enums;
using OrderApi.Services.BusServices.Handlers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderApi.Services.BusServices;

public class BusSubscriber : IBusSubscriber
{
	private readonly DefaultObjectPool<IModel> _channelPool;
	private readonly IApplicationBuilder _applicationBuilder;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public BusSubscriber(IApplicationBuilder applicationBuilder)
	{
		_applicationBuilder = applicationBuilder;
		var objectPolicy = applicationBuilder.ApplicationServices.GetRequiredService<IPooledObjectPolicy<IModel>>();
		_channelPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
		_httpContextAccessor = applicationBuilder.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
	}

	public async Task SubscribeAsync<TMessage>(
		string exchangeName,
		string queueName,
		string routingKey,
		ExchangeTypes exchangeType = ExchangeTypes.Unknown,
		ushort prefetchCount = 1,
		int retry = 0,
		CancellationToken ct = default) where TMessage : IMessage
	{
		if (exchangeType == ExchangeTypes.Unknown)
		{
			throw new ArgumentNullException(nameof(exchangeName), $"'{nameof(exchangeName)}' cannot be null or empty.");
		}

		var channel = _channelPool.Get();

		try
		{
			channel.ExchangeDeclare(exchangeName, exchangeType.ToString().ToLower(), durable: true, autoDelete: false);
			channel.QueueDeclare(queueName, durable: true, autoDelete: false, exclusive: false);
			channel.QueueBind(queueName, exchangeName, routingKey);
			channel.BasicQos(0, prefetchCount, true);

			var consumer = new AsyncEventingBasicConsumer(channel);
			consumer.Received += async (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				var messageObject = JsonSerializer.Deserialize<TMessage>(message,
					options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
				if (messageObject != null)
				{
					if (Guid.TryParse(ea.BasicProperties.MessageId, out Guid messageId))
					{
						messageObject.MessageId = messageId;
					}

					if (await TryHandleAsync(messageObject))
					{
						channel.BasicAck(ea.DeliveryTag, false);
					}
					else
					{
						channel.BasicNack(ea.DeliveryTag, false, false);
					}
				}
			};

			channel.BasicConsume(queue: queueName,
								 autoAck: false,
								 consumer: consumer);
		}
		catch(Exception ex)
		{
			throw;
		}
		finally
		{
			_channelPool.Return(channel);
		}
	}

	private async Task<bool> TryHandleAsync<TMessage>(TMessage message) where TMessage : IMessage
	{
		using var scope = _applicationBuilder.ApplicationServices.CreateScope();
		try
		{
			var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>();
			await handler.HandleAsync(message);
			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
	}
}
