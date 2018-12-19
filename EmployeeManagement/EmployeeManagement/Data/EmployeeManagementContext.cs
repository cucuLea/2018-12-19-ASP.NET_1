using Microsoft.EntityFrameworkCore;
namespace EmployeeManagement.Models
{
    public class EmployeeManagementContext : DbContext
    {
        public EmployeeManagementContext (DbContextOptions<EmployeeManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
    }
}
