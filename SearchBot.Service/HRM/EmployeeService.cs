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



        public List<Employee> GetEmployeesByName(string firstName, string lastName, string token)
        {
            var employees = hrmConnector.SearchEmployees(new Employee { FirstName = firstName, LastName = lastName }, token);
            return employees;
        }

        public Employee GetManger(Employee employee, string token)
        {

            var manager = hrmConnector.GetManagerForEmployee(employee, token);
            return manager;
        }

        public AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token)
        {

            //HrmApiConnector h = new HrmApiConnector(new RequestHelper(), new TokenProvider());

            var orgunit = hrmConnector.GetOrgUnitByName(orgUnitName, token);
            //var result = h.GetOrgUnitByName(orgUnitName);
            // var orgunit = hrmConnector.GetOrgUnitByName(orgUnitName);
            return orgunit;
        }

        public ResultTaskDto GetPendingTaskForEmployee(string token)
        {

            //HrmApiConnector h = new HrmApiConnector(new RequestHelper(), new TokenProvider());

            var pendingTask = hrmConnector.GetPendingTaskForEmployee(token);

            foreach (var task in pendingTask.Items)
            {
                task.UserImage = hrmConnector.GetUserImage(task.SubjectReferenceId, token);
                task.ProcessSubjectFullName = hrmConnector.GetUserDetails(task.SubjectReferenceId, token)?.DisplayName;
            }

            return pendingTask;
        }

        public SickLeave_Employees GetSickLeaveEmployees(string orgunitname, string from, string to, string token)
        {            

            var OrgUnit = hrmConnector.GetOrgUnitIdByPassingName(orgunitname, token);

            var pendingTask = hrmConnector.GetSickLeaveEmployees(OrgUnit, from,to,token);

            //foreach (var task in pendingTask)
            //{
            //    task.UserImage = hrmConnector.GetUserImage(task.SubjectReferenceId, token);
            //    task.ProcessSubjectFullName = hrmConnector.GetUserDetails(task.SubjectReferenceId, token)?.DisplayName;
            //}

            return pendingTask;
        }

    }
}
