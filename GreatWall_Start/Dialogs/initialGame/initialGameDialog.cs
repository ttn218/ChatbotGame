using LionKing.Dialogs.Rank;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LionKing.Dialogs.initialGame
{
    [Serializable]
    public class initialGameDialog : IDialog<string>
    {
        string type = "initial";
        string manual = "initialGame (초성 게임)" + Environment.NewLine
                        + "초성왕! 게임은 주어진 주제 중, 한 가지 주제를 선택하여 게임을 시작합니다." + Environment.NewLine + Environment.NewLine
                        + "제한 시간은 300초이며, 시간 내에 가능한 한 많은 문제를 맞혀 높은 점수를 따내면 되는 게임입니다." + Environment.NewLine

                        + "주제는 [나라 수도 맞추기]와 [동물 이름 맞추기] 두 가지로 구성됩니다." + Environment.NewLine
                        + "[나라 수도 맞추기]는 제시된 나라의 수도와 초성 단어를 보고 정답을 유추하여 맞추면 됩니다." + Environment.NewLine
                        + "[동물 이름 맞추기]는 제시된 동물의 설명과 초성 단어를 보고 정답을 유추하여 맞추면 됩니다." + Environment.NewLine
                        + "모든 문제는 주관식으로 출제됩니다.";

        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. 게임시작", Value = "게임시작", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 게임설명", Value = "게임설명", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 랭킹", Value = "랭킹", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 나가기", Value = "나가기", Type = ActionTypes.ImBack });

            message.Attachments.Add(new HeroCard { Title = "초성퀴즈 초성왕!", Buttons = actions }.ToAttachment());

            await context.PostAsync(message);

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (strSelected == "게임시작")
            {
                context.Call(new GameStartDialog(), DialogResumeAfter);
            }
            else if (strSelected == "게임설명")
            {
                await context.PostAsync(manual);
                await StartAsync(context);
            }
            else if (strSelected == "랭킹")
            {
                context.Call(new RankDialog(type), DialogResumeAfter);
            }
            else if (strSelected == "나가기")
            {
                context.Done("Exit");
            }
        }

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                await this.StartAsync(context);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred...");
            }
        }
    }
}