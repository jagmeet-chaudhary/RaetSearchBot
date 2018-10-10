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
                listActionValues.Add(new CardActionValues() { ActionType = ActionTypes.PostBack, ButtonLabel = $"{employee.FirstName} {employee.LastName}", ButtonValue = $"Who is the manager for {employee.FirstName} {employee.LastName}?".ToUserLocale(context) });
            }
            attachments.Add(UIHelper.CreateHeroCard("We found multiple people for your search.".ToUserLocale(context), "Who exactly are you looking for ?".ToUserLocale(context), "", listActionValues));
            return attachments;

        }

        public List<Attachment> GetPendingTaskForEmployee(ResultTaskDto tasks)
        {
            var attachments = new List<Attachment>();
            var listActionValues = new List<CardActionValues>();
            foreach (var task in tasks.Items)
            {
                listActionValues.Add(new CardActionValues() { ActionType = ActionTypes.OpenUrl, ButtonLabel = $"Click", ButtonValue = $"https://yfo-stark-test.azurewebsites.net/home/{task.ProcessId}/{task.Id}" });
                //attachments.Add(UIHelper.CreateHeroCard("Multiple task has been assigned to you", "Please click on them", "", listActionValues));
                attachments.Add(UIHelper.CreateThumbnailCard("Multiple task has been assigned to you", listActionValues, task));
            }

            //attachments.Add(UIHelper.CreateThumbnailCard());
            return attachments;
        }

        public string GetleavesOfEmployees(IList<SickLeave_Employee> sickLeave_Employees)
        {
            var bulder = new StringBuilder();

            foreach (var sick_employee in sickLeave_Employees)
            {
                bulder.AppendLine($"{sick_employee.Displayname} is on leave from {sick_employee.StartDate.ToShortDateString()} to {sick_employee.EndDate.ToShortDateString()}");
            }
            return bulder.ToString();
        }

        public string GetManagerMessage(Employee employee, Employee manager, IDialogContext context)
        {
            return $"Manager for {employee.FirstName} {employee.LastName} is {manager.FirstName} {manager.LastName}".ToUserLocale(context);
        }

        public string GetNoAuditChangeMessage(IDialogContext context)
        {
            return "We did not find any audit changes for this organizational unit.".ToUserLocale(context);
        }

        public string GetNoEmployeesMessage(IDialogContext context)
        {
            return "Sorry, I could not find any employee with this name.".ToUserLocale(context);
        }


        public string GetNoOrgUnitMessage(string name)
        {
            return $"Sorry, No changes has been done on {name}.";
        }
        

        public string GetOrgUnitMessage(AuditChangeContextDto dto, IDialogContext context)
        {
            return $"Last change on {dto.SubjectName} was done by {dto.InitiatorName}".ToUserLocale(context);
        }

        public string GetManagerMessage(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}