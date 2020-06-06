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
        private static List<Idiom> idioms = new List<Idiom>();
        private string word;
        private string meaning;

        private static string strSQL = "SELECT * FROM Idioms";

        public static List<Idiom> Idioms
        {
            get
            {
                return idioms;
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

        public Idiom(string word, string meaning)
        {
            this.word = word;
            this.meaning = meaning;

            
        }

        public static void Loadidiom()
        {
            DataSet DB_DS = SQLHelper.RunSQL(strSQL);
            foreach(DataRow row in DB_DS.Tables[0].Rows)
            {
                idioms.Add(new Idiom(row["Word"].ToString(), row["meaning"].ToString()));
            }
        }

    }
}