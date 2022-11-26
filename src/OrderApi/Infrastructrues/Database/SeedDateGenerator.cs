using Microsoft.EntityFrameworkCore;
using Order.Entities;

namespace Order.Infrastructrues.Database;

public static class SeedDateGenerator
{
	public static async Task Initialize(IServiceProvider serviceProvider, CancellationToken ct = default)
	{
		using (var context = new OrderDbContext(serviceProvider.GetRequiredService<DbContextOptions<OrderDbContext>>()))
		{
			if (!context.Users.Any())
			{
				UserEntity user = new()
				{
					Name = "Burak",
					Surname = "Akın",
					Email = "mail@mail.com",
					PhoneNumber = "05325320000",
					Balance = 30000,
					EmailNotification = true,
					SmsNotification = false,
					PushNotification = true,
				};
				user.Add();
				await context.Users.AddAsync(user, ct);
			}

			await context.SaveChangesAsync(ct);
		}
	}
}
