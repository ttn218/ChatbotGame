using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using LionKing.Helpers;
using LionKing.Model;

namespace LionKing.Dialogs.Rank
{
    [Serializable]
    public class RankDialog : IDialog<string>
    {
        string gtype;

        public RankDialog(string type)
        {
            this.gtype = type;
        }

        public async Task StartAsync(IDialogContext context)
        {
            Model.Rank.Ranks.Clear();
            Model.Rank.LoadRank();
            if (gtype == "idome")
            {
                var result1 = Model.Rank.Ranks.Where(n => (n.Gtype == gtype) && (n.Glevel.Trim() == "E")).OrderByDescending(n => n.Score).ToList();
                var result2 = Model.Rank.Ranks.Where(n => (n.Gtype == gtype) && (n.Glevel.Trim() == "N")).OrderByDescending(n => n.Score).ToList();
                var result3 = Model.Rank.Ranks.Where(n => (n.Gtype == gtype) && n.Glevel.Trim() == "H").OrderByDescending(n => n.Score).ToList();

                await context.PostAsync("사자왕 순위");
                await context.PostAsync(Model.Rank.makeRanking(result1, result2, result3, context));
            }
            else if(gtype == "neo")
            {
                var result1 = Model.Rank.Ranks.Where(n => (n.Gtype == gtype) && (n.Glevel.Trim() == "N")).OrderByDescending(n => n.Score).ToList();
                var result2 = Model.Rank.Ranks.Where(n => (n.Gtype == gtype) && (n.Glevel.Trim() == "H")).OrderByDescending(n => n.Score).ToList();

                await context.PostAsync("아재왕 순위");
                await context.PostAsync(Model.Rank.makeRanking(result1, result2, context));
            }
            else if(gtype == "initial")
            {
                var result1 = Model.Rank.Ranks.Where(n => (n.Gtype == gtype) && (n.Glevel.Trim() == "S")).OrderByDescending(n => n.Score).ToList();

                await context.PostAsync("초성왕 순위");
                await context.PostAsync(Model.Rank.makeRanking(result1, context));
            }
            context.Done("");
        }

    }
}