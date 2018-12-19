using System.Collections.Generic;


namespace EmployeeManagement.Services
{
    public class EmployeeOptionsService
    {
        public List<string> ListGenders()
        {
            return new List<string>() {"F", "M" };
        }

        public List<string> ListDepartments()
        {
            return new List<string>() { "develop", "manager" ,"test" };
        }

        public List<string> ListDepartments_2()
        {
            return new List<string>() { " ","develop", "manager", "test" };
        }

    }
}
