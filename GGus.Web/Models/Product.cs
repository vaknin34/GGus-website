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
        public string Name { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
        public string PhotosUrl1 { get; set; }
        public string PhotosUrl2 { get; set; }
        public string PhotosUrl3 { get; set; }
        public string PhotosUrl4 { get; set; }
        public string PhotosUrl5 { get; set; }
        public string Details { get; set; }
        public string TrailerUrl { get; set; }

    }
}
