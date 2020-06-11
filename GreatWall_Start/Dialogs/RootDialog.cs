using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using LionKing.Dialogs.IdiomGame;
using System.Collections.Generic;
using LionKing.Model;

namespace LionKing
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        protected int count = 1;
        string strMessage;
        private string strWelcomMMessage = "안녕하세요 게임봇 입니다. ";
        
        public async Task StartAsync(IDialogContext context)
        {
            Idiom.Loadwords();
            await context.PostAsync(strWelcomMMessage);

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. 사자성어 게임", Value = "사자성어게임", Type = ActionTypes.ImBack });

            message.Attachments.Add(new HeroCard { Title = "어떤 게임을 하실껀가요?", Buttons = actions }.ToAttachment());

            await context.PostAsync(message);

            context.Wait(SendWelcomeMessageAsync);
        }

        public async Task SendWelcomeMessageAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if(strSelected == "사자성어게임")
            {
                strMessage = "사자성어 게임 연결합니다.";
                await context.PostAsync(strMessage);

                context.Call(new IdiomGameDialog(), DialogResumeAfter);
            }
            else
            {
                strMessage = "목록중에서 다시 골라주세요.";
                await context.PostAsync(strMessage);
                context.Wait(SendWelcomeMessageAsync);
            }
        }

        public async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                strMessage = await result;
                await this.MessageReceivedAsync(context, result);
            }
            catch(TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}