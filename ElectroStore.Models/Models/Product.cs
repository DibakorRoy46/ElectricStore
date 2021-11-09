using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.Models.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter the Product Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter the Product Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please Enter the Product Price")]
        public decimal Price { get; set; }
        
        public decimal? DiscountPrice { get; set; }
        [Required(ErrorMessage = "Please Enter the Product Short Description")]
        public string ShortDes { get; set; }
        [Required(ErrorMessage = "Please Enter the Product Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter the Product Quantity")]
        public int Quantity { get; set; }  
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Please Enter the Product Category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required(ErrorMessage = "Please Enter the Product Brand")]
        [Display(Name="Brand")]
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }

    }
}
