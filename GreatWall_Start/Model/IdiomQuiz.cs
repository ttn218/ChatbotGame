using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LionKing.Model
{
    [Serializable]
    public class IdiomQuiz : IQuiz
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
        public string Answertemp = null;
        public IdiomQuiz(Level level)
        {
            LEVEL = level;
        }

        public void CreateQuiz()
        {
            
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

                    answers = new string[] { idiom.Word, words[index[0]].ToString(), words[index[1]].ToString(), (words[index[0]].ToString() + words[index[1]].ToString()).Trim() };
                    example = randidioms.Where(v => v != idiom).OrderBy(g => Guid.NewGuid()).Take(3).Select(v => v.Word.ToArray()[random.Next(3)].ToString()).ToList();
                    example.Add(words[index[0]].ToString());
                    example.Add(words[index[1]].ToString());
                }
                // 사자성어 설명만 나옴 -> 보기의 존재? -> 사자성어 전체를 쳐야함
                else
                {
                    answers = new string[] { idiom.Word };
                    index = null;
                    example = randidioms.Where(v => v != idiom).OrderBy(g => Guid.NewGuid()).Take(2).Select(v => v.Word).ToList();
                    example.Add(answers[0]);
                }

                Quizzes.Add(new IdiomQuizValue(answers, idiom.Meaning,example, index));
            }
            
        }

        public bool QuizAnswer(string Answer, int index, out string message)
        {
            IdiomQuizValue value = Quizzes.ElementAt(index) as IdiomQuizValue;
            //value.answers -> 0: 사자성어 1: 첫번쨰 글자 2:두번쨰 글자 3:노말난이도일때 첫번쨰 글자+ 두번쨰 글자
            if(!value.answers.Contains(Answer))
            {
                message = "틀렸습니다.";
                Answertemp = null;
                return false;
            }
            if(LEVEL == Level.NOMAL)
            {
                if(Answer.Length < 2)
                {   
                    if(Answertemp !=  Answer && Answertemp != null)
                    {
                        message = value.answers[0] + Environment.NewLine + "정답입니다!";
                        Answertemp = null;
                        return true;
                    }

                    if(value.answers[1].Equals(Answer))
                    {
                        char[] idiomchar = value.answers[0].ToArray();
                        idiomchar[value.index[1]] = 'O';
                        message = string.Concat(idiomchar) + Environment.NewLine + "정답입니다! 나머지 한글자까지 맞춰보세요";
                        Answertemp = Answer;
                        return true;
                    }
                    else if(value.answers[2].Equals(Answer))
                    {
                        char[] idiomchar = value.answers[0].ToArray();
                        idiomchar[value.index[0]] = 'O';
                        message = string.Concat(idiomchar) + Environment.NewLine + "정답입니다! 나머지 한글자까지 맞춰보세요";
                        Answertemp = Answer;
                        return true;
                    }
                }
            }
            message = value.answers[0] + Environment.NewLine + "정답입니다!";
            return true;
        }

        public IMessageActivity QuizMessage(IDialogContext context, int index, out string strMessage)
        {
            Message = context.MakeMessage();
            string strdata = "";
           
            var actions = new List<CardAction>();
            IdiomQuizValue value = Quizzes.ElementAt(index) as IdiomQuizValue;
            if (LEVEL == Level.EASY)
            {
                char[] idiomchar = value.answers[0].ToArray();
                idiomchar[value.index[0]] = 'O';

                strdata = "\"" + value.meaning + "\"라는 뜻을 가진 사자성어의 빈칸을 채우시오." + Environment.NewLine + "사자성어 : "+ string.Concat(idiomchar);

                actions.Add(new CardAction() { Title = "1. " + value.example[0], Value = value.example[0], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "2. " + value.example[1], Value = value.example[1], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "3. " + value.example[2], Value = value.example[2], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "4. " + value.example[3], Value = value.example[3], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "5. " + value.example[4], Value = value.example[4], Type = ActionTypes.ImBack });
                Message.Attachments.Add(new HeroCard { Title = "다음 보기중 고르시오", Buttons = actions }.ToAttachment());

            }
            else if(LEVEL == Level.NOMAL)
            {
                char[] idiomchar = value.answers[0].ToArray();
                idiomchar[value.index[0]] = 'O';
                idiomchar[value.index[1]] = 'O';
                strdata = "\"" + value.meaning + "\"라는 뜻을 가진 사자성어의 빈칸을 채우시오." + Environment.NewLine + "사자성어 : " + string.Concat(idiomchar);

                actions.Add(new CardAction() { Title = "1. " + value.example[0], Value = value.example[0], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "2. " + value.example[1], Value = value.example[1], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "3. " + value.example[2], Value = value.example[2], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "4. " + value.example[3], Value = value.example[3], Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "5. " + value.example[4], Value = value.example[4], Type = ActionTypes.ImBack });
                Message.Attachments.Add(new HeroCard { Title = "다음 보기중 고르시오", Buttons = actions }.ToAttachment());
            }
            else
            {
                strdata = "\"" + value.meaning + "\"라는 뜻을 가진 사자성어는?";
                Message.Attachments.Add(new HeroCard { Title = "주관식 입니다." }.ToAttachment());
            }

            
            strMessage = strdata;
            return Message;
        }
    }
}