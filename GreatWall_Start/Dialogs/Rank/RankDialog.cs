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
            if (gtype == "idom")
            {
                var result1 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel == "E").OrderByDescending(n => n.Score).ToList();
                var result2 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel == "N").OrderByDescending(n => n.Score).ToList();
                var result3 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel == "H").OrderByDescending(n => n.Score).ToList();

                await context.PostAsync(result1.ToString());
                await context.PostAsync(result2.ToString());
                await context.PostAsync(result3.ToString());
            }
            else if(gtype == "neo")
            {
                var result1 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel == "N").OrderByDescending(n => n.Score).ToList();
                var result2 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel == "H").OrderByDescending(n => n.Score).ToList();

                await context.PostAsync(result1.ToString());
                await context.PostAsync(result2.ToString());
            }
            else if(gtype == "initial")
            {
                var result1 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel == "S").OrderByDescending(n => n.Score).ToList();

                await context.PostAsync(result1.ToString());
            }

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Done("");
        }
    }
}