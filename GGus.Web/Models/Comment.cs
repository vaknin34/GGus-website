using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Web.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public  String text { get; set; }

        public DateTime date { get; set; }

        public int Productid { get; set; }

        public Product product { get; set; }


    }
}
