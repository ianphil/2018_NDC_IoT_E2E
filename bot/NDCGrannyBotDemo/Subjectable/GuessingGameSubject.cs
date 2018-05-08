using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using NDCGrannyBotDemo.Context;
using NDCGrannyBotDemo.Models;
using NDCGrannyBotDemo.Responses;

namespace NDCGrannyBotDemo.Subjectable
{
    public class GuessingGameSubject:ISubject
    {
        public string SubjectName { get; set; }

        public ISubject ParentSubject { get; set; }


        public GuessingGameSubject()
        {

        }

        public GuessingGameSubject(ISubject parentSubject)
        {
            ParentSubject = parentSubject;
        }

        public async Task<bool> StartSubject(V4ReferenceContext context)
        {
            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:

                    await GuessingGamesSubjectResponses.ReplyWithGreeting(context);
                    break;

                default:
                    break;
            }
            return true;
        }

        public async Task<bool> ContinueSubject(V4ReferenceContext context)
        {

            var conversation = ConversationState<ConversationData>.Get(context);

            if (conversation.SecretNumber == 0)
            {
                conversation.SecretNumber = new Random().Next(1, 10);
            }

            if (context.Activity.Type is ActivityTypes.Message)
            {
                if (int.TryParse(context.Activity.Text, out var guess))
                {
                    if (guess > conversation.SecretNumber)
                    {
                        await context.SendActivity($"Guess Lower");
                    }

                    if (guess < conversation.SecretNumber)
                    {
                        await context.SendActivity($"Guess Higher");
                    }

                    if (int.Parse(context.Activity.Text) == conversation.SecretNumber)
                    {
                        await context.SendActivity($"You Guessed it");
                    }

                    await context.SendActivity($"The number is {conversation.SecretNumber}");
                }
                else
                {
                    var luisResult = context.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);

                    (string key, double score) topItem = luisResult.GetTopScoringIntent();

                    switch (context.Activity.Type)
                    {
                        case ActivityTypes.Message:
                            switch (topItem.key)
                            {
                                
                                case "Help":
                                    // show help
                                    await GuessingGamesSubjectResponses.ReplyWithHelp(context);
                                    break;

                                case "MainMenu":
                                    // show Main Menu
                                    conversation.CurrentSubject = conversation.MainMenuSubject;
                                    await conversation.MainMenuSubject.ContinueSubject(context);
                                    break;

                                case "Quit":
                                    // show Main Menu
                                    conversation.CurrentSubject = ParentSubject;
                                    await ParentSubject.StartSubject(context);
                                    break;

                                default:
                                    // show our confusion
                                    await GuessingGamesSubjectResponses.ReplyWithConfused(context);
                                    break;
                            }
                            break;

                        default:
                            break;
                    }

                }
            }

            return true;
        }
    }
}
