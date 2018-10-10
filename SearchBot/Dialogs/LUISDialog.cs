using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using SearchBot.Service.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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


            await Signout(context);


            //await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Signin")]
        public async Task SignIn(IDialogContext context, LuisResult result)
        {
            var accessToken = await context.GetUserTokenAsync("testclient1");

            if (string.IsNullOrEmpty(accessToken?.Token))
            {

                context.Call(GetSignInDialog(), this.GetToken);

            }
        }

        [LuisIntent("Greetings")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {


            var accessToken = await context.GetUserTokenAsync("testclient1");

            if (string.IsNullOrEmpty(accessToken?.Token))
            {

                await context.Forward(GetSignInDialog(), this.ResumeAfterAuth, context.MakeMessage(), CancellationToken.None);

            }
            //await context.PostAsync("You're inside");

            context.Call(new GreetingDialog(), Callback);


        }

        private async Task ResumeAfterAuth(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;

            await context.PostAsync(context.MakeMessage());

        }

        [LuisIntent("HRM.QueryManagerForEmployee")]
        public async Task QueryManagerForEmployee(IDialogContext context, LuisResult result)
        {
            var firstName = result.Entities?.FirstOrDefault(x => x.Type == "FirstName")?.Entity;
            var lastName = result.Entities?.FirstOrDefault(x => x.Type == "LastName")?.Entity;

            var accessToken = await context.GetUserTokenAsync("testclient1");


            var employees = employeeService.GetEmployeesByName(firstName, lastName, accessToken.Token);

            if (employees.Count > 1)
            {
                var attachments = conversationInterface.GetEmployeeSearchList(employees);
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                message.Attachments = attachments;

                await context.PostAsync(message);
            }
            else if (employees.Count == 1)
            {
                var manager = employeeService.GetManger(employees.FirstOrDefault(), accessToken.Token);
                var message = conversationInterface.GetManagerMessage(manager);
                await context.PostAsync(message);
            }
            else
            {
                var message = conversationInterface.GetNoEmployeesMessage();
                await context.PostAsync(message);
            }



        }

        [LuisIntent("HRM.QueryOrgUnitChange")]
        public async Task QueryOrgUnitChange(IDialogContext context, LuisResult result)
        {

            var accessToken = await context.GetUserTokenAsync("testclient1");

            if (!string.IsNullOrEmpty(accessToken?.Token))
            {
                var orgUnitName = result.Entities?.FirstOrDefault(x => x.Type == "OrgUnit")?.Entity;

                var orgunit = employeeService.GetOrgUnitByName(orgUnitName, accessToken.Token);


                if (orgunit != null)
                {
                   
                    var message = conversationInterface.GetOrgUnitMessage(orgunit);
                    await context.PostAsync(message);
                }
                else
                {
                    var message = conversationInterface.GetNoOrgUnitMessage(orgUnitName);
                    await context.PostAsync(message);
                }

            }
            else
            {
                context.Call(GetSignInDialog(), this.GetToken);

            }

        }


        [LuisIntent("HRM.QueryPendingTaskForEmployee")]
        public async Task QueryPendingTaskForEmployee(IDialogContext context, LuisResult result)
        {

            var accessToken = await context.GetUserTokenAsync("testclient1");

            if (!string.IsNullOrEmpty(accessToken?.Token))
            {

                var tasks = employeeService.GetPendingTaskForEmployee(accessToken.Token);

                var attachments = conversationInterface.GetPendingTaskForEmployee(tasks);
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                message.Attachments = attachments;

                await context.PostAsync(message);


            }
            else
            {
                context.Call(GetSignInDialog(), this.GetToken);

            }

        }



        [LuisIntent("HRM.QuerySickLeaveForEmployee")]
        public async Task QuerySickLeaveForEmployee(IDialogContext context, LuisResult result)
        {

            var accessToken = await context.GetUserTokenAsync("testclient1");

            if (!string.IsNullOrEmpty(accessToken?.Token))
            {
                var orgUnitName = result.Entities?.FirstOrDefault(x => x.Type == "OrgUnit")?.Entity;

                var sickLeave_Employees = employeeService.GetSickLeaveEmployees(orgUnitName, "1800-01-01", "9999-12-31", accessToken.Token);

                await context.PostAsync(conversationInterface.GetleavesOfEmployees(sickLeave_Employees));


            }
            else
            {
                context.Call(GetSignInDialog(), this.GetToken);

            }

        }

        #region Private
        private static async Task Signout(IDialogContext context)
        {
            string ConnectionName = ConfigurationManager.AppSettings["ConnectionName"];

            await context.SignOutUserAsync(ConnectionName);
            await context.PostAsync($"You have been signed out.");
        }

        private async Task GetToken(IDialogContext context, IAwaitable<GetTokenResponse> tokenResponse)
        {

            var message = await tokenResponse;
            context.PrivateConversationData.SetValue("Token", message?.Token);


            await context.PostAsync("You're inside");

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
        #endregion


    }
}