using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public ICollection<Post> Posts { get; set; }

        // Constructeur pour initialiser les propriétés
        public Category()
        {
            Title = string.Empty;
            Posts = new List<Post>();
        }
    }
}
