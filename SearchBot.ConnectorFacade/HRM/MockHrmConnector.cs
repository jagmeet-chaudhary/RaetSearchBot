
using SearchBot.Connectors.HRM.Model;
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
        public List<Employee> SearchEmployees(Employee employee, string token)
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

        public Employee GetEmployeeManager(Employee employee, string token)
        {
            return new Employee()
            {
                FirstName = "Mark",
                LastName = "Sanders"

            };
        }

        public Employee GetManagerForEmployee(Employee employee, string token)
        {
            return new Employee()
            {
                FirstName = "Jason",
                LastName = "Smith"
            };
        }


        public AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token)
        {
            throw new NotImplementedException();
        }

        public ResultTaskDto GetPendingTaskForEmployee(string token)
        {
            throw new NotImplementedException();
        }

        public string GetUserImage(string externalId, string token)
        {
            throw new NotImplementedException();
        }

        public PersonDetails GetUserDetails(string externalId, string token)
        {
            throw new NotImplementedException();
        }

        public IList<SickLeave_Employee> GetSickLeaveEmployees(string from, string to, string token)
        {
            throw new NotImplementedException();
        }

        public string GetOrgUnitIdByPassingName(string OrgUnitName, string token)
        {
            throw new NotImplementedException();
        }
    }
}
