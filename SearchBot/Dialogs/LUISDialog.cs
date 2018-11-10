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
using SearchBot.Connectors.HRM.Model;
using System.IdentityModel.Tokens.Jwt;
using SearchBot.Service.RVM;
using Autofac;
using SearchBot.UI;
using System.Collections.Generic;

namespace SearchBot.Dialogs
{
    //[LuisModel("e73128cf-c14b-4b31-9cf8-196c771183e5", "253cdcc8eb464c25817edccb8554077e")]
    [LuisModel("2323a370-19f5-464d-99da-13753303fabc", "eeb1ff9fedda47d29280f6cf706558e2")]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        IEmployeeService employeeService;

        IRVM_UserService rvmService;

        IQueryManagerConversationInterface conversationInterface;
        IGreetingsConversationalInterface greetingsConversationInterface;
        public LUISDialog(IEmployeeService employeeService, IRVM_UserService rvmService, IQueryManagerConversationInterface conversationInterface, IGreetingsConversationalInterface greetingsConversationInterface)
        {
            this.employeeService = employeeService;
            this.conversationInterface = conversationInterface;
            this.rvmService = rvmService;
            this.greetingsConversationInterface = greetingsConversationInterface;
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

            await context.PostAsync("I'm sorry I don't know what you mean.".ToUserLocale(context));
            context.Wait(MessageReceived);
        }
        [LuisIntent("Signin")]
        public async Task SignIn(IDialogContext context, LuisResult result)
        {
            var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]);

            if (string.IsNullOrEmpty(accessToken?.Token))
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);
            }
            else
            {
                await context.PostAsync("You're already logged in.".ToUserLocale(context));
            }
        }

        [LuisIntent("Greetings")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]).ConfigureAwait(false);

            if (string.IsNullOrEmpty(accessToken?.Token))
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);
            }
            else
            {
                JsonWebToken jsonWebToken = GetInfoToken(accessToken.Token);
                context.UserData.SetValue("Name", jsonWebToken.Name);
                context.Call(new GreetingDialog(employeeService,greetingsConversationInterface, conversationInterface), Callback);
            }

        }

        private async Task ResumeAfterAuth(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;

            await context.PostAsync(context.MakeMessage());

        }


        [LuisIntent("HRM.QueryManagerForEmployee")]
        public async Task QueryManagerForEmployee(IDialogContext context, LuisResult result)
        {
            var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(accessToken?.Token))
            {
                var firstName = result.Entities?.FirstOrDefault(x => x.Type == "FirstName")?.Entity ?? String.Empty;
                var lastName = result.Entities?.FirstOrDefault(x => x.Type == "LastName")?.Entity ?? String.Empty;
                var employees = employeeService.GetEmployeesByName(firstName, lastName, accessToken.Token);

                if (employees.Count > 1)
                {
                    var attachments = conversationInterface.GetEmployeeSearchList(employees, context);
                    var message = context.MakeMessage();
                    message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    message.Attachments = attachments;

                    await context.PostAsync(message);
                }
                else if (employees.Count == 1)
                {
                    var manager = employeeService.GetManger(employees.FirstOrDefault(), accessToken.Token);
                    var message = conversationInterface.GetManagerMessage(employees.FirstOrDefault(), manager, context);
                    await context.PostAsync(message);
                }
                else
                {
                    var message = conversationInterface.GetNoEmployeesMessage(context);
                    await context.PostAsync(message);
                }
            }
            else
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);
            }



        }

        [LuisIntent("HRM.QueryOrgUnitChangeWithinDates")]
        public async Task QueryOrgUnitChangeWithinDates(IDialogContext context,LuisResult result)
        {
            var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]);
   
                
            if (!string.IsNullOrEmpty(accessToken?.Token))
            {
                var orgUnitName = result.Entities?.FirstOrDefault(x => x.Type == "OrgUnitName")?.Entity;
                var resolutions = (IList<object>)result.Entities?.FirstOrDefault(x => x.Type == "builtin.datetimeV2.daterange")?.Resolution["values"];
                if (resolutions == null)
                {
                    await context.PostAsync(conversationInterface.GetOrgUnitDatesValidationMessage(context));
                }
                else
                {
                    var entityValues = (Dictionary<string, object>)resolutions[0];
                    string start = DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0)).ToString();
                    string end = DateTime.UtcNow.ToString();

                    if (entityValues.ContainsKey("start"))
                    {
                        start = entityValues["start"]?.ToString();
                    }
                    if (entityValues.ContainsKey("end"))
                    {
                        end = entityValues["end"]?.ToString();
                    }

                    var auditChanges = employeeService.GetOrgUnitChangeByDates(orgUnitName, start, end, accessToken.Token);


                    if (auditChanges != null)
                    {

                        var message = conversationInterface.GetOrgUnitAuditChangeMessage(auditChanges, context);
                        await context.PostAsync(message);
                        await context.PostAsync(conversationInterface.GetOrgUnitAuditChangeDetailsMessage(auditChanges, context));
                    }
                    else
                    {
                        var message = conversationInterface.GetNoAuditChangeMessage(orgUnitName);
                        await context.PostAsync(message);
                    }
                }
                

            }
            else
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);

            }

        }
        
        
        [LuisIntent("HRM.QueryOrgUnitChange")]
        public async Task QueryOrgUnitChange(IDialogContext context, LuisResult result)
        {

            var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]);

            if (!string.IsNullOrEmpty(accessToken?.Token))
            {
                var orgUnitName = result.Entities?.FirstOrDefault(x => x.Type == "OrgUnit")?.Entity;

                var orgunit = employeeService.GetOrgUnitByName(orgUnitName, accessToken.Token);


                if (orgunit != null)
                {

                    var message = conversationInterface.GetOrgUnitAuditChangeMessage(orgunit, context);
                    await context.PostAsync(message);
                }
                else
                {
                    var message = conversationInterface.GetNoAuditChangeMessage(orgUnitName);
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

            var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]);

            if (!string.IsNullOrEmpty(accessToken?.Token))
            {

                var tasks = employeeService.GetPendingTaskForEmployee(accessToken.Token);
                var message = context.MakeMessage();
                if (tasks.Items.Count > 0)
                {
                    await context.PostAsync(greetingsConversationInterface.GetMostRecentPendingTaskText(context));
                    var attachments = conversationInterface.GetPendingTaskForEmployee(tasks,context);
                    message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    message.Attachments = attachments;
                    await context.PostAsync(message);
                    await context.PostAsync(greetingsConversationInterface.GetFullListPendingTaskText(context));
                }
                else
                {
                    message.Text = greetingsConversationInterface.GetNoPendingTaskText(context);
                    await context.PostAsync(message);
                }

                


            }
            else
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);
            }

        }



        [LuisIntent("HRM.QuerySickLeaveForEmployee")]
        public async Task QuerySickLeaveForEmployee(IDialogContext context, LuisResult result)
        {

            var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]);

            if (!string.IsNullOrEmpty(accessToken?.Token))
            {
                var orgUnitName = result.Entities?.FirstOrDefault(x => x.Type == "OrgUnit")?.Entity;

                var sickLeave_Employees = employeeService.GetSickLeaveEmployees(orgUnitName, "1800-01-01", "9999-12-31", accessToken.Token);

                await context.PostAsync(conversationInterface.GetleavesOfEmployees(sickLeave_Employees));


            }
            else
            {
                await SendOAuthCardAsync(context, (Activity)context.Activity).ConfigureAwait(false);

            }

        }


        [LuisIntent("HRM.QueryForResetPasswordForRVM")]
        public async Task QueryForResetPasswordForRVM(IDialogContext context, LuisResult result)
        {


            var firstName = result.Entities?.FirstOrDefault(x => x.Type == "FirstName")?.Entity ?? String.Empty;
            var lastName = result.Entities?.FirstOrDefault(x => x.Type == "LastName")?.Entity ?? String.Empty;
            var newPassword = result.Entities?.FirstOrDefault(x => x.Type == "ResetPassword")?.Entity;
            var userName = firstName + ' ' + lastName;

            var reset_RvmPassword = rvmService.Resetpassword(userName, newPassword);

            if(reset_RvmPassword)
            {
                await context.PostAsync(conversationInterface.GetPasswordResetMessage(userName));
            }
            else
            {
                await context.PostAsync(conversationInterface.GetPasswordResetErrorMessage(userName));
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
            var reply = await context.Activity.CreateOAuthReplyAsync(ConnectionName, "Please sign in to proceed.", "Sign In").ConfigureAwait(false);
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

        private JsonWebToken GetInfoToken(string token)
        {
            JwtSecurityToken jwtSecurityToken = GetSecurityToken(token);

            JsonWebTokenAssembler jsonWebTokenAssembler = new JsonWebTokenAssembler();
            var jsonWebToken = jsonWebTokenAssembler.ListOfClaimsToJsonWebToken(jwtSecurityToken.Claims);
            return jsonWebToken;
        }

        private JwtSecurityToken GetSecurityToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenS = tokenHandler.ReadToken(token) as JwtSecurityToken;
            return tokenS;
        }

        #endregion


    }
}