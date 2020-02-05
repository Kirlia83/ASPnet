using System.Diagnostics.CodeAnalysis;
using Isen.Dotnet.Library.Model;
using Microsoft.EntityFrameworkCore;

namespace Isen.Dotnet.Library.Context
{    
    public class ApplicationDbContext : DbContext
    {        
        // Listes des classes modèle / tables
        public DbSet<Person> PersonCollection { get; set; }
        public DbSet<Role> RoleCollection {get; set;}
        public DbSet<Service> ServiceCollection {get; set;}


        public ApplicationDbContext(
            [NotNullAttribute] DbContextOptions options) : 
            base(options) {  }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {

        
            base.OnModelCreating(modelBuilder);

            // Tables et relations
            modelBuilder.Entity<RolePerson>()
                .HasKey(rp => new { rp.PersonId, rp.RoleId }); 

            modelBuilder.Entity<RolePerson>()
                .HasOne(rp => rp.Person)
                .WithMany(p => p.RolesPerson)
                .HasForeignKey(rp => rp.PersonId);

            modelBuilder.Entity<RolePerson>()
                .HasOne(rp => rp.Role)
                .WithMany(p => p.PersonsRole)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<Person>()
                .ToTable(nameof(Person));

            modelBuilder.Entity<Person>()
                .HasOne(p => p.ServiceMembership);

            modelBuilder.Entity<Role>()
                .ToTable(nameof(Role));

            modelBuilder.Entity<Service>()
                .ToTable(nameof(Service));
        }
    }
}