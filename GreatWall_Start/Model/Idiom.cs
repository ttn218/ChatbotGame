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
    public class Idiom
    {
        static List<Idiom> words = new List<Idiom>();

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

        public Idiom(string word, string meaning)
        {
            this.word = word;
            this.meaning = meaning;
        }

        public static List<Idiom> Words { get => words; set => words = value; }

        private static string strSQL = "SELECT * FROM Idioms";

        public static void Loadwords()
        {
            DataSet DB_DS = SQLHelper.RunSQL(strSQL);
            foreach(DataRow row in DB_DS.Tables[0].Rows)
            {
                words.Add(new Idiom(row["Word"].ToString(), row["meaning"].ToString()));
            }
        }

    }
}