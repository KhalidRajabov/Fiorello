using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontToBack.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Can not be empty"), MaxLength(30, ErrorMessage = "Can not be more than 30")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
        [Required(ErrorMessage = "Can not be empty")]
        public double Price { get; set; }
        [Required(ErrorMessage ="Choose a category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required(ErrorMessage = "Can not be empty")]
        public int Count { get; set; } 
    }
}
