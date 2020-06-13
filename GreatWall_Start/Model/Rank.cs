using LionKing.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

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
        public void MakeRank(IDialogContext context)
        {
            
        }
    }
}