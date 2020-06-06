using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    [Serializable]
    public class Quiz
    {
        private static List<Quiz> quizzes = new List<Quiz>();

        private string meaning;
        private string answer;

        private string[] example = new string[3];
        public static List<Quiz> Quizzes
        {
            get
            {
                return quizzes;
            }
        }

        public string Meaning
        {
            get
            {
                return meaning;
            }
        }

        public string Answer
        {
            get
            {
                return answer;
            }
        }

        public string[] Example
        {
            get
            {
                return example;
            }
        }

        private Quiz(Idiom idiom, List<string> example)
        {
            meaning = idiom.Meaning;
            answer = idiom.Word;

            this.example = example.OrderBy(g => Guid.NewGuid()).ToArray();

            
        }


        public static void CreateQuiz(List<Idiom> idioms)
        {
            List<Idiom> randidioms = idioms.OrderBy(g => Guid.NewGuid()).ToList();

            foreach(Idiom idiom in randidioms)
            {
                List<string> example = randidioms.Where(v => v != idiom).OrderBy(g => Guid.NewGuid()).Take(2).Select(v => v.Word).ToList();

                example.Add(idiom.Word);
                quizzes.Add(new Quiz(idiom, example));
            }
        }

    }
}