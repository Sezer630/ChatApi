using Microsoft.EntityFrameworkCore;
using SimpleChatApi.Contexts;
using SimpleChatApi.Entities;

namespace SimpleChatApi.Seed
{
    public static class SeedData
    {
        /// <summary>
        /// Uygulama ilk çalıştığında veritabanı yoksa tablolar ile birlikte oluşturulur. İçersine varsayılan veriler eklenir.
        /// </summary>
        public static async ValueTask EnsurePopulatedAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await appDbContext.Database.MigrateAsync();

            if (!await appDbContext.Users.AnyAsync())
            {
                await appDbContext.Users.AddRangeAsync(
                                       new User
                                       {
                                           Fullname = "Test User 1",
                                           Username = "user1",
                                           Password = "123456",
                                       },
                                       new User
                                       {
                                           Fullname = "Test User 2",
                                           Username = "user2",
                                           Password = "123456",
                                       });

                await appDbContext.SaveChangesAsync();
            }

        }
    }
}
