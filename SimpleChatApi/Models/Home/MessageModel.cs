namespace SimpleChatApi.Models.Home
{
    public class MessageModel
    {
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
