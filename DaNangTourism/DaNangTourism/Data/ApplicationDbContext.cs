using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DaNangTourism.Models;
using Microsoft.AspNetCore.Identity;

namespace DaNangTourism.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduleDestination>()
                .HasKey(s => s.SDID);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProcessingReport>()
                .HasKey(s => s.prcReportID);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<DaNangTourism.Models.Destination> Destination { get; set; } = default!;
        public DbSet<DaNangTourism.Models.Review> Review { get; set; } = default!;
        public DbSet<DaNangTourism.Models.Schedule> Schedule { get; set; } = default!;
        public DbSet<DaNangTourism.Models.ProcessingReport> ProcessingReport { get; set; } = default!;
        public DbSet<DaNangTourism.Models.ScheduleDestination> ScheduleDestination { get; set; } = default!;
    }
}
