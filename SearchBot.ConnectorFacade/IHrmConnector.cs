
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
        List<Employee> SearchEmployees(Employee employee,string token);
        Employee GetManagerForEmployee(Employee employee, string token);

        AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token);
        ResultTaskDto GetPendingTaskForEmployee(string token);

        string GetUserImage(string externalId, string token);
        PersonDetails GetUserDetails(string externalId, string token);

        IList<SickLeave_Employee> GetSickLeaveEmployees(string orgUnitName,string from, string to, string token);
        AuditChangeContextDto GetOrgUnitChangeByDates(string orgUnitName, string start, string end, string token);



    }
}
