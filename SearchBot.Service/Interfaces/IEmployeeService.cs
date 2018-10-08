using SearchBot.Model;
using System.Collections.Generic;
using System.Linq;

namespace SearchBot.Service.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetEmployeesByName(string firstName,string lastName);
        Employee GetManger(Employee employee);

        AuditChangeContextDto GetOrgUnitByName(string orgUnitName);

        ResultTaskDto GetPendingTaskForEmployee();

    }
}