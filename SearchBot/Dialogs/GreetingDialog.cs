using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Internals.Fibers;
using SearchBot.Service.Interfaces;
using System.Configuration;
using SearchBot.Extensions;
using SearchBot.UI;
using SearchBot.Model;

namespace SearchBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        IEmployeeService employeeService;
        IGreetingsConversationalInterface greetingConversationInterface;
        IQueryManagerConversationInterface queryManagerConversationInterface;
        
        public GreetingDialog(IEmployeeService employeeService, IGreetingsConversationalInterface greetingConversationInterface
            , IQueryManagerConversationInterface queryManagerConversationInterface)
        {
            this.employeeService = employeeService;
            this.greetingConversationInterface = greetingConversationInterface;
            this.queryManagerConversationInterface = queryManagerConversationInterface;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(greetingConversationInterface.GetRaetBotIntroText(context));
            await Respond(context);
        }

        private async Task Respond(IDialogContext context)
        {
            var userName = String.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync(greetingConversationInterface.GetAskNameText(context));
                context.UserData.SetValue<bool>("GetName", true);
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync(String.Format(greetingConversationInterface.GetHiGreetingText(context), userName));
                await context.PostAsync(String.Format(greetingConversationInterface.GetFetchingPendingTasksText(context)));
                var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]);
                var count = employeeService.GetPendingTaskCountForEmployee(accessToken.Token);
                if (count > 0)
                {
                    await context.PostAsync(String.Format(greetingConversationInterface.GetPendingTaskDetailsText(context), count));
                    context.Wait(CallbackForTasks);
                }
                else
                {
                    await context.PostAsync(String.Format(greetingConversationInterface.GetNoPendingTaskText(context)));
                    await context.PostAsync(greetingConversationInterface.GetUserInputQuestionText(context));
                    context.Done("");
                }
                

            }
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var userName = String.Empty;
            var getName = false;
            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
                await Respond(context);
            }
            context.Done(message);
        }

        public async Task CallbackForTasks(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var response = await argument;
            var yesResponse = greetingConversationInterface.GetYesText(context);
            if (response.Text.Trim().Equals(yesResponse, StringComparison.InvariantCultureIgnoreCase))
            {
                var accessToken = await context.GetUserTokenAsync(ConfigurationManager.AppSettings["ConnectionName"]);
                var tasks = employeeService.GetPendingTaskForEmployee(accessToken.Token);
                var message = context.MakeMessage();
                if (tasks.Items.Count > 0)
                {
                    await context.PostAsync(greetingConversationInterface.GetMostRecentPendingTaskText(context));
                    var attachments = queryManagerConversationInterface.GetPendingTaskForEmployee(tasks,context);
                    message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    message.Attachments = attachments;
                    await context.PostAsync(message);
                    await context.PostAsync(greetingConversationInterface.GetFullListPendingTaskText(context));
                }
                else
                {
                    message.Text = greetingConversationInterface.GetNoPendingTaskText(context);
                    await context.PostAsync(message);
                    
                }
            }
            else
            {
                var responseText = $"{greetingConversationInterface.GetOkText(context)}{greetingConversationInterface.GetUserInputQuestionText(context)}";
                await context.PostAsync(greetingConversationInterface.GetUserInputQuestionText(context));
            }
            context.Done(string.Empty);
        }
    }
}
