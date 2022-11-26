namespace Order.Services.BusServices.Messages;

public interface IMessage
{
	public Guid MessageId { get; set; }
}
