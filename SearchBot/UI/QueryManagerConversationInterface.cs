using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SearchBot.Extensions;
using SearchBot.Connectors.HRM.Model;
using SearchBot.Model;
using SearchBot.UI;
using System.Configuration;

namespace SearchBot
{
    public class QueryManagerConversationInterface : IQueryManagerConversationInterface
    {
        public List<Attachment> GetEmployeeSearchList(List<Employee> employees,IDialogContext context)
        {
            var attachments = new List<Attachment>();
            var listActionValues = new List<CardActionValues>();
            foreach(var employee in employees)
            {
                listActionValues.Add(new CardActionValues() {  ActionType = ActionTypes.ImBack, ButtonLabel = $"{employee.FirstName} {employee.LastName}", ButtonValue = $"Who is the manager for {employee.FirstName} {employee.LastName}?".ToUserLocale(context)});
            }
            attachments.Add(UIHelper.CreateHeroCard("I found multiple people with the name you provided.".ToUserLocale(context), "Who exactly are you looking for ?".ToUserLocale(context), "", listActionValues));
            return attachments;

        }

        public List<Attachment> GetPendingTaskForEmployee(ResultTaskDto tasks,IDialogContext context)
        {
            var baseUrl = ConfigurationManager.AppSettings["UiAppUrl"];
            
            var attachments = new List<Attachment>();
            var listActionValues = new List<CardActionValues>();
            foreach (var task in tasks.Items)
            {
                //todo : remove hardcoding of url
                listActionValues.Add(new CardActionValues() { ActionType = ActionTypes.OpenUrl, ButtonLabel = $"Click", ButtonValue = $"{baseUrl}home/{task.ProcessId}/{task.Id}" });
                //attachments.Add(UIHelper.CreateHeroCard("Multiple task has been assigned to you", "Please click on them", "", listActionValues));
                attachments.Add(UIHelper.CreateThumbnailCard("Multiple task has been assigned to you", listActionValues, task,context));
            }

            //attachments.Add(UIHelper.CreateThumbnailCard());
            return attachments;
        }

        public string GetleavesOfEmployees(IList<SickLeave_Employee> sickLeave_Employees)
        {
            var bulder = new StringBuilder();

            foreach (var sick_employee in sickLeave_Employees)
            {
               // bulder.AppendLine($"{sick_employee.Displayname} is on leave from {sick_employee.StartDate.ToShortDateString()} to {sick_employee.EndDate.ToShortDateString()}");

                bulder.AppendLine($"{sick_employee.Displayname} is on sick leave from {sick_employee.StartDate.ToShortDateString()} to 12/12/2018");
            }
            return bulder.ToString();
        }

        public string GetManagerMessage(Employee employee, Employee manager, IDialogContext context)
        {
            return $"Manager for {employee.FirstName} {employee.LastName} is {manager.FirstName} {manager.LastName}".ToUserLocale(context);
        }


        public string GetNoEmployeesMessage(IDialogContext context)
        {
            return "Sorry, I could not find any employee with this name.".ToUserLocale(context);
        }


        public string GetNoAuditChangeMessage(string name,IDialogContext context)
        {
            return $"Sorry, No changes has been done on {name}.".ToUserLocale(context);
        }
        

        public string GetOrgUnitAuditChangeMessage(AuditChangeContextDto dto, IDialogContext context)
        {
            return $"Last change on {dto.SubjectName} was done by {dto.InitiatorName}".ToUserLocale(context);
        }
        public string GetOrgUnitAuditChangeDetailsMessage(AuditChangeContextDto dto, IDialogContext context)
        {
            var baseUrl = ConfigurationManager.AppSettings["UiAppUrl"];
            var url = $"{baseUrl}settings/auditlog/ODM/{dto.SubjectId}/{dto.AuditEntityId}";

            return String.Format("You can also view the [details]({0}) of the changes.", url).ToUserLocale(context);
        }


        public string GetManagerMessage(Employee employee)
        {
            throw new NotImplementedException();
        }

        public string GetPasswordResetMessage(string userName,IDialogContext context)
        {
            return $"Password has been changed for {userName}.".ToUserLocale(context);
        }


        public string GetPasswordResetErrorMessage(string userName, IDialogContext context)
        {
            return $"Error Occurred while changing the Password for {userName}".ToUserLocale(context);
        }

        public string GetOrgUnitDatesValidationMessage(IDialogContext context)
        {
            return $"Something seems odd...Can you please elaborate a little more on the date range.".ToUserLocale(context);
        }

        public string GetHireLink(IDialogContext context)
        {
            var baseUrl = ConfigurationManager.AppSettings["UiAppUrl"];
            var taskUrl = $"{baseUrl}employees/hire////start";
            return $"Go to this [link]({taskUrl}) to initiate a hire process.".ToUserLocale(context);
        }

        public string GetNoSickLeaveMessage(IList<SickLeave_Employee> sickLeave_Employees, IDialogContext context)
        {
            return $"There are no sick leaves registered within this org unit for the provided date range.".ToUserLocale(context);
        }
    }
}