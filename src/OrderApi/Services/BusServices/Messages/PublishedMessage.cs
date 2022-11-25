using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Order.Services.BusServices.Messages;

[NotMapped]
public abstract record PublishedMessage
{
    private readonly string _exchangeName;
    private readonly string _routingKey;

    protected PublishedMessage(string exchangeName, string routingKey)
    {
        _exchangeName = exchangeName.ToLower();
        _routingKey = routingKey.ToLower();
    }

    [JsonIgnore]
    public string ExchangeName { get => _exchangeName; }

    [JsonIgnore]
    public string RoutingKey { get => _routingKey; }

    public Guid MessageId { get; set; }
}