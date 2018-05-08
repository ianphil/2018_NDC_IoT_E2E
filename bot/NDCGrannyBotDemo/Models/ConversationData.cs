using NDCGrannyBotDemo.Subjectable;

namespace NDCGrannyBotDemo.Models
{
    public class ConversationData
    {
        public bool IsGreeted { get; set; }

        public ISubject CurrentSubject { get; set; }

        public ISubject MainMenuSubject { get; set; }

        public int SecretNumber { get; set; }
    }
}
