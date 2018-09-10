using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchBot.UI
{
    public  class CardActionValues
    {
        public string ButtonLabel;
        public string ButtonValue;
        public string ActionType;
        public string responseImage;

    }
    public static class UIHelper
    {
        public static Attachment CreateHeroCard(string title,string subtitle,string responseMsgOnly,List<CardActionValues> cardActionValues)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = responseMsgOnly,
                
            };
            var cardImages = new List<CardImage>();
            var cardActions = new List<CardAction>();
            foreach (var cardActionValue in cardActionValues)
            {
                cardImages.Add(new CardImage(cardActionValue.responseImage));
                cardActions.Add(new CardAction(cardActionValue.ActionType,cardActionValue.ButtonLabel,value: cardActionValue.ButtonValue));
            }
            heroCard.Images = cardImages;
            heroCard.Buttons = cardActions;
            return heroCard.ToAttachment();
        }
        
    }
}