using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Web.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        
        public List<Product> Products { get; set; }

        public double TotalPrice { get; set; } = 0;



    }
}
