using System.ComponentModel.DataAnnotations;

namespace NoteListAPI.DomainLayer.Models
{
    public class TodoList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
