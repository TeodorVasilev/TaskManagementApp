using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagmentApp.DAL.Configuration;
using TaskManagmentApp.DAL.Models.Account;
using TaskManagmentApp.DAL.Models.Assignments;

namespace TaskManagmentApp.DAL.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TaskManagmentApp"))
                    .AddJsonFile("appsettings.json")
                    .Build();
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());

            //seed admin user to database
            var adminUser = new AppUser
            {
                Id = 1,
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Name = "Admin",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Test123!");

            modelBuilder.Entity<AppUser>().HasData(adminUser);
            modelBuilder.Entity<IdentityUserRole<int>>().HasData
                (
                    new IdentityUserRole<int>
                    {
                        UserId = 1,
                        RoleId = 1,
                    }
                );

            var employeeUser = new AppUser
            {
                Id = 2,
                Email = "employee@employee.com",
                NormalizedEmail = "EMPLOYEE@EMPLOYEE.COM",
                EmailConfirmed = true,
                UserName = "employee@employee.com",
                NormalizedUserName = "EMPLOYEE@EMPLOYEE.COM",
                Name = "Employee",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            employeeUser.PasswordHash = ph.HashPassword(employeeUser, "Test123!");

            modelBuilder.Entity<AppUser>().HasData(employeeUser);
            modelBuilder.Entity<IdentityUserRole<int>>().HasData
                (
                    new IdentityUserRole<int>
                    {
                        UserId = 2,
                        RoleId = 2,
                    }
                );

            //seed assignment to the database
            var assignment = new Assignment
            {
                Id = 1,
                Title = "TestAssignment",
                Description = "TestAssignmentDescription",
                CreatedAt = DateTime.Now,
                DueDate = DateTime.Now.AddHours(1),
                CompletedAt = DateTime.MinValue,
                Status = AssignmentStatus.InProgress,
                UserId = 2
            };

            modelBuilder.Entity<Assignment>().HasData(assignment);
        }
    }
}
