using EImzaTakip.Models.Entities;
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
            //User-Role
            modelBuilder.Entity<User>()
                    .HasOne(x => x.Role)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

            //Person-Department
            modelBuilder.Entity<Person>()
                    .HasOne(x=>x.Department)
                    .WithMany(x=>x.Persons)
                    .HasForeignKey(x=>x.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

            //Person-Person Note
            modelBuilder.Entity<PersonNote>()
                    .HasOne(x=>x.Person)
                    .WithMany()
                    .HasForeignKey(x=>x.PersonId)
                    .OnDelete(DeleteBehavior.Cascade);

            //Person-Certificate
            modelBuilder.Entity<Certificate>()
                    .HasOne(x=>x.Person)
                    .WithMany(x=>x.Certificates)
                    .HasForeignKey(x=>x.PersonId)
                    .OnDelete(DeleteBehavior.Cascade);

            //User
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

            //Role
            modelBuilder.Entity<Role>()
                .Property(x=>x.Name)
                .HasMaxLength(100);

            //Department
            modelBuilder.Entity<Department>()
                .Property(x=>x.Name)
                .HasMaxLength(100);

            //Person
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

            //Certificate
            modelBuilder.Entity<Certificate>()
                .Property(x=>x.CertificateName)
                .HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }
    }
}
