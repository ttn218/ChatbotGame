using LionKing.Helpers;
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
        
        public static string makeRanking(List<Model.Rank> easy, List<Model.Rank> nomal, List<Model.Rank> hard)
        {
            string strdata = "\t\tEasy\t\t\t\t\tNomal\t\t\t\t\tHard" + Environment.NewLine
                            + "순위\t닉네임\t\t스코어\t\t닉네임\t\t스코어\t\t닉네임\t\t스코어\t\t" + Environment.NewLine;
            string strlist = null;
            for(int i=0; i < 10; i++)
            {
                strlist = + (i+1) + "\t" + easy[i].User + "\t\t" + easy[i].Score + "\t\t" + nomal[i].User + "\t\t" + nomal[i].Score + "\t\t" + hard[i].User + "\t\t" + hard[i].Score + Environment.NewLine;
            }

            return strdata + strlist;
        }

        public static string makeRanking(List<Model.Rank> nomal, List<Model.Rank> hard)
        {
            string strdata = "\t\tNomal\t\t\t\t\tHard" + Environment.NewLine
                           + "순위\t닉네임\t\t스코어\t\t닉네임\t\t스코어" + Environment.NewLine;
            string strlist = null;
            for (int i = 0; i < 10; i++)
            {
                strlist = +(i + 1) + "\t" + nomal[i].User + "\t\t" + nomal[i].Score + "\t\t" + hard[i].User + "\t\t" + hard[i].Score + Environment.NewLine;
            }

            return strdata + strlist;
        }

        public static string makeRanking(List<Model.Rank> topic)
        {
            string strdata = "\t\tEasy\t\t\t\t\tNomal\t\t\t\t\tHard" + Environment.NewLine
                           + "순위\t닉네임\t\t스코어\t\t닉네임\t\t스코어\t\t닉네임\t\t스코어\t\t" + Environment.NewLine;
            string strlist = null;
            for (int i = 0; i < 10; i++)
            {
                strlist = +(i + 1) + "\t" + topic[i].User + "\t\t" + topic[i].Score + Environment.NewLine;
            }

            return strdata + strlist;
        }
    }
}