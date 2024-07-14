using System.Collections.Generic;

namespace BlogAdmin.Models
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
