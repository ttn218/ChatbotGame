using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Timers;
using LionKing.Dialogs;
using LionKing.Helpers;
using LionKing.Model;


namespace LionKing.Dialogs.IdiomGame
{
    [Serializable]
    public class GameStartDialog : IDialog<string>
    {
        string strMessage;
        private string strWelcomMMessage = "게임난이도를 정해주세요. (Easy, Nomal, Hard)";
        public async Task StartAsync(IDialogContext context)
        {
            
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. Easy", Value = "Easy", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. Nomal", Value = "Nomal", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. Hard", Value = "Hard", Type = ActionTypes.ImBack });

            message.Attachments.Add(new HeroCard { Title = strWelcomMMessage, Buttons = actions}.ToAttachment());

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);

        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if(strSelected == "Easy")
            {
                context.Call(new GameLoopDialog(Level.EASY), GameOver);
            }
            else if(strSelected == "Nomal")
            {
                context.Call(new GameLoopDialog(Level.NOMAL), GameOver);
            }
            else if(strSelected == "Hard")
            {
                context.Call(new GameLoopDialog(Level.HARD), GameOver);
            }
            else
            {
                strMessage = "목록중에 골라 주세요";
                await context.PostAsync(strMessage);
                context.Wait(MessageReceivedAsync);
                return;
            }
            
        }

        public async Task GameOver(IDialogContext context, IAwaitable<string> result)
        { 
            try
            {
                strMessage = "Your Score : " + await result;
                
                string score = await result;
                if (Int32.Parse(score) >= 1)
                {
                    context.Call(new RankingDialog(), DialogResumeAfter);
                }
                //context.Done("Game Over");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
        }
        /**
        public async Task Ranker(IDialogContext context, IAwaitable<object> score)
        {
            Activity activity = await score as Activity;
            string strSelected = activity.Text.Trim();
            string init = activity.Text.Trim();
            var input = init;
            var message = context.MakeMessage();
            if((strSelected) == "input_user")
            {
                await context.PostAsync("이니셜을 입력해주세요 ex) KMS");
                

            }
                
              
        } **/
        public async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                strMessage = await result;
                await this.GameOver(context, result);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
        }
    }
}