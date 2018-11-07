using System;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SearchBot.Common.Exceptions;

namespace SearchBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private readonly IDialog<object> _dialog;
        private readonly bool _displayException=true;
        private readonly int _stackTraceLength=500;

        public RootDialog(LUISDialog dialog)
        {
            _dialog = dialog;
           
        }

        public async Task StartAsync(IDialogContext context)
        {
            try
            {
                context.Call(_dialog, ResumeAsync);
            }
            catch (Exception e)
            {
                if (_displayException)
                    await DisplayException(context, e).ConfigureAwait(false);
            }
        }

        private async Task ResumeAsync(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                context.Done(await result);
            }
            catch (Exception e)
            {
                if (_displayException)
                    await DisplayException(context, e).ConfigureAwait(false);
            }
        }

        private async Task DisplayException(IDialogContext context, Exception e)
        {
            if(e.InnerException is NotAuthorizedException)
            {
                await context.PostAsync("Sorry...Looks like you are not authorized to view this information.").ConfigureAwait(false);
            }
            else
            {
                var stackTrace = e.StackTrace;
                if (stackTrace.Length > _stackTraceLength)
                    stackTrace = stackTrace.Substring(0, _stackTraceLength) + "…";
                stackTrace = stackTrace.Replace(Environment.NewLine, "  \n");

                var message = e.Message.Replace(Environment.NewLine, "  \n");

                var exceptionStr = $"**{message}**  \n\n{stackTrace}";

                await context.PostAsync(exceptionStr).ConfigureAwait(false);
            }
            
        }
    }
}