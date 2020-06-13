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
                var result1 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel.Trim().Equals("E")).OrderByDescending(n => n.Score).ToList();
                var result2 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel.Trim().Equals("N")).OrderByDescending(n => n.Score).ToList();
                var result3 = Model.Rank.Ranks.Where(n => n.Gtype == gtype && n.Glevel.Trim().Equals("H")).OrderByDescending(n => n.Score).ToList();
                // 각 난이도별 몇등까지 출력 ?
                // 각 난이도별 
                string strData1 = "";
                string strData2 = "";
                string strData3 = "";

                foreach (var result in result1)
                {             
                    strData1 += "사용자 :" + result.User + "/// " +"점수 :" + result.Score +
                        "//// " + "난이도" + result.Glevel + Environment.NewLine;
                }
                foreach (var result in result2)
                {
                    strData2 += "사용자 :" + result.User + "/// " + "점수 :" + result.Score + 
                        "//// " + "난이도" + result.Glevel + Environment.NewLine; 
                }
                foreach (var result in result3)
                {
                    strData3 += "사용자 :" + result.User + "/// " + "점수 :" + result.Score + 
                        "//// " + "난이도" + result.Glevel + Environment.NewLine; 
                }

                await context.PostAsync(strData1);
                await context.PostAsync(strData2);
                await context.PostAsync(strData3);
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