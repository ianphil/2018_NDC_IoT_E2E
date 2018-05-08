using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace NDCGrannyBotDemo.Responses
{
    public class GuessingGamesSubjectResponses
    {
        public static async Task ReplyWithGreeting(ITurnContext context)
        {
            await context.SendActivity($"Welcome to the Guessing Game");
            await context.SendActivity($"Guess a number between 1 and 10");
        }

        public static async Task ReplyWithHelp(ITurnContext context)
        {
            await context.SendActivity($"Try guessing a number between 1 and 10");
        }

        public static async Task ReplyWithConfused(ITurnContext context)
        {
            await context.SendActivity($"I am sorry, I didn't understand that.");
        }
    }
}
