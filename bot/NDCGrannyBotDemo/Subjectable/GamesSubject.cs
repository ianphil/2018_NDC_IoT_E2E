using System.Threading.Tasks;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Schema;
using NDCGrannyBotDemo.Context;
using NDCGrannyBotDemo.Models;
using NDCGrannyBotDemo.Responses;

namespace NDCGrannyBotDemo.Subjectable
{
    public class GamesSubject : ISubject
    {
        public string SubjectName { get; set; }
        public ISubject ParentSubject { get; set; }


        public GamesSubject()
        {

        }

        public GamesSubject(ISubject parentSubject)
        {
            ParentSubject = parentSubject;
        }

        public async Task<bool> StartSubject(V4ReferenceContext context)
        {
            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:

                    await GamesSubjectResponses.ReplyWithGreeting(context);
                    break;

                default:
                    break;
            }
            return true;
        }

        public async Task<bool> ContinueSubject(V4ReferenceContext context)
        {
            var conversation = ConversationState<ConversationData>.Get(context);

            var luisResult = context.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);

            (string key, double score) topItem = luisResult.GetTopScoringIntent();

            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:
                    switch (topItem.key)
                    {
                        case "GuessingGame":
                           conversation.CurrentSubject = new GuessingGameSubject(this);
                           await conversation.CurrentSubject.StartSubject(context);
                           break;

                        case "Help":
                            // show help
                            await GamesSubjectResponses.ReplyWithHelp(context);
                            break;

                        case "MainMenu":
                            // show Main Menu
                            conversation.CurrentSubject = conversation.MainMenuSubject;
                            await conversation.MainMenuSubject.ContinueSubject(context);
                            break;

                        case "Quit":
                            // show Main Menu
                            conversation.CurrentSubject = ParentSubject;
                            await ParentSubject.ContinueSubject(context);
                            break;

                        default:
                            // show our confusion
                            await GamesSubjectResponses.ReplyWithConfused(context);
                            break;
                    }

                    break;
                default:
                    await context.SendActivity($"Not setup yet");
                    break;
            }

            return true;
        }
    }
}
