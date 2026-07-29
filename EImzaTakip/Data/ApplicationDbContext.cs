using EImzaTakip.Models.Entities;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace EImzaTakip.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonNote> PersonNotes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER - ROLE
            modelBuilder.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // PERSON - DEPARTMENT
            modelBuilder.Entity<Person>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Persons)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // PERSON - PERSON NOTE
            modelBuilder.Entity<PersonNote>()
                .HasOne(x => x.Person)
                .WithMany(x => x.PersonNotes)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // PERSON - CERTIFICATE
            modelBuilder.Entity<Certificate>()
                .HasOne(x => x.Person)
                .WithMany(x => x.Certificates)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER
            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(x => x.Surname)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(x => x.NickName)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .HasMaxLength(100);

            // ROLE
            modelBuilder.Entity<Role>()
                .Property(x => x.Name)
                .HasMaxLength(100);

            // DEPARTMENT
            modelBuilder.Entity<Department>()
                .Property(x => x.Name)
                .HasMaxLength(150);

            // PERSON
            modelBuilder.Entity<Person>()
                .Property(x => x.IdentityNumber)
                .HasMaxLength(11);

            modelBuilder.Entity<Person>()
                .Property(x => x.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Person>()
                .Property(x => x.Surname)
                .HasMaxLength(100);

            modelBuilder.Entity<Person>()
                .Property(x => x.Email)
                .HasMaxLength(100);

            // CERTIFICATE
            modelBuilder.Entity<Certificate>()
                .Property(x => x.CertificateName)
                .HasMaxLength(100);

            // ROLE SEED
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Status=true
                },
                new Role
                {
                    Id = 2,
                    Name = "Editör",
                    Status =true
                }
            );

            // USER SEED
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Surname = "Admin",
                    NickName = "admin",
                    Email = "admin@test.com",
                    Status=true,
                    //Password = "123456",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    RoleId = 1
                }
            );
        }
    }
}
