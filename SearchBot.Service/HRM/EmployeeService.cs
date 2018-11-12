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



        public List<Employee> GetEmployeesByName(string firstName, string lastName,string token)
        {
            var employees = hrmConnector.SearchEmployees(new Employee { FirstName = firstName, LastName = lastName },token);
            return employees;
        }

        public Employee GetManger(Employee employee,string token)
        {

            var manager = hrmConnector.GetManagerForEmployee(employee,token);
            return manager;
        }

        public AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token)
        {

            var orgunit = hrmConnector.GetOrgUnitByName(orgUnitName, token);

            return orgunit;
        }
        public AuditChangeContextDto GetOrgUnitChangeByDates(string orgUnitName,string start,string end, string token)
        {
            var startDate = DateTime.Parse(start).ToString("yyyy-MM-ddTHH:mm:ssZ");
            var endDate = DateTime.Parse(end).ToString("yyyy-MM-ddTHH:mm:ssZ");
            var auditChanges = hrmConnector.GetOrgUnitChangeByDates(orgUnitName, startDate, endDate, token);

            return auditChanges;    
        }
        public int GetPendingTaskCountForEmployee(string token)
        {
            var pendingTask = hrmConnector.GetPendingTaskForEmployee(token);
            return Convert.ToInt32(pendingTask.Count);
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
            var startDate = DateTime.Parse(from).ToString("yyyy-MM-ddTHH:mm:ssZ");
            var endDate = DateTime.Parse(to).ToString("yyyy-MM-ddTHH:mm:ssZ");
            var sickLeave_Employees = hrmConnector.GetSickLeaveEmployees(orgunitname,startDate,endDate,token);

            return sickLeave_Employees;
        }

      

    }
}
