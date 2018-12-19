using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class ValidationController : Controller
    {
        private readonly EmployeeManagementContext _context;

        public ValidationController(EmployeeManagementContext context)
        {
            _context = context;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyEmail(string email,int id)
        {
            if (EmailExists(email,id))
            {
                return Json($"Email {email} is already in use.");
            }
            return Json(true);
        }

        private bool EmailExists(string email,int id)
        {
            return _context.Employee.Any(e => email.Equals(e.Email)) &&! _context.Employee.Any(e => e.ID==id);
        }
    }
}