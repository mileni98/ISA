using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Hospital.Models;

namespace Hospital.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Hospital.Models.Pharmacy> Pharmacy { get; set; }
        public DbSet<Hospital.Models.Review> Review { get; set; }
        public DbSet<Hospital.Models.WorkingContract> WorkingContract { get; set; }
        public DbSet<Hospital.Models.Drug> Drug { get; set; }
        public DbSet<Hospital.Models.Vacation> Vacation { get; set; }
    }
}
