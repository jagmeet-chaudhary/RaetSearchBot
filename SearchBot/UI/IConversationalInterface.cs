using Microsoft.Bot.Builder.Dialogs;
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

        string GetNoOrgUnitMessage(string orgUnitName);
        string GetManagerMessage(Employee employee);

        List<Attachment> GetEmployeeSearchList(List<Employee> employees,IDialogContext context);
        string GetNoEmployeesMessage(IDialogContext context);
        string GetManagerMessage(Employee employee,Employee manager,IDialogContext context);
        string GetOrgUnitMessage(AuditChangeContextDto employee);

        List<Attachment> GetPendingTaskForEmployee(ResultTaskDto tasks);
        string GetleavesOfEmployees(IList<SickLeave_Employee> sickLeave_Employees);
    }
}
