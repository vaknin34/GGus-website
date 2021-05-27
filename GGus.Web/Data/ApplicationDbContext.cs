using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GGus.Web.Models;

namespace GGus.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GGus.Web.Models.Product> Product { get; set; }

        public DbSet<GGus.Web.Models.Category> Category { get; set; }

        public DbSet<GGus.Web.Models.User> User { get; set; }
    }
}
