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
    public class RankDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {

        }
    }
}