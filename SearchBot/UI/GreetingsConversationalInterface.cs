using Microsoft.Bot.Builder.Dialogs;
using SearchBot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchBot
{
    public class GreetingsConversationalInterface : IGreetingsConversationalInterface
    {
        const string RaetBotIntro = "Hi I'm Raet Bot.";

        const string AskName = "What is your name?";

        const string HiGreeting = "Hi {0}.  How are you today?";

        const string PendingTaskDetails = "You have {0} pending tasks today.Do you want to see the details ?";

        const string YesText = "Yes";
        const string UserInputQuestion = "Sure.What can I do for you ?";

        const string FetchingPendingTasks = "Let me fetch your pending tasks for today...";

        public string GetAskNameText(IDialogContext context)
        {
            return AskName.ToUserLocale(context);
        }

        public string GetFetchingPendingTasksText(IDialogContext context)
        {
            return FetchingPendingTasks.ToUserLocale(context);
        }

        public string GetHiGreetingText(IDialogContext context)
        {
            return HiGreeting.ToUserLocale(context);
        }

        public string GetPendingTaskDetailsText(IDialogContext context)
        {
            return PendingTaskDetails.ToUserLocale(context);
        }

        public string GetRaetBotIntroText(IDialogContext context)
        {
            return RaetBotIntro.ToUserLocale(context);
        }

        public string GetUserInputQuestionText(IDialogContext context)
        {
            return UserInputQuestion.ToUserLocale(context);
        }

        public string GetYesText(IDialogContext context)
        {
            return YesText.ToUserLocale(context);
        }
    }
}