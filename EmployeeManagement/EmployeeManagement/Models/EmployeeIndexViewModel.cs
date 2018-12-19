using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeeIndexViewModel
    {
        
        public string SearchFirstName { get; set; }
        public string SearchDepartment { get; set; }
        public string SortOrderParam { get; set; }
        public string SortOrder { get; set; }
        public PaginatedList<Employee> PageShow { get; set; }

        public EmployeeIndexViewModel(string searchFirstName, string searchDepartment, string sortOrderParam, string sortOrder,PaginatedList<Employee> pageShow)
        {
            SearchFirstName = searchFirstName;
            SearchDepartment = searchDepartment;
            SortOrderParam=sortOrderParam;
            SortOrder = sortOrder;
            PageShow = pageShow;
        }
    }
}
