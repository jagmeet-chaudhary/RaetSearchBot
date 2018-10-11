using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchBot.Dialogs
{
    public sealed class CustomActivityMapper : IMessageActivityMapper
    {
        public IMessageActivity Map(IMessageActivity message)
        {
            if (message.ChannelId == ChannelIds.Msteams)
            {
                if (message.Attachments.Any() && message.Attachments[0].ContentType == "application/vnd.microsoft.card.signin")
                {
                    var card = message.Attachments[0].Content as SigninCard;
                    var buttons = card.Buttons as CardAction[];
                    if (buttons.Any())
                    {
                        buttons[0].Type = ActionTypes.OpenUrl;
                    }
                }
            }
            return message;
        }
    }
}