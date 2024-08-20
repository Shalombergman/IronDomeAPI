using System.Collections.Generic;
using IronDomeAPI.Models;
using Microsoft.EntityFrameworkCore;
using IronDomeAPI.Models;
namespace IronDomeAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
      
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
                Database.EnsureCreated();
            }

            public DbSet<Attack> attacks { get; set; }
           

    }
}
