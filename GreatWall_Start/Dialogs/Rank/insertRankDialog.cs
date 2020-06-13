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
    public class insertRankDialog : IDialog<string>
    {
        private string strdata = "축하합니다. 랭킹에 등록하실 수 있게 되었습니다. 이니셜을 입력해주세요.";
        string score;

        public insertRankDialog(string score)
        {
            this.score = score;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(strdata);

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            // insert


            context.Done(this.score);
        }
    }
}