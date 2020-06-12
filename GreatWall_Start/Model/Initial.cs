using LionKing.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    [Serializable]
    public class Initial
    {
        static List<Initial> words = new List<Initial>();
        static List<String> topics = new List<string>();
        private string initial;
        private string word;
        private string meaning;
        private string topic;

        public string INITIAL
        {
            get
            {
                return initial;
            }

            set
            {
                initial = value;
            }
        }

        public string Word
        {
            get
            {
                return word;
            }
        }

        public string Meaning
        {
            get
            {
                return meaning;
            }
        }
        public string Topic
        {
            get
            {
                return topic;
            }
            set
            {
                topic = value;
            }
        }

        public Initial(string initial, string word, string meaning, string topic)
        {
            this.initial = initial;
            this.word = word;
            this.meaning = meaning;
            this.topic = topic;
        }

        public static List<Initial> Words
        {
            get
            {
                return words;
            }
            set
            {
                words = value;
            }
        }
        public static List<string> TOPICS
        {
            get
            {
                return topics;
            }
            set
            {
                topics = value;
            }
        }

        private static string strSQL = "SELECT * FROM Initials";

        public static void Loadwords()
        {
            DataSet DB_DS = SQLHelper.RunSQL(strSQL);
            foreach (DataRow row in DB_DS.Tables[0].Rows)
            {
                words.Add(new Initial(row["initial"].ToString(), row["Word"].ToString(), row["meaning"].ToString(), row["topic"].ToString()));
                if(!topics.Contains(row["topic"].ToString()))
                {
                    topics.Add(row["topic"].ToString());
                }
            }

        }
    }
}