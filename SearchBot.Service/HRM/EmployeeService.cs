using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchBot.Connectors;
using SearchBot.Connectors.HRM.Model;
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
            var employees = hrmConnector.SearchEmployees(new Employee { FirstName = firstName, LastName = lastName });
            return employees;
        }

        public Employee GetManger(Employee employee)
        {

            var manager = hrmConnector.GetManagerForEmployee(employee);
            return manager;
        }

        public AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token)
        {

            var orgunit = hrmConnector.GetOrgUnitByName(orgUnitName, token);

            return orgunit;
        }

        public ResultTaskDto GetPendingTaskForEmployee(string token)
        {

            var pendingTask = hrmConnector.GetPendingTaskForEmployee(token);

            foreach (var task in pendingTask.Items)
            {
                task.UserImage = hrmConnector.GetUserImage(task.SubjectReferenceId, token);
                task.ProcessSubjectFullName = hrmConnector.GetUserDetails(task.SubjectReferenceId, token)?.DisplayName;
            }

            return pendingTask;
        }

        public IList<SickLeave_Employee> GetSickLeaveEmployees(string orgunitname, string from, string to, string token)
        {            

            var sickLeave_Employees = hrmConnector.GetSickLeaveEmployees(from,to,token);

            return sickLeave_Employees;
        }

    }
}
