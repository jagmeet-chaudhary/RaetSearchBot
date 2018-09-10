using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchBot.Connectors;
using SearchBot.Model;
using SearchBot.Service.Interfaces;

namespace SearchBot.Service.HRM
{
    public class EmployeeService : IEmployeeService
    {
        IHrmConnector hrmConnector;
        public EmployeeService(IHrmConnector hrmConnector)
        {
            this.hrmConnector = hrmConnector;
        }

    

        public List<Employee> GetEmployeesByName(string firstName, string lastName)
        {
            var employees = hrmConnector.SearchEmployees(new Employee { FirstName = firstName,LastName = lastName});
            return employees;
        }

        public Employee GetManger(Employee employee)
        {
            
            var manager = hrmConnector.GetManagerForEmployee(employee);
            return manager;
        }
    }
}
