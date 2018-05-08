using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

namespace NDCGrannyBotDemo.Responses
{
    public class GamesSubjectResponses
    {
        public static async Task ReplyWithGreeting(ITurnContext context)
        {
            await context.SendActivity($"Welcome to the Games Section");
            await context.SendActivity($"Here is a list of games");

            Activity replyToConversation = MessageFactory.Carousel(new List<Attachment>()) as Activity;
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            List<CardImage> cardImages = new List<CardImage>
            {
                new CardImage(url: "http://icons.iconarchive.com/icons/mazenl77/I-like-buttons-3a/512/Cute-Ball-Games-icon.png")
            };

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction guessNumberButton = new CardAction()
            {
                Value = $"guessing game",
                Type = "imBack",
                Title = "Play a Guess a Number game"
            };

            cardButtons.Add(guessNumberButton);

            HeroCard plCard = new HeroCard()
            {
                Title = "Please Select a Game",
                Images = cardImages,
                Buttons = cardButtons,
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);

            var activities = new IActivity[] { MessageFactory.Text("Welcome to the Games Section."), replyToConversation };

            await context.SendActivities(activities);
        }

        public static async Task ReplyWithHelp(ITurnContext context)
        {
            await context.SendActivity($"Let's find you some help with games. ");
        }

        public static async Task ReplyWithConfused(ITurnContext context)
        {
            await context.SendActivity($"I am sorry, I didn't understand that.");
        }
    }
}
