using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PerformanceOptimizationAssignment.Data;
using PerformanceOptimizationAssignment.Models;
using System.Text.Json;

namespace PerformanceOptimizationAssignment.Controllers
{
    /// <summary>
    /// API methods to perform operations and use the redis cache mechanism
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IDistributedCache _cache;

        public EmployeeController(EmployeeDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Retrieves all employees from the cache if available, otherwise fetches from the database.
        /// If data is fetched from the database, it is also cached.
        /// </summary>
        /// <returns>List of all employees</returns>
     
        [HttpGet("all")]
        public IActionResult GetAllEmployees()
        {
            string cacheKey = "EmployeeList";
            string? cachedEmployeeList = _cache.GetString(cacheKey);

            if (!string.IsNullOrEmpty(cachedEmployeeList))
            {
                // Return cached data
                return Ok(JsonSerializer.Deserialize<List<Employee>>(cachedEmployeeList));
            }

            // Fetch data from SQL Server
            var employeeList = _context.Employees.ToList();

            if (employeeList == null || employeeList.Count == 0)
            {
                return NotFound();
            }

            // Cache the data
            var serializedEmployeeList = JsonSerializer.Serialize(employeeList);
            _cache.SetString(cacheKey, serializedEmployeeList, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)  // Cache duration
            });

            return Ok(employeeList);
        }

        /// <summary>
        /// Retrieves a specific employee by ID from the cache if available, otherwise fetches from the database.
        /// If data is fetched from the database, it is also cached.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            string cacheKey = $"Employee_{id}";
            string? cachedEmployee = _cache.GetString(cacheKey);

            if (!string.IsNullOrEmpty(cachedEmployee))
            {
                // Returns cached data
                return Ok(JsonSerializer.Deserialize<Employee>(cachedEmployee));
            }

            // Fetch data from SQL Server
            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            // Caches the data if not found
            var serializedEmployee = JsonSerializer.Serialize(employee);
            _cache.SetString(cacheKey, serializedEmployee, new DistributedCacheEntryOptions
            {
                // setting the expiration time for caching data.
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)  
            });

            return Ok(employee);
        }

        /// <summary>
        /// Adds a new employee or updates an existing employee in the database.
        /// The employee data is also updated in the cache.
        /// </summary>
        /// <param name="employee">Employee object</param>
        /// <returns>The added or updated employee</returns>
        
        [HttpPost]
        public IActionResult AddOrUpdateEmployee(Employee employee)
        {
            var existingEmployee = _context.Employees.Find(employee.Id);

            if (existingEmployee == null)
            {
                // Adding new employee
                _context.Employees.Add(employee);
            }
            else
            {
                // Updating existing employee
                existingEmployee.Name = employee.Name;
                existingEmployee.Position = employee.Position;
                existingEmployee.Salary = employee.Salary;
            }

            _context.SaveChanges();

            // Update the cache
            string cacheKey = $"Employee_{employee.Id}";
            var serializedEmployee = JsonSerializer.Serialize(employee);
            _cache.SetString(cacheKey, serializedEmployee, new DistributedCacheEntryOptions
            {
                // setting the expiration time for caching data.
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)  
            });

            return Ok(employee);
        }

        /// <summary>
        /// Deletes an employee by ID from the database.
        /// The employee data is also removed from the cache.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Confirmation message</returns>

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            // Remove data from cache
            string cacheKey = $"Employee_{id}";
            _cache.Remove(cacheKey);

            return Ok("Employee Deleted");
        }
    }
}
