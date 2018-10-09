using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
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
        string GetOrgUnitMessage(AuditChangeContextDto employee);
    }
}
