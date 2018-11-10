using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot
{
    public interface IGreetingsConversationalInterface
    {
        string GetRaetBotIntroText(IDialogContext context);
        string GetAskNameText(IDialogContext context);
        string GetHiGreetingText(IDialogContext context);
        string GetPendingTaskDetailsText(IDialogContext context);

        string GetYesText(IDialogContext context);
        string GetUserInputQuestionText(IDialogContext context);
        string GetFetchingPendingTasksText(IDialogContext context);
        string GetNoPendingTaskText(IDialogContext context);
        string GetMostRecentPendingTaskText(IDialogContext context);
        string GetFullListPendingTaskText(IDialogContext context);
        string GetOkText(IDialogContext context);
    }
}
