using System.ComponentModel.DataAnnotations;

namespace SimpleChatApi.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Fullname { get; set; }
    }
}
