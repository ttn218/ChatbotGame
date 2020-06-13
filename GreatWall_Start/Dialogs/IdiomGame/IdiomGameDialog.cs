using LionKing.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LionKing.Dialogs;
using LionKing.Helpers;
using LionKing.Model;

namespace LionKing.Dialogs.IdiomGame
{
    [Serializable]
    public class IdiomGameDialog : IDialog<string>
    {

        public async Task StartAsync(IDialogContext context)
        {
            
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. 게임시작", Value = "게임시작", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 게임설명", Value = "게임설명", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 랭킹", Value = "랭킹", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 나가기", Value = "나가기", Type = ActionTypes.ImBack });

            message.Attachments.Add(new HeroCard { Title = "사자성어 게임봇 사자왕!", Buttons = actions }.ToAttachment());

            await context.PostAsync(message);

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if(strSelected == "게임시작")
            {
                context.Call(new GameStartDialog(), DialogResumeAfter);
            }
            else if(strSelected == "나가기")
            {
                context.Done("Exit");
            }
        }

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
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