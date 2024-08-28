using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PerformanceOptimizationAssignment.Models;

namespace PerformanceOptimizationAssignment.Controllers
{
    /// <summary>
    /// Controller to display the usecase of outputcache attribute.
    /// </summary>
    public class BankController : Controller
    {
        // Static list of banks to simulate a data source
        private static readonly List<Bank>? banks =
        [
            new Bank { BankId = 1, BankName = "State Bank of India", Address = "Hitech city", BranchCode = "FNB001" },
            new Bank { BankId = 2, BankName = "Punjab National Bank", Address = "Madhapur", BranchCode = "GB002" },
            new Bank { BankId = 3, BankName = "Bank of Baroda", Address ="Ameerpet", BranchCode = "BB006" },
        ];

        /// <summary>
        /// Retrieves and displays the list of banks. The result is cached for 10 seconds
        /// to optimize performance by reducing redundant processing.
        /// </summary>
        /// <returns>View displaying the list of banks with the fetch time.</returns>
        
        [OutputCache(Duration = 10)]
        public IActionResult List()
        {
            // Store the current fetch time to demonstrate caching
            ViewData["FetchTime"] = DateTime.Now;

            // Return the view with the list of banks
            return View(banks);
        }
    }
}
