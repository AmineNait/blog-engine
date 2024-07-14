using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BlogAdmin.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Publication date is required")]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }
    }
}
