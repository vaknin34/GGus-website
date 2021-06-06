using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Web.Models
{
    public class Cart
    {
       [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }

        public User User { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        public double TotalPrice { get; set; } = 0;


        public IList<CartProduct> CartProducts { get; set; }

    }
}
