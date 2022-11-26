using Order.Enums;

namespace Order.Entities.Common;

public abstract class BaseEntity
{
	public long Id { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public RecordStatuses Status { get; set; }

	public void Add()
	{
		CreatedDate = DateTime.Now;
		UpdatedDate = null;
		Status = RecordStatuses.Active;
	}

	public void Update()
	{
		UpdatedDate = DateTime.Now;
		Status = RecordStatuses.Active;
	}

	public void Delete()
	{
		UpdatedDate = DateTime.Now;
		Status = RecordStatuses.Passive;
	}
}
