using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using QnAMakerDialog.Models;
using QnAMakerDialog;

namespace GreatWall.Dialogs
{
    [Serializable]
    [QnAMakerService("https://greatwallqna.azurewebsites.net/qnamaker",
                    "75cf4e71-783f-4be9-ba61-e43762292753",
                    "74b1576d-49ab-4fba-b0de-7c742dd3ac7a", MaxAnswers = 5)]

    public class FAQDialog : QnAMakerDialog<string>
    {
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync($"Sorry, I couldn't find an answer for'{originalQueryText }'.");
            context.Wait(MessageReceived);
        }

        public override async Task DefaultMatchHandler(IDialogContext context,
                                                        string originalQueryText, QnAMakerResult result)
        {
            if(originalQueryText == "Exit")
            {
                context.Done("");
                return;
            }
            await context.PostAsync(result.Answers.First().Answer);
            context.Wait(MessageReceived);
        }

        [QnAMakerResponseHandler(0.5)]
        public async Task LowScoreHandler(IDialogContext context, 
                                            string originalQueryText, QnAMakerResult result)
        {
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);

            messageActivity.Text = $"I found an answer that might help..." +
                                       $"{result.Answers.First().Answer}.";

            await context.PostAsync(messageActivity);

            context.Wait(MessageReceived);
        }

        #region
        /*
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("FAQ Service");

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;

            if (activity.Text.Trim() == "Exit")
            {
                context.Done("Order Completed");
            }
            else
            {
                await context.PostAsync("FAQ Dialog.");

                context.Wait(MessageReceivedAsync);
            }
        }
        */
        #endregion
    }
}