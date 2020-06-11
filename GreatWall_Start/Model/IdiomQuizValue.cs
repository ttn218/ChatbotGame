using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    [Serializable]
    public class IdiomQuizValue : AQuizValue
    {
        public string[] example = new string[5];   // 보기들
        public int[] index = new int[2]; // 사자성어 맞춰야할 위치 기억용
        public IdiomQuizValue(string[] answers, string meaning,List<string> example, int[] indexs)
        {
            this.meaning = meaning;
            this.answers = answers;
            index = indexs;
            this.example = example.OrderBy(g => Guid.NewGuid()).ToArray();
        }

    }
}