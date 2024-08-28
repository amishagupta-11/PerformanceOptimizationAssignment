using System.ComponentModel.DataAnnotations;

namespace PerformanceOptimizationAssignment.Models
{
    /// <summary>
    /// Represents basic employee details.
    /// </summary>
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Position { get; set; }
        [Required]
        public decimal Salary { get; set; }
    }
}
