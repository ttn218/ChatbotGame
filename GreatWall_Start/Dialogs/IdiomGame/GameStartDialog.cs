using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Timers;

using LionKing.Helpers;
using LionKing.Model;


namespace LionKing.Dialogs.IdiomGame
{
    [Serializable]
    public class GameStartDialog : IDialog<string>
    {
        string strMessage;
        private string strWelcomMMessage = "게임을 시작할까요? (Yes or No)";
        IQuiz quiz = new IdiomQuiz();
        public async Task StartAsync(IDialogContext context)
        {
            
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. Yes", Value = "Yes", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. No", Value = "No", Type = ActionTypes.ImBack });

            message.Attachments.Add(new HeroCard { Title = strWelcomMMessage, Buttons = actions }.ToAttachment());

            message.AttachmentLayout = "carousel";

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);

        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if(strSelected == "Yes")
            {
                quiz.CreateQuiz();
                context.Call(new GameLoopDialog(quiz), GameOver);
            }
            else if(strSelected == "No")
            {
                context.Done("Exit Game");
            }
            else
            {
                strMessage = "Yes or No 중에 골라주세요";
                await context.PostAsync(strMessage);
                context.Wait(MessageReceivedAsync);
                
            }
        }

        public async Task GameOver(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                strMessage = "Your Score : " + await result;

                await context.PostAsync(strMessage);

                context.Done("Game Over");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
        }
    }
}