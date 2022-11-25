namespace Order.Services.BusServices.Models;

public record RabbitMqConfigModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Hostname { get; set; } = null!;
    public int Port { get; set; } = 5672;
    public string VHost { get; set; } = "/";
}
