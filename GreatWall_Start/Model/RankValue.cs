using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LionKing.Model
{
    public class RankValue : ARankValue
    {
        public RankValue(string user, string gtype, int score, string glevel)
        {
            this.user = user;
            this.gtype = gtype;
            this.score = score;
            this.glevel = glevel;
        }
    }
}