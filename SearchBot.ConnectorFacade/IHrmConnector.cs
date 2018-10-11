
using SearchBot.Connectors.HRM.Model;
using SearchBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors
{
    public interface IHrmConnector
    {
        List<Employee> SearchEmployees(Employee employee);
        Employee GetManagerForEmployee(Employee employee);

        AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token);
        ResultTaskDto GetPendingTaskForEmployee(string token);

        string GetUserImage(string externalId, string token);
        PersonDetails GetUserDetails(string externalId, string token);

        IList<SickLeave_Employee> GetSickLeaveEmployees(string from, string to, string token);
       

    }
}
