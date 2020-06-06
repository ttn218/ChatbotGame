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
        static int Time = 30;
        string strMessage;
        
        public async Task StartAsync(IDialogContext context)
        {
            score = 0;
            Time = 30;
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (sender, e) => { OnTimedEventAsync(context, timer); };
            timer.Start();
            

            Quiz quiz = Quiz.Quizzes.ElementAt(index - 1);
            await Quizset(context, quiz);
            
        }

        private async void OnTimedEventAsync(IDialogContext context, Timer timer)
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

            Quiz quiz = Quiz.Quizzes.ElementAt(index - 1);
            if (!Answerflag)
            {
                await Quizset(context, quiz);
            }
            else
            {
                Activity activity = await result as Activity;
                string strSelected = activity.Text.Trim();

                if (strSelected == quiz.Answer)
                {
                    score++;
                    index++;
                    Answerflag = false;
                    strMessage = "정답입니다.";
                    await context.PostAsync(strMessage);

                    await GameLoop(context, result);
                }
                else if(strSelected == "Exit")
                {
                    context.Done(score.ToString());
                }
                else
                {
                    index++;
                    Answerflag = false;
                    strMessage = "틀렸습니다.";
                    await context.PostAsync(strMessage);
                    await GameLoop(context, result);
                }
            }
        }

        public async Task Quizset(IDialogContext context, Quiz quiz)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();
            actions.Add(new CardAction() { Title = "1. " + quiz.Example[0], Value = quiz.Example[0], Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. " + quiz.Example[1], Value = quiz.Example[1], Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. " + quiz.Example[2], Value = quiz.Example[2], Type = ActionTypes.ImBack });

            strMessage = "남은시간 : " + Time + "초 " + "점수 : " + score + System.Environment.NewLine + "\"" + quiz.Meaning + "\" 라는 뜻을 가진 사자성어는?";

            await context.PostAsync(strMessage);

            message.Attachments.Add(new HeroCard { Title = "다음 보기중 고르시오", Buttons = actions }.ToAttachment());

            message.AttachmentLayout = "carousel";

            await context.PostAsync(message);

            Answerflag = true;
            context.Wait(GameLoop);
        }
    }
}