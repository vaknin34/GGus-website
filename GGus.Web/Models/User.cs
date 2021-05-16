using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GGus.Web.Models
{
    public enum UserType
    {
        Client,
        Admin
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UserType Type { get; set; } = UserType.Client;
    }
}
