using Microsoft.EntityFrameworkCore;
using PatientApi.Models;

namespace PatientApi.Database.Contexts
{
    public class PatientContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; } = null!;

        public PatientContext(DbContextOptions<PatientContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var patientModelBuilder = modelBuilder.Entity<Patient>();
            patientModelBuilder.ToTable("Patient").HasKey(p => p.Id);
            patientModelBuilder.Property(p => p.Gender).HasConversion<string>();
            patientModelBuilder.HasOne(p => p.PatientName).WithOne().OnDelete(DeleteBehavior.Cascade);

            var patientNameModelBuilder = modelBuilder.Entity<PatientName>();
            patientNameModelBuilder.ToTable("PatientName").HasKey(pn => pn.Id);
            patientNameModelBuilder.Property(p => p.Family).IsRequired();
            patientNameModelBuilder.Property(pn => pn.Given)
                .HasConversion(l => string.Join(';', l), s => s.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
