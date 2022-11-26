using Microsoft.EntityFrameworkCore;
using Order.Entities;

namespace Order.Infrastructrues.Database;

public class OrderDbContext : DbContext
{
	public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
	{
	}

	public virtual DbSet<UserEntity> Users { get; set; } = null!;
	public virtual DbSet<OrderEntity> Orders { get; set; } = null!;
	public virtual DbSet<NotificationHistoryEntity> NotificationHistory { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserEntity>(e =>
		{
			e.HasKey(k => k.Id);
			e.Property(p => p.Email).HasMaxLength(100);
			e.Property(p => p.PhoneNumber).HasMaxLength(50);
		});

		modelBuilder.Entity<OrderEntity>(e =>
		{
			e.HasKey(k => k.Id);
		});

		modelBuilder.Entity<NotificationHistoryEntity>(e =>
		{
			e.HasKey(k => k.Id);
		});
	}
}
