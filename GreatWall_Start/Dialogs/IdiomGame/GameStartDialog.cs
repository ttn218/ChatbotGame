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
using LionKing.Dialogs.Rank;


namespace LionKing.Dialogs.IdiomGame
{
    [Serializable]
    public class GameStartDialog : IDialog<string>
    {
        // 랭킹 인스턴스
        public string level;
        public int score;
        // 요기까지

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
                context.Call(new GameLoopDialog(Level.EASY), Rank);
            }
            else if(strSelected == "Nomal")
            {
                context.Call(new GameLoopDialog(Level.NOMAL), Rank);
            }
            else if(strSelected == "Hard")
            {
                context.Call(new GameLoopDialog(Level.HARD), Rank);
            }
            else
            {
                strMessage = "목록중에 골라 주세요";
                await context.PostAsync(strMessage);
                context.Wait(MessageReceivedAsync);
                return;
            }
            
        }

        public async Task Rank(IDialogContext context, IAwaitable<string> result)
        {
            string score = await result;
            context.Call(new insertRankDialog(score), GameOver);
        }

        public async Task GameOver(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                //strMessage = "Your Score : " + await result;
                bool chk = false;
                await context.PostAsync(strMessage);
                var message = context.MakeMessage();
                var actions = new List<CardAction>();
                message.Attachments.Add(new HeroCard { Title = "Your Score :" + await result, Buttons = actions }.ToAttachment());
                await context.PostAsync(message);

                context.Done("Game Over");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
        }
       
    }
}