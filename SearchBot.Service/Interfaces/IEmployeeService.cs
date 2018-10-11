using SearchBot.Connectors.HRM.Model;
using SearchBot.Model;
using System.Collections.Generic;
using System.Linq;

namespace SearchBot.Service.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetEmployeesByName(string firstName,string lastName);
        Employee GetManger(Employee employee);

        AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token);

        ResultTaskDto GetPendingTaskForEmployee(string token);

        IList<SickLeave_Employee> GetSickLeaveEmployees(string orgunitname, string from, string to, string token);
    }
}