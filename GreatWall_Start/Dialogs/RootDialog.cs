using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using LionKing.Dialogs.IdiomGame;
using System.Collections.Generic;
using LionKing.Model;
using LionKing.Dialogs.neoGame;
using LionKing.Dialogs.initialGame;

namespace LionKing
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        protected int count = 1;
        string strMessage;
        private string strWelcomMMessage = "�ȳ��ϼ��� ���Ӻ� �Դϴ�. ";
        
        public async Task StartAsync(IDialogContext context)
        {
            Idiom.Loadwords();
            Neo.Loadwords();
            Initial.Loadwords();
            await context.PostAsync(strWelcomMMessage);

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. ���ڼ��� ����", Value = "���ڼ������", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. ������ ����", Value = "���������", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. �ʼ�����", Value = "�ʼ�����", Type = ActionTypes.ImBack });
            message.Attachments.Add(new HeroCard { Title = "� ������ �Ͻǲ�����?", Buttons = actions }.ToAttachment());

            await context.PostAsync(message);

            context.Wait(SendWelcomeMessageAsync);
        }

        public async Task SendWelcomeMessageAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if(strSelected == "���ڼ������")
            {
                strMessage = "���ڼ��� ���� �����մϴ�.";
                await context.PostAsync(strMessage);

                context.Call(new IdiomGameDialog(), DialogResumeAfter);
            }
            else if(strSelected == "���������")
            {
                strMessage = "������ ���� �����մϴ�.";
                await context.PostAsync(strMessage);

                context.Call(new NeoGameDialog(), DialogResumeAfter);
            }
            else if(strSelected == "�ʼ�����")
            {
                strMessage = "�ʼ����� �����մϴ�.";
                await context.PostAsync(strMessage);

                context.Call(new initialGameDialog(), DialogResumeAfter);
            }
            else
            {
                strMessage = "����߿��� �ٽ� ����ּ���.";
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