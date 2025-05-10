using DeltaBrainJSC.Enum;
using DeltaBrainJSC.Models;
using DeltaBrainJSC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace DeltaBrainJSC.DB
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
       : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình Employee
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .HasMaxLength(11);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Sex)
                    .HasDefaultValue(Sex.Male);
            });

            // Cấu hình Department
            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Khóa ngoại
            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne(de => de.Employee)
                .WithMany(e => e.DepartmentEmployees)
                .HasForeignKey(de => de.EmployeeId);

            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne(de => de.Department)
                .WithMany(d => d.DepartmentEmployees)
                .HasForeignKey(de => de.DepartmentId);
        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentEmployee> DepartmentEmployee { get; set; }
    }
}
