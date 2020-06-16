using LionKing.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    [Serializable]
    public class Rank
    {
        static List<Rank> ranks = new List<Rank>();

        private string user;
        private string gtype;
        private int score;
        private string glevel;
        public static IMessageActivity Message { get; set; }

        public string User
        {
            get
            {
                return user;
            }
        }

        public string Gtype
        {
            get
            {
                return gtype;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
        }

        public string Glevel
        {
            get
            {
                return glevel;
            }
        }

        public static List<Rank> Ranks
        {
            get
            {
                return ranks;
            }
            set
            {
                ranks = value;
            }
        }

        public Rank(string u, string t, int s, string l)
        {
            this.user = u;
            this.gtype = t;
            this.score = s;
            this.glevel = l;
        }
        
        public static string selectSql = "select g_user, g_type, g_level, score from ranking";
        
        public static void LoadRank()
        {
            DataSet DB_DS = SQLHelper.RunSQL(selectSql);
            foreach (DataRow row in DB_DS.Tables[0].Rows)
            {
                ranks.Add(new Rank(row["g_user"].ToString(), row["g_type"].ToString(), Int32.Parse(row["score"].ToString()), row["g_level"].ToString()));
            }
        }
        
        public static IMessageActivity makeRanking(List<Model.Rank> easy, List<Model.Rank> nomal, List<Model.Rank> hard, IDialogContext context)
        {
            Message = context.MakeMessage();

            List<CardAction> actions1 = new List<CardAction>();
            List<CardAction> actions2 = new List<CardAction>();
            List<CardAction> actions3 = new List<CardAction>();

            for(int i=0; i < easy.Count && i < 7; i++)
            {
                actions1.Add(new CardAction() { Title = (i+1)+"등 / 닉네임 : " + easy[i].User + " / 점수 : " + easy[i].score, Value = "", Type = ActionTypes.PostBack});
            }
            Message.Attachments.Add(new HeroCard { Title = "사자왕 Easy 순위", Buttons = actions1 }.ToAttachment());

            for (int i = 0; i < nomal.Count && i < 7; i++)
            {
                actions2.Add(new CardAction() { Title = (i + 1) + "등 / 닉네임 : " + nomal[i].User + " / 점수 : " + nomal[i].score, Value = "", Type = ActionTypes.PostBack });
            }
            Message.Attachments.Add(new HeroCard { Title = "사자왕 Nomal 순위", Buttons = actions2 }.ToAttachment());

            for (int i = 0; i < hard.Count && i < 7; i++)
            {
                actions3.Add(new CardAction() { Title = (i + 1) + "등 / 닉네임 : " + hard[i].User + " / 점수 : " + hard[i].score, Value = "", Type = ActionTypes.PostBack });
            }
            Message.Attachments.Add(new HeroCard { Title = "사자왕 Hard 순위", Buttons = actions3 }.ToAttachment());

            Message.AttachmentLayout = "carousel";

            return Message;
        }

        public static IMessageActivity makeRanking(List<Model.Rank> nomal, List<Model.Rank> hard, IDialogContext context)
        {
            Message = context.MakeMessage();

            List<CardAction> actions1 = new List<CardAction>();
            List<CardAction> actions2 = new List<CardAction>();

            for (int i = 0; i < nomal.Count && i < 7 ; i++)
            {
                actions1.Add(new CardAction() { Title = (i + 1) + "등 / 닉네임 : " + nomal[i].User + " / 점수 : " + nomal[i].score, Value = "", Type = ActionTypes.PostBack });
            }
            Message.Attachments.Add(new HeroCard { Title = "아재왕 Nomal 순위", Buttons = actions1 }.ToAttachment());

            for (int i = 0; i < hard.Count && i < 7 ; i++)
            {
                actions2.Add(new CardAction() { Title = (i + 1) + "등 / 닉네임 : " + hard[i].User + " / 점수 : " + hard[i].score, Value = "", Type = ActionTypes.PostBack });
            }
            Message.Attachments.Add(new HeroCard { Title = "아재왕 Hard 순위", Buttons = actions2 }.ToAttachment());

            Message.AttachmentLayout = "carousel";

            return Message;
        }

        public static IMessageActivity makeRanking(List<Model.Rank> topic, IDialogContext context)
        {
            Message = context.MakeMessage();

            List<CardAction> actions = new List<CardAction>();
            
            var result1 = Model.Rank.Ranks.Where(n => (n.Gtype.Trim() == "initial")).OrderByDescending(n => n.Score).ToList();
            int Count = result1.Select(v => v.Glevel).Distinct().Count();
            string[] strchar = result1.Select(v => v.Glevel).Distinct().ToArray();


            for (int i = 0; i<Count; i++)
            {
                topic = result1.Where(v => v.glevel == strchar[i]).OrderByDescending(n => n.Score).ToList();
                actions = new List<CardAction>();
                for (int j = 0; j < topic.Count && i < 7; j++)
                {
                    actions.Add(new CardAction() { Title = (j + 1) + "등 / 닉네임 : " + topic[j].User + " / 점수 : " + topic[j].score, Value = "", Type = ActionTypes.PostBack });
                }
                Message.Attachments.Add(new HeroCard { Title = "초성왕" + strchar[i] + "순위", Buttons = actions }.ToAttachment());
            }

            Message.AttachmentLayout = "carousel";

            return Message;
        }
    }
}