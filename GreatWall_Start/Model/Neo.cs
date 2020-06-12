using LionKing.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    [Serializable]
    public class Neo
    {
        static List<Neo> words = new List<Neo>();

        private string word;
        private string meaning;

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

        public Neo(string word, string meaning)
        {
            this.word = word;
            this.meaning = meaning;
        }

        public static List<Neo> Words
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

        private static string strSQL = "SELECT * FROM Neos";

        public static void Loadwords()
        {
            DataSet DB_DS = SQLHelper.RunSQL(strSQL);
            foreach (DataRow row in DB_DS.Tables[0].Rows)
            {
                words.Add(new Neo(row["NeoWord"].ToString(), row["meaning"].ToString()));
            }

        }
    }
}