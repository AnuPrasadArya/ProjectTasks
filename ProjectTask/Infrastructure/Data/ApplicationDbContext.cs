using EmployeeService;
using Microsoft.EntityFrameworkCore;
using ProjectTask.Domain.Entities;
using System.Collections.Generic;

namespace ProjectTask.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectTasks> ProjectTasks { get; set; }      
       
        public DbSet<EmployeeId> EmployeeId { get; set; }
        public DbSet<Employees> Employees { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}
