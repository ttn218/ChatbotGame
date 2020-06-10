using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    public class IdiomQuizValue : AQuizValue
    {
        private string[] example = new string[5];

        public IdiomQuizValue(string[] answers, string meaning,List<string> example)
        {
            this.meaning = meaning;
            this.answers = answers;

            this.example = example.OrderBy(g => Guid.NewGuid()).ToArray();
        }
    }
}