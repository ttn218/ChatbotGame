using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionKing.Model
{
    public interface IQuiz
    {
        List<AQuizValue> Quizzes { get; set; }
        IMessageActivity Message { get; set; }
        Level LEVEL { get; set; }
        void CreateQuiz();
        IMessageActivity QuizMessage();
        bool QuizAnswer(string Answer);

    }
}
