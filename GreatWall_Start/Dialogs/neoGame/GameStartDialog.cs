﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LionKing.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LionKing.Dialogs.neoGame
{
    [Serializable]
    public class GameStartDialog : IDialog<string>
    {
        string strMessage;
        private string strWelcomMMessage = "게임난이도를 정해주세요. (Nomal, Hard)";
        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. Nomal", Value = "Nomal", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. Hard", Value = "Hard", Type = ActionTypes.ImBack });

            message.Attachments.Add(new HeroCard { Title = strWelcomMMessage, Buttons = actions }.ToAttachment());

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (strSelected == "Nomal")
            {
                context.Call(new GameLoopDialog(Level.NOMAL), GameOver);
            }
            else if (strSelected == "Hard")
            {
                context.Call(new GameLoopDialog(Level.HARD), GameOver);
            }
            else
            {
                strMessage = "목록중에서 골라 주세요";
                await context.PostAsync(strMessage);
                context.Wait(MessageReceivedAsync);
                return;
            }
        }

        private async Task GameOver(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                strMessage = "Your Score : " + await result;

                await context.PostAsync(strMessage);

                context.Done("Game Over");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
        }
    }
}