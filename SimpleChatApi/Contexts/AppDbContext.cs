using Microsoft.EntityFrameworkCore;
using SimpleChatApi.Entities;

namespace SimpleChatApi.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Sender)
                .WithMany()
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Eylemi "NO ACTION" olarak değiştirme

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<GroupUsers> GroupUserss { get; set; }

    }
}
