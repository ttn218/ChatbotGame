using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    [Serializable]
    public class NeoQuizValue : AQuizValue
    {
        public string[] example = new string[3];

        public NeoQuizValue(string[] answers, string meaning, List<string> example)
        {
            this.meaning = meaning;
            this.answers = answers;
            this.example = example.OrderBy(g => Guid.NewGuid()).ToArray();
        }
    }
}