using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using NDCGrannyBotDemo.Models;

namespace NDCGrannyBotDemo.Context
{
    public class V4ReferenceContext: TurnContextWrapper
    {
        public V4ReferenceContext(ITurnContext context) : base(context)
        {
        }

        public ConversationData ConversationState
        {
            get
            {
                return ConversationState<ConversationData>.Get(this);
            }
        }
    }
}
