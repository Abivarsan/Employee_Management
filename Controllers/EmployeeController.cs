using Employee_Management.Interface;
using Employee_Management.Models;
using Employee_Management.Service_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Employee_Management.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IHolidayService _holidayService;
        private readonly IMemoryCache _cache;

        public EmployeeController(IEmployeeService employeeService, IHolidayService holidayService, IMemoryCache cache)
        {
            _employeeService = employeeService;
            _holidayService = holidayService;
            _cache = cache;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.AddEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _employeeService.UpdateEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Employee/CalculateWorkingDays
        public IActionResult CalculateWorkingDays()
        {
            return View();
        }

        // POST: Employee/CalculateWorkingDays
        [HttpPost]
        public async Task<IActionResult> CalculateWorkingDays(DateTime StartDate, DateTime EndDate)
        {
            if (StartDate > EndDate)
            {
                ViewBag.Error = "Start date cannot be later than end date.";
                return View();
            }

            if (StartDate.DayOfWeek == DayOfWeek.Saturday || StartDate.DayOfWeek == DayOfWeek.Sunday)
            {
                ViewBag.Error = "Start date cannot be a weekend.";
                return View();
            }

            var publicHolidays = await GetCachedHolidays();

            int workingDays = 0;
            DateTime current = StartDate;

            while (current <= EndDate)
            {
                if (current.DayOfWeek != DayOfWeek.Saturday &&
                    current.DayOfWeek != DayOfWeek.Sunday &&
                    !publicHolidays.Contains(current))
                {
                    workingDays++;
                }
                current = current.AddDays(1);
            }

            ViewBag.WorkingDays = workingDays;
            return View();
        }

        // Caching Public Holidays for 5 minutes
        private async Task<List<DateTime>> GetCachedHolidays()
        {
            if (!_cache.TryGetValue("PublicHolidays", out List<DateTime> publicHolidays))
            {
                publicHolidays = await _holidayService.GetPublicHolidaysAsync();
                _cache.Set("PublicHolidays", publicHolidays, TimeSpan.FromMinutes(5));
            }
            return publicHolidays;
        }
    }
}
