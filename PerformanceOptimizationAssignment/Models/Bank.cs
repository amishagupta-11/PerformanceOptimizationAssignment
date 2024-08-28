namespace PerformanceOptimizationAssignment.Models
{
    /// <summary>
    /// Represents a bank with its basic details.
    /// </summary>
    public class Bank
    {
        public int BankId { get; set; }
        public string? BankName { get; set; }
        public string? Address { get; set; }
        public string? BranchCode { get; set; }
    }

}
