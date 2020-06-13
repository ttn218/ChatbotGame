using LionKing.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace LionKing.Dialogs.neoGame
{
    [Serializable]
    public class GameLoopDialog : IDialog<string>
    {
        
        int index = 1;
        int score;
        static int Time = 300;
        string strMessage;
        static IQuiz quiz;
        static bool exitcheck = false;
        public GameLoopDialog(Level level)
        {
            quiz = new NeoQuiz(level);
        }

        public async Task StartAsync(IDialogContext context)
        {
            quiz.CreateQuiz();
            score = 0;
            Time = 300;
            index = 1;
            exitcheck = false;

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (sender, e) => { OnTimedEventAsync(context, timer); };
            timer.Start();

            await showMessage(context);

            context.Wait(GameLoop);
        }
        private async void OnTimedEventAsync(IDialogContext context, Timer timer)
        {
            Time = Time - 1;

            if (Time <= 0)
            {
                var message = context.MakeMessage();
                var actions = new List<CardAction>();
                actions.Add(new CardAction() { Title = "1. 나가기", Value = "Exit", Type = ActionTypes.PostBack });
                message.Attachments.Add(new HeroCard { Title = "TimeOver", Buttons = actions }.ToAttachment());
                exitcheck = true;
                await context.PostAsync(message);
                timer.Stop();
            }
        }
        public async Task GameLoop(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (exitcheck)
            {
                context.Done(score.ToString());
                return;
            }

            if (!quiz.QuizAnswer(strSelected, index - 1, out strMessage))
            {
                index++;
                await context.PostAsync(strMessage);
                await showMessage(context);
                context.Wait(GameLoop);
                return;
            }
            index++;
            score++;

            await context.PostAsync(strMessage);
            await showMessage(context);
            context.Wait(GameLoop);
        }
        public async Task showMessage(IDialogContext context)
        {
            var message = quiz.QuizMessage(context, index - 1, out strMessage);

            strMessage = "남은시간 : " + Time + "초" + "\t" + "Score : " + score + Environment.NewLine + strMessage;

            await context.PostAsync(strMessage);
            await context.PostAsync(message);
        }
    }
}