
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace EmployeeManagement.Controllers
{   
    public class EmployeesController : Controller
    {
        private readonly EmployeeManagementContext _context;

        public EmployeesController(EmployeeManagementContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task<IActionResult> Index(
            string searchFirstName, string searchDepartment,
            int? page,
            string currentOrderParam,string sortOrderParam ="FirstName", string sortOrder="asc" 
            )
        {
           // if (!IsLogin()) { return RedirectToAction("Login", "Login"); } //login already?

            IQueryable<Employee> employees = _context.Employee;

            employees = SearchEmployees(searchFirstName,searchDepartment,employees);
            //sort     
            sortOrder = JudgeSortOrder(sortOrderParam, currentOrderParam, sortOrder);            
            employees = SortEmployees(sortOrder, sortOrderParam, employees);


            int pageSize = Configuration.GetSection("Constant").GetValue<int>("PageSize");
            PaginatedList<Employee> pageShow = await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), page ?? 1, pageSize);
            EmployeeIndexViewModel employeeIndexViewModel = new EmployeeIndexViewModel(searchFirstName, searchDepartment, sortOrderParam, sortOrder, pageShow);

            return View(employeeIndexViewModel);
        }

        // GET: Employees/Details
        public async Task<IActionResult> Details(int? id)
        {
 
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.ID == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Gender,Birth,Department,Address,Phone,Email")] Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(employee);
        }

        // GET: Employees/Edit/
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ID,FirstName,LastName,Gender,Birth,Department,Address,Phone,Email")] Employee employee)
        {
           if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                }
            }
            return View(employee);
        }

        // GET: Employees/Delete/
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (employee == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(employee);
        }

        // POST: Employees/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var employee = await _context.Employee
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (employee == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id, saveChangesError = true });
            }

        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.ID == id);
        }

        private IQueryable<Employee> SortEmployees(string sortOrder, string sortOrderParam,IQueryable<Employee> employees)
        {
            employees = employees.OrderBy($"{sortOrderParam} {sortOrder}");
            //Expression<Func<Employee,object>> sortExpression;
            //switch (sortOrderParam)
            //{  
            //    case "FirstName":
            //        sortExpression = e => e.FirstName;
            //        break;
            //    case "LastName":
            //        sortExpression = e => e.LastName;
            //        break;
            //    case "Gender":
            //        sortExpression = e => e.Gender; 
            //        break;
            //    case "Birth":
            //        sortExpression = e => e.Birth; 
            //        break;
            //    case "Department":
            //        sortExpression = e => e.Department; 
            //        break;
            //    case "Address":
            //        sortExpression = e => e.Address; 
            //        break;
            //    case "Phone":
            //        sortExpression = e => e.Phone;
            //        break;
            //    case "Email":
            //        sortExpression = e => e.Email; 
            //        break;
            //    default:
            //        sortExpression = e => e.FirstName;
            //        break;
            //}
            //if ("desc".Equals(sortOrder))
            //{
            //    employees=employees.OrderByDescending(sortExpression);
            //}
            //else
            //{
            //    employees = employees.OrderBy(sortExpression);
            //}
            return employees;
        }

        private IQueryable<Employee> SearchEmployees(string searchFirstName, string searchDepartment, IQueryable<Employee> employees)
        {
            if (!string.IsNullOrEmpty(searchFirstName))
            {
                employees = employees.Where(s => s.FirstName.Contains(searchFirstName.Trim()));
            }
            if (!string.IsNullOrEmpty(searchDepartment))
            {
                employees = employees.Where(s => s.Department.Contains(searchDepartment.Trim()));
            }
            return employees;
        }

        private string JudgeSortOrder(string sortOrderParam,string currentOrderParam,string sortOrder)
        {
            if (!string.IsNullOrEmpty(currentOrderParam))
            {
                if (sortOrderParam.Equals(currentOrderParam))
                {
                    sortOrder = "desc".Equals(sortOrder) ? "asc" : "desc";
                }
                else
                {
                    sortOrder = "asc";
                }
            }
            return sortOrder;
        }

        
    }
}
