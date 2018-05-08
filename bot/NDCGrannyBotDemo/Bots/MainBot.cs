using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using NDCGrannyBotDemo.Context;
using NDCGrannyBotDemo.Models;
using NDCGrannyBotDemo.Subjectable;

namespace NDCGrannyBotDemo.Bots
{
    public class MainBot: IBot
    {
        public async Task OnTurn(ITurnContext turnContext)
        {

            var localContext = new V4ReferenceContext(turnContext);

            // Get the current ActiveTopic from my persisted conversation state
            var conversation = ConversationState<ConversationData>.Get(localContext);

            var handled = false;

            // if we don't have an active subject yet
            if (conversation.CurrentSubject == null)
            {
                // use the default topic
                conversation.CurrentSubject = new MainSubject();
                handled = await conversation.CurrentSubject.StartSubject(localContext);
            }
            else
            {
                // we do have an active subject, so call it 
                handled = await conversation.CurrentSubject.ContinueSubject(localContext);
            }

            //// if activeTopic's result is false and the activeTopic is NOT already the default topic
            //if (handled == false && !(conversation.ActiveTopic is DefaultTopic))
            //{
            //    // Use DefaultTopic as the active topic
            //    conversation.CurrentSubject = new DefaultTopic();
            //    await conversation.CurrentSubject.ResumeTopic(localContext);
            //}
        }
    }
}
