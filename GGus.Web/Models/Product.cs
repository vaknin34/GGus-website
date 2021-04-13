using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }

    }
}
