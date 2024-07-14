using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required(ErrorMessage = "Publication date is required")]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        // Constructeur pour initialiser les propriétés
        public Post()
        {
            Title = string.Empty;
            Content = string.Empty;
            Category = new Category(); // Initialisation de la propriété Category
        }
    }
}
