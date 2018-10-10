using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SearchBot.Extensions;
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

        public string GetManagerMessage(Employee employee,Employee manager, IDialogContext context)
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

        public string GetOrgUnitMessage(AuditChangeContextDto dto,IDialogContext context)
        {
            return $"Last change on {dto.SubjectName} was done by {dto.InitiatorName}".ToUserLocale(context);
        }
    }
}