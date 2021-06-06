using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Web.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50,MinimumLength = 5)]
        [Required]
        public string Name { get; set; }
        [StringLength(50, MinimumLength = 5)]
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [Range(0,500)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Display(Name = "Category Id")]
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        [Display(Name = "Photo url")]
        public string PhotosUrl1 { get; set; }
        [Display(Name = "Photo url")]
        [Required]
        public string PhotosUrl2 { get; set; }
        [Display(Name = "Photo url")]
        [Required]
        public string PhotosUrl3 { get; set; }
        [Display(Name = "Photo url")]
        [Required]
        public string PhotosUrl4 { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        [Display(Name = "Trailer Url")]
        public string TrailerUrl { get; set; }
        [Required]
        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        public IList<CartProduct> CartProducts { get; set; }
    }
}
