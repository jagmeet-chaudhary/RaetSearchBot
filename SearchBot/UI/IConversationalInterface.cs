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
        List<Attachment> GetEmployeeSearchList(List<Employee> employees,IDialogContext context);
        string GetNoEmployeesMessage(IDialogContext context);
        string GetNoAuditChangeMessage(IDialogContext context);
        string GetManagerMessage(Employee employee,Employee manager,IDialogContext context);
        List<Attachment> GetPendingTaskForEmployee(ResultTaskDto tasks);
        string GetNoOrgUnitMessage(string orgUnitName);
        string GetOrgUnitMessage(AuditChangeContextDto dto, IDialogContext context);

        string GetleavesOfEmployees(IList<SickLeave_Employee> sickLeave_Employees);
    }
}
