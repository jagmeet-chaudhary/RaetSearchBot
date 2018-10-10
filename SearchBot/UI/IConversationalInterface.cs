using Microsoft.Bot.Connector;
using SearchBot.Connectors.HRM.Model;
using SearchBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot
{
    public interface IQueryManagerConversationInterface
    {
        List<Attachment> GetEmployeeSearchList(List<Employee> employees);
        string GetNoEmployeesMessage();

        string GetNoOrgUnitMessage(string orgUnitName);
        string GetManagerMessage(Employee employee);

        string GetOrgUnitMessage(AuditChangeContextDto employee);

        List<Attachment> GetPendingTaskForEmployee(ResultTaskDto tasks);
        string GetleavesOfEmployees(IList<SickLeave_Employee> sickLeave_Employees);
    }
}
