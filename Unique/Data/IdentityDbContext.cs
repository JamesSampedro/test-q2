using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unique.Models;

namespace Unique.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext( DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        DbSet<ApplicationUser> applicationUsers { get; set; }

    }
}
