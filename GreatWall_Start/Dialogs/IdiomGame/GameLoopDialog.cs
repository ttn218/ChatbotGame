using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using System.Timers;

using LionKing.Model;
using Microsoft.Bot.Connector;
using System.Threading;
using Timer = System.Timers.Timer;

namespace LionKing.Dialogs.IdiomGame
{
    [Serializable]
    public class GameLoopDialog : IDialog<string>
    {
        int index = 1;
        int score;
        bool Answerflag = false;
        int Time = 30;
        string strMessage;
        static Random random;
        IQuiz quiz;

        public async Task StartAsync(IDialogContext context)
        {
            score = 0;
            Time = 30;
            index = 1;
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (sender, e) => { OnTimedEventAsync(context, timer, Time); };
            timer.Start();
            
        }

        public GameLoopDialog(IQuiz quiz)
        {
            this.quiz = quiz;
        }

        private async void OnTimedEventAsync(IDialogContext context, Timer timer, int Time)
        {
            Time = Time - 1;
            
            if(Time <= 0)
            {
                var message = context.MakeMessage();
                var actions = new List<CardAction>();
                actions.Add(new CardAction() { Title = "1. 나가기", Value = "Exit", Type = ActionTypes.PostBack });
                message.Attachments.Add(new HeroCard { Title = "TimeOver", Buttons = actions }.ToAttachment());
                await context.PostAsync(message);
                timer.Stop();
                
            }

        }

        public async Task GameLoop(IDialogContext context, IAwaitable<object> result)
        {

        }
    }
}