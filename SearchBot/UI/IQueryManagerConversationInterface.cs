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
        
        string GetManagerMessage(Employee employee,Employee manager,IDialogContext context);
        List<Attachment> GetPendingTaskForEmployee(ResultTaskDto tasks, IDialogContext context);
        string GetNoAuditChangeMessage(string orgUnitName, IDialogContext context);

        string GetPasswordResetMessage(string userName,IDialogContext context);
        string GetOrgUnitAuditChangeMessage(AuditChangeContextDto dto, IDialogContext context);

        string GetleavesOfEmployees(IList<SickLeave_Employee> sickLeave_Employees);

        string GetPasswordResetErrorMessage(string userName,IDialogContext context);
        string GetOrgUnitAuditChangeDetailsMessage(AuditChangeContextDto dto, IDialogContext context);
        string GetOrgUnitDatesValidationMessage(IDialogContext context);
        string GetHireLink(IDialogContext context);
        string GetNoSickLeaveMessage(IList<SickLeave_Employee> sickLeave_Employees, IDialogContext context);
    }
}
