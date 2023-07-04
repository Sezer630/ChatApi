using System.ComponentModel.DataAnnotations;

namespace SimpleChatApi.Entities
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int RecipientId { get; set; }
        public virtual User Recipient { get; set; }
    }
}
