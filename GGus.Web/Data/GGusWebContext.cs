using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GGus.Web.Models;

namespace GGus.Web.Data
{
    public class GGusWebContext : DbContext
    {
        public GGusWebContext (DbContextOptions<GGusWebContext> options)
            : base(options)
        {
        }

        public DbSet<GGus.Web.Models.User> User { get; set; }
    }
}
