using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Casterr.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> movies { get; set; }
        public DbSet<Tvshow> tvshows { get; set; }
        public DbSet<Episode> episodes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Seasons>().HasOne(s => s.show).WithMany(b => b.seasons).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Episode>().HasOne(s => s.show).WithMany(b => b.episodes).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
