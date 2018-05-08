using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Schema;
using NDCGrannyBotDemo.Context;
using NDCGrannyBotDemo.Models;
using NDCGrannyBotDemo.Responses;

namespace NDCGrannyBotDemo.Subjectable
{
    public class MainSubject:ISubject
    {
        public string SubjectName { get; set; }

        public ISubject ParentSubject { get; set; }

        // track in this topic if we have greeted the user already
        public bool Greeted { get; set; } = false;


        public MainSubject()
        {
            SubjectName = "Main";
        }

        public MainSubject(ISubject parentSubject) : this()
        {
            ParentSubject = parentSubject;

        }

        public async Task<bool> StartSubject(V4ReferenceContext context)
        {
            var conversation = ConversationState<ConversationData>.Get(context);

            conversation.MainMenuSubject = this;

            switch (context.Activity.Type)
            {
                case ActivityTypes.ConversationUpdate:
                
                // greet when added to conversation
                var activity = context.Activity.AsConversationUpdateActivity();
                if (activity.MembersAdded.Any(m => m.Id == activity.Recipient.Id))
                {
                    await MainSubjectResponses.ReplyWithGreeting(context);
                    this.Greeted = true;
                }
                
                break;

                case ActivityTypes.Message:
                    // greet on first message if we haven't already 
                    if (!Greeted)
                    {
                        await MainSubjectResponses.ReplyWithGreeting(context);
                        this.Greeted = true;
                    }
                    return await this.ContinueSubject(context);
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


                        case "GrannyBot":
                            conversation.CurrentSubject = new GrannyBotSubject(this);
                            await conversation.CurrentSubject.StartSubject(context);
                            break;

                        case "MainGames":

                            conversation.CurrentSubject = new GamesSubject(this);
                            await conversation.CurrentSubject.StartSubject(context);
                            break;

                        case "MainMenu":
                            await MainSubjectResponses.ReplyWithGreeting(context);
                            break;
                         
                        case "Help":
                            // show help
                            await MainSubjectResponses.ReplyWithHelp(context);
                            break;

                        default:
                            // show our confusion
                            await MainSubjectResponses.ReplyWithConfused(context);
                            break;
                    }

                    break;

                default:
                    break;
            }

            return true;
        }
    }
}
