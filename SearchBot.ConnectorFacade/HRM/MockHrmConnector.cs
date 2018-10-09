
using SearchBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SearchBot.Connectors.Mocks
{
    public class MockHrmConnector : IHrmConnector
    {
        public List<Employee> SearchEmployees(Employee employee)
        {

            var employees =  new List<Employee>()
            {
                new Employee
                {
                    FirstName = "John",
                    LastName   = "Smith"
                },
                 new Employee
                {
                    FirstName = "John",
                    LastName   = "Doe"
                },
                  new Employee
                {
                    FirstName = "John",
                    LastName   = "Stein"
                }

            };

            return employees.Where(x => (String.IsNullOrEmpty(employee.FirstName) || employee.FirstName.Equals(x.FirstName, StringComparison.OrdinalIgnoreCase)) &&
            (String.IsNullOrEmpty(employee.LastName) || employee.LastName.Equals(x.LastName, StringComparison.OrdinalIgnoreCase))).ToList();


        }

        public Employee GetEmployeeManager(Employee employee)
        {
            return new Employee()
            {
                FirstName = "Mark",
                LastName = "Sanders"

            };
        }

        public Employee GetManagerForEmployee(Employee employee)
        {
            return new Employee()
            {
                FirstName = "Jason",
                LastName = "Smith"
            };
        }


        public AuditChangeContextDto GetOrgUnitByName(string orgUnitName)
        {
            throw new NotImplementedException();
        }

        public ResultTaskDto GetPendingTaskForEmployee()
        {
            throw new NotImplementedException();
        }
    }
}
