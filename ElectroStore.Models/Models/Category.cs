using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.Models.Models
{

    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter the Category Name")]
        public string Name { get; set; }
    }
}
