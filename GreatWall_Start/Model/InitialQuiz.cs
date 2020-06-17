using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    [Serializable]
    public class InitialQuiz : IQuiz
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
        public string topic;

        public InitialQuiz(string topic)
        {
            this.topic = topic;
        }

        public void CreateQuiz()
        {
            List<Initial> randNeos = Initial.Words.OrderBy(g => Guid.NewGuid()).ToList();
            Random random = new Random();
            string[] answers;

            foreach (Initial initial in randNeos)
            {
                if (this.topic != initial.Topic) continue;

                answers = new string[] { initial.Word };

                quizzes.Add(new InitialQuizValue(answers, initial.Meaning, initial.INITIAL));
            }

            quizzes = quizzes.Distinct().ToList();
        }

        public bool QuizAnswer(string Answer, int index, out string message)
        {
            InitialQuizValue value = Quizzes.ElementAt(index) as InitialQuizValue;
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
            InitialQuizValue value = Quizzes.ElementAt(index) as InitialQuizValue;
            strdata = "맞춰라! 초성퀴즈~ " + Environment.NewLine + "초성 : " + value.initial + " 주제 : " + this.topic;
            Message.Text = "Hint " + Environment.NewLine + value.meaning;
            Message.Locale = "UTF-8";
            strMessage = strdata;
            return Message;
        }
    }
}