using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace LionKing.Model
{
    public class IdiomQuiz : IQuiz
    {

        
        public List<AQuizValue> Quizzes
        {
            get
            {
                Quizzes = Quizzes.OrderBy(g => Guid.NewGuid()).ToList();
                return Quizzes;
            }
            set => Quizzes = value;
        }
        public IMessageActivity Message { get; set; }
        public Level LEVEL { get; set; }

        public IdiomQuiz(Level level)
        {
            LEVEL = level;
        }

        public void CreateQuiz()
        {
            Idiom.Loadwords();
            List<Idiom> randidioms = Idiom.Words.OrderBy(g => Guid.NewGuid()).ToList();
            Random random = new Random();
            int[] index;
            string[] answers;

            foreach (Idiom idiom in randidioms)
            {
                List<string> example;
                //사자성어 4글자중 랜덤한 위치 빈칸 -> 나오는 보기들은 한글자 5개 -> 보기에서 고르거나, 직접 사자성어 전체를 써서 맞추거나.
                if (LEVEL == Level.EASY)
                {
                    index = new int[] { random.Next(4) };
                    char[] words = idiom.Word.ToArray();
                    
                    answers = new string[] { idiom.Word , words[index[0]].ToString()};
                    example = randidioms.Where(v => v != idiom).OrderBy(g => Guid.NewGuid()).Take(4).Select(v => v.Word.ToArray()[random.Next(4)].ToString()).ToList();
                    example.Add(words[index[0]].ToString());
                }
                //사자성어 4글자중 랜덤한 2개위치 빈칸 -> 나오는 보기들은 한글자 5개 -> 한글자 맞추고 한글자 다시 맞추거나, 한번에 2글자를 채팅으로 직접 쳐서 맞추거나(순서대로), 사자성어 전체를 써서 맞추기
                else if(LEVEL == Level.NOMAL)
                {
                    int i_1 = random.Next(4);
                    int i_2;
                    while (i_1 == (i_2 = random.Next(4))) ;
                    index = new int[] { i_1, i_2 };

                    char[] words = idiom.Word.ToArray();

                    answers = new string[] { idiom.Word, words[index[0]].ToString(), words[index[1]].ToString(), words[index[0]].ToString() + words[index[1]].ToString() };
                    example = randidioms.Where(v => v != idiom).OrderBy(g => Guid.NewGuid()).Take(4).Select(v => v.Word.ToArray()[random.Next(3)].ToString()).ToList();
                    example.Add(words[index[0]].ToString());
                    example.Add(words[index[1]].ToString());
                }
                // 사자성어 설명만 나옴 -> 보기의 존재? -> 사자성어 전체를 쳐야함
                else
                {
                    answers = new string[] { idiom.Word };

                    example = randidioms.Where(v => v != idiom).OrderBy(g => Guid.NewGuid()).Take(2).Select(v => v.Word).ToList();
                    example.Add(answers[0]);
                }

                Quizzes.Add(new IdiomQuizValue(answers, idiom.Meaning,example));
            }
        }

        public bool QuizAnswer(string Answer)
        {
            throw new NotImplementedException();
        }

        public IMessageActivity QuizMessage()
        {
            throw new NotImplementedException();
        }
    }
}