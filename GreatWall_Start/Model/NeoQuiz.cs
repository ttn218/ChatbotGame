using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LionKing.Model
{
    [Serializable]
    public class NeoQuiz : IQuiz
    {
        List<AQuizValue> quizzes = new List<AQuizValue>();

        public List<AQuizValue> Quizzes
        {
            get
            {
                return quizzes;
            }
            set
            {
                quizzes = value.OrderBy(g => Guid.NewGuid()).ToList();
            }
        }
        public IMessageActivity Message { get; set; }
        public Level LEVEL { get; set; }

        public NeoQuiz(Level level)
        {
            LEVEL = level;
        }

        public void CreateQuiz()
        {
            List<Neo> randNeos = Neo.Words.OrderBy(g => Guid.NewGuid()).ToList();
            Random random = new Random();
            string[] answers;

            foreach (Neo neo in randNeos)
            {
                List<string> example;
                
                answers = new string[] { neo.Word };
                example = randNeos.Where(v => v != neo).OrderBy(g => Guid.NewGuid()).Take(2).Select(v => v.Word).ToList();
                example.Add(answers[0]);

                quizzes.Add(new NeoQuizValue(answers, neo.Meaning, example));
            }
            quizzes = quizzes.Distinct().ToList();
        }

        public bool QuizAnswer(string Answer, int index, out string message)
        {
            NeoQuizValue value = Quizzes.ElementAt(index) as NeoQuizValue;
            if (!value.answers.Contains(Answer))
            {
                message = "틀렸습니다.";
                return false;
            }

            message = value.answers[0] + Environment.NewLine + "정답입니다!";
            return true;
        }

        public IMessageActivity QuizMessage(IDialogContext context, int index, out string strMessage)
        {
            Message = context.MakeMessage();
            string strdata = "";

            var actions = new List<CardAction>();
            NeoQuizValue value = Quizzes.ElementAt(index) as NeoQuizValue;

            if(LEVEL == Level.NOMAL)
            {

                strdata = "다음은 무엇에 대한 설명인가? " + Environment.NewLine + "\"" + value.meaning + "\"";
                actions.Add(new CardAction() { Title = "1. " + value.example[0], Value = value.example[0], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "2. " + value.example[1], Value = value.example[1], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "3. " + value.example[2], Value = value.example[2], Type = ActionTypes.ImBack });
                Message.Attachments.Add(new HeroCard { Title = "다음 보기중 고르시오", Buttons = actions }.ToAttachment());
            }
            else
            {
                strdata = "다음은 무엇에 대한 설명인가? "+ Environment.NewLine + "\"" +value.meaning + "\"";
                Message.Attachments.Add(new HeroCard { Title = "주관식 입니다." }.ToAttachment());
            }

            strMessage = strdata;
            return Message;
        }
    }
}