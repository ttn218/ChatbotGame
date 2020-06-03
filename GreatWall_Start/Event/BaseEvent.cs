using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using GreatWall.Helpers;

using System.Data;
using System.Data.SqlClient;
using GreatWall.Model;

namespace GreatWall.Event
{
    [Serializable]
    public class BaseEvent : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context) // 초기화
        {
            string strMessage = "성공이다..";
            
            await context.PostAsync(strMessage);

            await this.MessageReceivedAsync(context, null);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            if (result != null)
            {
                Activity activity = await result as Activity;

                if (activity.Text.Trim() == "Exit")
                {

                }
                else
                {

                }
            }
            else
            {

            }
        }

    }
}