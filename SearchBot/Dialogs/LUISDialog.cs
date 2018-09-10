using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using SearchBot.Model;
using SearchBot.Service.HRM;
using SearchBot.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SearchBot.Dialogs
{
    //[LuisModel("e73128cf-c14b-4b31-9cf8-196c771183e5", "253cdcc8eb464c25817edccb8554077e")]
    [LuisModel("2323a370-19f5-464d-99da-13753303fabc", "eeb1ff9fedda47d29280f6cf706558e2")]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        IEmployeeService employeeService;
        IQueryManagerConversationInterface conversationInterface;
        public LUISDialog(IEmployeeService employeeService, IQueryManagerConversationInterface conversationInterface)
        {
            this.employeeService = employeeService;
            this.conversationInterface = conversationInterface;
        }
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }
        [LuisIntent("Greetings")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
        }

        [LuisIntent("HRM.QueryManagerForEmployee")]
        public async Task QueryManagerForEmployee(IDialogContext context, LuisResult result)
        {
            

            var firstName  = result.Entities?.FirstOrDefault(x => x.Type == "FirstName")?.Entity;
            var lastName = result.Entities?.FirstOrDefault(x => x.Type == "LastName")?.Entity;
            var employees = employeeService.GetEmployeesByName(firstName,lastName);

            if(employees.Count > 1)
            {
                var attachments = conversationInterface.GetEmployeeSearchList(employees);
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                message.Attachments = attachments;

                await context.PostAsync(message);
            }
            else if(employees.Count == 1)
            {
                var manager = employeeService.GetManger(employees.FirstOrDefault());
                var message = conversationInterface.GetManagerMessage(manager);
                await context.PostAsync(message);
            }
            else
            {
                var message = conversationInterface.GetNoEmployeesMessage();
                await context.PostAsync(message);
            }
            

            
        }

        
        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        private GetTokenDialog GetSignInDialog()
        {
            string ConnectionName = ConfigurationManager.AppSettings["ConnectionName"];
            return new GetTokenDialog(
               ConnectionName,
               $"Please sign in to {ConnectionName} to proceed.",
               "Sign In",
               2,
               "Hmm. Something went wrong, let's try again.");
        }


    }
}