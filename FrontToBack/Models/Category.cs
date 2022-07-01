using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "CAn not be empty"), MaxLength(30, ErrorMessage = "Can not be more than 30")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Can not be empty"), MaxLength(255, ErrorMessage = "Can not be more than 255") ]
        public string Desc { get; set; }

        public List<Product> Products { get; set; }
    }
}
