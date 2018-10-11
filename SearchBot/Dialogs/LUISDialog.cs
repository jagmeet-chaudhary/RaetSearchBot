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
using SearchBot.Extensions;
using System.Net;

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
        [LuisIntent("SignOut")]
        public async Task LogOut(IDialogContext context, LuisResult result)
        {
            await Signout(context);
            //await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
           
            await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }
        [LuisIntent("Signin")]
        public async Task SignIn(IDialogContext context, LuisResult result)
        {
            var accessToken = await context.GetUserTokenAsync("testclient1");

            if (string.IsNullOrEmpty(accessToken?.Token))
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);
            }
            else
            {
                await context.PostAsync("You're already logged in.");
            }
        }

        [LuisIntent("Greetings")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            var accessToken = await context.GetUserTokenAsync("testclient1").ConfigureAwait(false);

            if (string.IsNullOrEmpty(accessToken?.Token))
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);
            }
            else
            {
                context.Call(new GreetingDialog(), Callback);
            }
            
            
        }


        [LuisIntent("HRM.QueryManagerForEmployee")]
        public async Task QueryManagerForEmployee(IDialogContext context, LuisResult result)
        {
            var firstName = result.Entities?.FirstOrDefault(x => x.Type == "FirstName")?.Entity??String.Empty;
            var lastName = result.Entities?.FirstOrDefault(x => x.Type == "LastName")?.Entity??String.Empty;
            var employees = employeeService.GetEmployeesByName(firstName, lastName);

            if (employees.Count > 1)
            {
                var attachments = conversationInterface.GetEmployeeSearchList(employees,context);
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                message.Attachments = attachments;

                await context.PostAsync(message);
            }
            else if (employees.Count == 1)
            {
                var manager = employeeService.GetManger(employees.FirstOrDefault());
                var message = conversationInterface.GetManagerMessage(employees.FirstOrDefault(),manager,context);
                await context.PostAsync(message);
            }
            else
            {
                var message = conversationInterface.GetNoEmployeesMessage(context);
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

                var orgunit = employeeService.GetOrgUnitByName(orgUnitName);


                if (orgunit != null)
                {
                    //var manager = employeeService.GetManger(employees.FirstOrDefault());
                    var message = conversationInterface.GetOrgUnitMessage(orgunit,context);
                    await context.PostAsync(message);
                }
                else
                {
                    var message = conversationInterface.GetNoAuditChangeMessage(context);
                    await context.PostAsync(message);
                }

            }
            else
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);

            }

        }


        [LuisIntent("HRM.QueryPendingTaskForEmployee")]
        public async Task QueryPendingTaskForEmployee(IDialogContext context, LuisResult result)
        {

            var accessToken = await context.GetUserTokenAsync("testclient1");

            if (!string.IsNullOrEmpty(accessToken?.Token))
            {
               // var orgUnitName = result.Entities?.FirstOrDefault(x => x.Type == "OrgUnit")?.Entity;

                var orgunit = employeeService.GetPendingTaskForEmployee();
            }
            else
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);

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
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }
        private async Task SendOAuthCardAsync(IDialogContext context, Activity activity)
        {
            
            string ConnectionName = ConfigurationManager.AppSettings["ConnectionName"];
            var reply = await context.Activity.CreateOAuthReplyAsync(ConnectionName, "Please sign in to proceed.", "Sign In",true).ConfigureAwait(false);
            await context.PostAsync(reply);

            //context.Wait(WaitForToken);
        }

        //private async Task WaitForToken(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = context.Activity as Activity;
        //    if (activity.Name == "signin/verifyState")
        //    {
        //        // We do this so that we can pass handling to the right logic in the dialog. You can
        //        // set this to be whatever string you want.
        //        activity.Text = "loginComplete";
        //        await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
        //    }

        //}

        #endregion


    }
}