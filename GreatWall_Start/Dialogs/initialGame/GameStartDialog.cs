using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LionKing.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LionKing.Dialogs.initialGame
{
    [Serializable]
    public class GameStartDialog : IDialog<object>
    {
        string strMessage;
        private string strWelcomMMessage = "주제를 정해주세요.";
        
        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();
            foreach (string topic in Initial.TOPICS)
            {
                actions.Add(new CardAction() { Title = "1." + topic, Value = topic , Type = ActionTypes.ImBack });
            }
            message.Attachments.Add(new HeroCard { Title = strWelcomMMessage, Buttons = actions }.ToAttachment());

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (!Initial.TOPICS.Contains(strSelected))
            {
                strMessage = "목록중에서 골라 주세요";
                await context.PostAsync(strMessage);
                context.Wait(MessageReceivedAsync);
                return;
            }
            context.Call(new GameLoopDialog(strSelected), GameOver);
        }

        private async Task GameOver(IDialogContext context, IAwaitable<object> result)
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