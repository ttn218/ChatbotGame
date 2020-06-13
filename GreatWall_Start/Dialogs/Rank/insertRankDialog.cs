using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using LionKing.Helpers;
using LionKing.Model;
using System.Data.SqlClient;

namespace LionKing.Dialogs.Rank
{
    [Serializable]
    public class insertRankDialog : IDialog<string>
    {
        private string strdata = "축하합니다. 랭킹에 등록하실 수 있게 되었습니다. 이니셜을 입력해주세요.";
        string score;
        string type;
        string level;

        public insertRankDialog(string type, string level,string score)
        {
            this.type = type;
            this.level = level;
            this.score = score;
        }

        public async Task StartAsync(IDialogContext context)
        {
            if( this.type == "initial")
            {
                if(this.level == "수도")
                {
                    this.level = "S";
                }
            }

            await context.PostAsync(strdata);

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strdata = activity.Text.Trim();
            // insert
            SqlParameter[] para =
                    {
                        new SqlParameter("@g_user", strdata),
                        new SqlParameter("@g_type", this.type),
                        new SqlParameter("@score", this.score),
                        new SqlParameter("@g_level", this.level)
                    };

            SQLHelper.ExecuteNonQuery("INSERT INTO ranking(g_user, g_type, score, g_level) " +
                                                "VALUES(@g_user, @g_type, @score, @g_level)", para);
            
            context.Done(this.score);
        }
    }
}