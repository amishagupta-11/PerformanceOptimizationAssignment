using Microsoft.EntityFrameworkCore;
using PerformanceOptimizationAssignment.Models;

namespace PerformanceOptimizationAssignment.Data
{
    /// <summary>
    /// Database context class for the Employee entity.
    /// It manages the Employee table in the database and provides methods for querying and saving data.
    /// </summary>
    public class EmployeeDbContext : DbContext
    {        
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Represents the Employees table in the database.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }
    }
}
