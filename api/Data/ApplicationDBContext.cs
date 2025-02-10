using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace api.Data
{
    public class ApplicationDBContext :  IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }     

        public DbSet<Stock> Stock { get; set;}
        public DbSet<Comment> Comment { get; set;}
        public DbSet<Portfolio> Portfolios{ get; set;}
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId}));

            modelBuilder.Entity<Portfolio>()
            .HasOne(u => u.AppUser)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.AppUserId);


            modelBuilder.Entity<Portfolio>()
            .HasOne(u => u.Stock)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.StockId);

            // Configure cascade delete
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Stock)
                .WithMany(s => s.Comments) // Assuming Stock has a navigation property ICollection<Comment> Comments
                .HasForeignKey(c => c.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{
                    // Id = "1b2a3c4d-5678-90ab-cdef-1234567890ab",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                 new IdentityRole{
                    // Id = "2c3d4e5f-6789-01bc-defg-2345678901bc",
                    Name = "User",
                    NormalizedName = "USER"
                },

                // new IdentityRole{
                //     // Id = "2c3d4e5f-6789-01bc-defg-2345678952aa",
                //     Name = "Test",
                //     NormalizedName = "TEST"
                // },
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    }
}