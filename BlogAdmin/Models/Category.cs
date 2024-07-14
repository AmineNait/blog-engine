using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogAdmin.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
