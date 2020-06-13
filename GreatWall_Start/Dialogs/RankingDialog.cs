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

namespace LionKing.Dialogs
{
    [Serializable]
    public class RankingDialog : IDialog<string>
    {

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(Ranker);
        }
        public async Task Rank_in(IDialogContext context, IAwaitable<string> result)
        {
            
        }
        public async Task Ranker(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                //strMessage = "Your Score : " + await result
                string score = await result;
                if (Int32.Parse(score) >= 1)
                {
                    var message = context.MakeMessage();
                    var actions = new List<CardAction>();
                    actions.Add(new CardAction() { Title = "랭킹 입력하기", Value = "input_user", Type = ActionTypes.PostBack });
                    message.Attachments.Add(new HeroCard { Title = "Your Score :" + await result, Buttons = actions }.ToAttachment());
                    await context.PostAsync(message);
                    //context.Wait(Ranker);
                }

            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();
            string init = activity.Text.Trim();
            var input = init;
            var message = context.MakeMessage();
            if ((strSelected) == "input_user")
            {
                await context.PostAsync("이니셜을 입력해주세요 ex) KMS");
            }
        }
















    }
}