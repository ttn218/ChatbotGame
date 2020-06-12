using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    [Serializable]
    public class InitialQuizValue : AQuizValue
    {
        public string initial;
        public InitialQuizValue(string[] answers, string meaning, string initial)
        {
            this.meaning = meaning;
            this.answers = answers;
            this.initial = initial;
        }

    }
}