using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

namespace NDCGrannyBotDemo.Responses
{
    public static class BotResponses
    {
        public static IActivity[] GetSingle(String activityID)
        {
 
            Activity replyToConversation = MessageFactory.Carousel(new List<Attachment>()) as Activity;
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            List<CardImage> cardImages = new List<CardImage>
            {
                new CardImage(url: "https://ndcdemogrannybotstorage.blob.core.windows.net/images/health-alert-378x237.jpg")
            };

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction sendAlertButton = new CardAction()
            {
                Value = $"send alert {activityID}",
                Type = "imBack",
                Title = "Send Alert"
            };

            CardAction canelAlertButton = new CardAction()
            {
                Value = $"cancel alert {activityID}",
                Type = "imBack",
                Title = "Cancel Alert" ,
                Image = "https://buildahead.com/wp-content/uploads/2017/02/Blush-Emoji-smaller.png",
            };

            cardButtons.Add(sendAlertButton);
            cardButtons.Add(canelAlertButton);

            HeroCard plCard = new HeroCard()
                {
                    Title = "Medical Alert Detected",
                    Images = cardImages,
                    Buttons = cardButtons,
                    Subtitle = activityID,
                    Text = "Sarah Smith has fallen"
                };
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);
                
            return new IActivity[] { replyToConversation, MessageFactory.Text("Review activity") };

        }

            public static IActivity[] GetMultiple()
        {
            Activity replyToConversation = MessageFactory.Carousel(new List<Attachment>()) as Activity;
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            for (int activityID = 0; activityID < 5; activityID++)
            {
                List<CardImage> cardImages = new List<CardImage>
                {
                    new CardImage(url: "https://ndcdemogrannybotstorage.blob.core.windows.net/images/health-alert-378x237.jpg")
                };

                List<CardAction> cardButtons = new List<CardAction>();

                CardAction sendAlertButton = new CardAction()
                {
                    Value = $"send alert {activityID}",
                    Type = "imBack",
                    Title = "Send Alert"
                };

                CardAction canelAlertButton = new CardAction()
                {
                    Value = $"cancel alert {activityID}",
                    Type = "imBack",
                    Title = "Cancel Alert"
                };

                cardButtons.Add(sendAlertButton);
                cardButtons.Add(canelAlertButton);

                HeroCard plCard = new HeroCard()
                {
                    Title = "Medical Alert Detected",
                    Images = cardImages,
                    Buttons = cardButtons,
                    Subtitle = activityID.ToString(),
                    Text = $"Sarah Smith {activityID} has fallen"
                };

                Attachment plAttachment = plCard.ToAttachment();
                replyToConversation.Attachments.Add(plAttachment);
            }

            return new IActivity[] { replyToConversation, MessageFactory.Text("Review activities") };
        } 
    }
}
