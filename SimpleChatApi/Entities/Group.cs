using System.ComponentModel.DataAnnotations;

namespace SimpleChatApi.Entities
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
