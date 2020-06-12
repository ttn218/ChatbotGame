using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LionKing.Dialogs.neoGame
{
    [Serializable]
    public class NeoGameDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. 게임시작", Value = "게임시작", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 게임설명", Value = "게임설명", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 랭킹", Value = "랭킹", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 나가기", Value = "나가기", Type = ActionTypes.ImBack });

            message.Attachments.Add(new HeroCard { Title = "신조어를 얼마나 알고있나요? 아재왕!", Buttons = actions }.ToAttachment());

            await context.PostAsync(message);

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (strSelected == "게임시작")
            {
                context.Call(new GameStartDialog(), DialogResumeAfter);
            }
            else if (strSelected == "나가기")
            {
                context.Done("Exit");
            }
        }

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                await this.StartAsync(context);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
        }
    }
}