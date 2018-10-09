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
            foreach (var employee in employees)
            {
                listActionValues.Add(new CardActionValues() { ActionType = ActionTypes.PostBack, ButtonLabel = $"{employee.LastName}, {employee.FirstName}", ButtonValue = $"Who is the manager for {employee.FirstName} {employee.LastName}?" });
            }
            attachments.Add(UIHelper.CreateHeroCard("We seem to have multiple people with name 'John'.", "Who exactly are you looking for ?", "", listActionValues));
            return attachments;

        }

        public List<Attachment> GetPendingTaskForEmployee(ResultTaskDto tasks)
        {
            var attachments = new List<Attachment>();
            var listActionValues = new List<CardActionValues>();
            foreach (var task in tasks.Items)
            {
                listActionValues.Add(new CardActionValues() {ActionType = ActionTypes.OpenUrl, ButtonLabel = $"Click", ButtonValue = $"https://yfo-stark-test.azurewebsites.net/home/{task.ProcessId}/{task.Id}" });
                //attachments.Add(UIHelper.CreateHeroCard("Multiple task has been assigned to you", "Please click on them", "", listActionValues));
                attachments.Add(UIHelper.CreateThumbnailCard("Multiple task has been assigned to you", listActionValues, task));
            }

            //attachments.Add(UIHelper.CreateThumbnailCard());
            return attachments;
        }

        public string GetManagerMessage(Employee employee)
        {
            return $"Manager is {employee.LastName}, {employee.FirstName}";
        }

        public string GetNoEmployeesMessage()
        {
            return "Sorry, I could not find any employee with this name.";
        }

        public string GetOrgUnitMessage(AuditChangeContextDto dto)
        {
            return $"Change made on {dto.SubjectName} by {dto.InitiatorName}";
        }
    }
}