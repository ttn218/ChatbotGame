using LionKing.Dialogs.Rank;
using LionKing.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LionKing.Dialogs.IdiomGame
{
    [Serializable]
    public class IdiomGameDialog : IDialog<string>
    {
        string type = "idome";
        string manual = "IdiomGame (사자성어 게임)" + Environment.NewLine
                        + "사자왕! 게임은 제시되는 설명을 읽고, 해당 뜻이 의미하는 사자성어를 맞추는 게임입니다." + Environment.NewLine + Environment.NewLine
                        + "제한 시간은 300초이며, 시간 내에 가능한 한 많은 문제를 맞혀 높은 점수를 따내면 되는 게임입니다." + Environment.NewLine
                        
                        + "난이도는 [Easy], [Normal], [Hard] 세 가지로 구성됩니다." + Environment.NewLine
                        + "[Easy] 난이도는 사자성어에 빈칸이 하나 존재합니다. 빈칸 안에 들어갈 글자를 써넣으면 됩니다." + Environment.NewLine
                        + "[Normal] 난이도는 사자성어에 빈칸이 두개 존재합니다. 빈칸 안에 들어갈 글자를 써넣으면 됩니다." + Environment.NewLine
                        + "[Hard] 난이도는 힌트가 없는 주관식 문제입니다. 해당 뜻을 가진 사자성어를 써넣으면 됩니다.";

        public async Task StartAsync(IDialogContext context)
        {
            
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. 게임시작", Value = "게임시작", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 게임설명", Value = "게임설명", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 랭킹", Value = "랭킹", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 나가기", Value = "나가기", Type = ActionTypes.ImBack });

            message.Attachments.Add(new HeroCard { Title = "사자성어 게임봇 사자왕!", Buttons = actions }.ToAttachment());

            await context.PostAsync(message);

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if(strSelected == "게임시작")
            {
                context.Call(new GameStartDialog(), DialogResumeAfter);
            }
            else if(strSelected == "게임설명")
            {
                await context.PostAsync(manual);
                await StartAsync(context);
            }
            else if(strSelected == "랭킹")
            {
                context.Call(new RankDialog(type), DialogResumeAfter);
            }
            else if(strSelected == "나가기")
            {
                context.Done("Exit");
            }
        }

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
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