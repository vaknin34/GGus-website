using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Web.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public Product Product { get; set; }
        public Cart Cart { get; set; }


    }
}
