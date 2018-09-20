using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;
using SearchBot.Model;
using SearchBot.UI;

namespace SearchBot
{
    public class QueryManagerConversationInterface : IQueryManagerConversationInterface
    {
        public List<Attachment> GetEmployeeSearchList(List<Employee> employees)
        {
            var attachments = new List<Attachment>();
            var listActionValues = new List<CardActionValues>();
            foreach(var employee in employees)
            {
                listActionValues.Add(new CardActionValues() { ActionType = ActionTypes.PostBack, ButtonLabel = $"{employee.FirstName} {employee.LastName}", ButtonValue = $"Who is the manager for {employee.FirstName} {employee.LastName}?" });
            }
            attachments.Add(UIHelper.CreateHeroCard("We seem to have multiple people with name 'John'.", "Who exactly are you looking for ?", "", listActionValues));
            return attachments;

        }

        public string GetManagerMessage(Employee employee)
        {
            return $"Manager is {employee.FirstName} {employee.LastName}";
        }

        public string GetNoEmployeesMessage()
        {
            return "Sorry, I could not find any employee with this name.";
        }
    }
}