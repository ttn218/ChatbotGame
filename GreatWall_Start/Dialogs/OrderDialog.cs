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

namespace GreatWall.Dialogs
{
    [Serializable]
    public class OrderDialog : IDialog<string>
    {
        private string strMessage = null;
        private string strOrder;
        private string strServerUrl = "http://localhost:3984/Images/";
        private string strSQL = "SELECT * FROM Menus";
        List<OrderItem> MenuItems = new List<OrderItem>();


        public async Task StartAsync(IDialogContext context)
        {
            //strMessage = null;
            //strOrder = "[Order Menu List]" + "\n";

            await this.MessageReceivedAsync(context, null);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            if(result != null)
            {
                Activity activity = await result as Activity;

                if (activity.Text.Trim() == "Exit")
                {
                    List<ReceiptItem> receiptItems = new List<ReceiptItem>();
                    Decimal totalPrice = 0;

                    foreach(OrderItem orderItem in MenuItems)
                    {
                        receiptItems.Add(new ReceiptItem()
                        {
                            Title = orderItem.Title,
                            Price = orderItem.Price.ToString("########"),
                            Quantity = orderItem.Quantity.ToString(),
                        });
                        totalPrice += orderItem.Price;
                    }
                    #region
                    /*
                    // sql insert 문제 생김 수정필요
                    SqlParameter[] para =
                    {
                        new SqlParameter("@TotalPrice", SqlDbType.SmallMoney),
                        new SqlParameter("@UserID", SqlDbType.NVarChar, 50)
                    };

                    para[0].Value = totalPrice;
                    para[1].Value = activity.Id;

                    SQLHelper.ExecuteNonQuery("INSERT INTO Order(TotalPrice, UserID, OrderDate) " +
                                                "VALUES(@TotalPrice, @UserID, GETDATE())", para);
                    DataSet orderNumber = SQLHelper.RunSQL("SELECT MAX(OrderID) FROM Order " +
                                                            "WHERE UserID = '" + activity.Id + "'");
                    DataRow row = orderNumber.Tables[0].Rows[0];
                    int orderID = (int)row[0];

                    foreach(OrderItem orderItem in MenuItems)
                    {
                        SqlParameter[] para2 =
                        {
                            new SqlParameter("@OrderID", SqlDbType.Int),
                            new SqlParameter("@ItemName", SqlDbType.NVarChar),
                            new SqlParameter("@ItemPrice", SqlDbType.SmallMoney),
                            new SqlParameter("@Quantity", SqlDbType.Int),
                        };

                        para2[0].Value = orderID;
                        para2[1].Value = orderItem.Title;
                        para2[2].Value = orderItem.Price;
                        para2[3].Value = orderItem.Quantity;

                        SQLHelper.ExecuteNonQuery("INSERT INTO Items(OrderID, ItemName, ItemPrice, Quantity) " +
                                                    "VALUES(@OrderID, @ItemName, @ItemPrice, @Quantity)", para2);
                    }
                    // 여기까지
                    */
                    #endregion
                    var cardMessage = context.MakeMessage();
                    cardMessage.Attachments.Add(CardHelper.GetReceiptCard("[Order Menu List] \n", receiptItems,
                                                                            totalPrice.ToString(), "2%", "10%"));

                    MenuItems.Clear();

                    await context.PostAsync(cardMessage);
                    context.Done("Order Completed");
                }
                else
                {
                    string strSQL = "SELECT * FROM Menus WHERE MenuID = " + activity.Text;
                    DataSet DB_DS = SQLHelper.RunSQL(strSQL);
                    DataRow row = DB_DS.Tables[0].Rows[0];

                    MenuItems.Add(new OrderItem
                    {
                        ItemId = (int)row["MenuID"],
                        Title = row["Title"].ToString(),
                        Price = (Decimal)row["Price"],
                        Quantity = 1,
                    });

                    string strOrderMenus = "You ordered..." + "\n";
                    foreach(OrderItem orderItem in MenuItems)
                    {
                        strOrderMenus += orderItem.Title + ": " + orderItem.Price.ToString("########") + "\n\n";
                    }

                    await context.PostAsync(strOrderMenus);
                    context.Wait(this.MessageReceivedAsync);

                    #region
                    /*
                    strMessage = string.Format("You ordered {0}.", activity.Text);
                    strOrder += activity.Text + "\n";

                    DataSet DB_DS = SQLHelper.RunSQL(strSQL + "WHERE MenuID = " + activity.Text);
                    DataRow row = DB_DS.Tables[0].Rows[0];

                    await context.PostAsync(strMessage);

                    context.Wait(MessageReceivedAsync);
                    */
                    #endregion
                }
            }
            else
            {
                strMessage = "[Food Order Menu] Select the menu you want to order.>";
                await context.PostAsync(strMessage);

                DataSet DB_DS = SQLHelper.RunSQL(strSQL);

                var message = context.MakeMessage();
                foreach(DataRow row in DB_DS.Tables[0].Rows)
                {
                    message.Attachments.Add(CardHelper.GetHeroCard(row["Title"].ToString(),
                                            row["Price"].ToString(),
                                            this.strServerUrl + row["Images"].ToString(),
                                            row["Title"].ToString(), row["MenuID"].ToString()));
                }
                message.Attachments.Add(CardHelper.GetHeroCard("Exit food order...", "Exit", null, "Exit Order", "Exit"));

                #region
                /*
                List<CardImage> menu01_images = new List<CardImage>();
                menu01_images.Add(new CardImage() { Url = this.strServerUrl + "menu_01.jpg" });

                List<CardAction> menu01_Button = new List<CardAction>();

                menu01_Button.Add(new CardAction()
                {
                    Title = "자장면",
                    Value = "자장면",
                    Type = ActionTypes.ImBack
                });

                HeroCard menu_01_Card = new HeroCard()
                {
                    Title = "자장면",
                    Subtitle = "옛날 자장면",
                    Images = menu01_images,
                    Buttons = menu01_Button
                };

                List<CardImage> menu02_images = new List<CardImage>();
                menu02_images.Add(new CardImage() { Url = this.strServerUrl + "menu_02.jpg" });

                List<CardAction> menu02_Button = new List<CardAction>();

                menu02_Button.Add(new CardAction()
                {
                    Title = "짬뽕",
                    Value = "짬뽕",
                    Type = ActionTypes.ImBack
                });

                HeroCard menu_02_Card = new HeroCard()
                {
                    Title = "짬뽕",
                    Subtitle = "굴짬뽕",
                    Images = menu02_images,
                    Buttons = menu02_Button
                };

                List<CardImage> menu03_images = new List<CardImage>();
                menu03_images.Add(new CardImage() { Url = this.strServerUrl + "menu_03.jpg" });

                List<CardAction> menu03_Button = new List<CardAction>();

                menu03_Button.Add(new CardAction()
                {
                    Title = "탕수육",
                    Value = "탕수육",
                    Type = ActionTypes.ImBack
                });

                HeroCard menu_03_Card = new HeroCard()
                {
                    Title = "탕수육",
                    Subtitle = "찹쌀 탕수육",
                    Images = menu03_images,
                    Buttons = menu03_Button
                };

                List<CardAction> menu04_Button = new List<CardAction>();

                menu04_Button.Add(new CardAction()
                {
                    Title = "Exit food order",
                    Value = "Exit",
                    Type = ActionTypes.ImBack
                });

                HeroCard menu_04_Card = new HeroCard()
                {
                    Title = "Exit food order...",
                    Subtitle = null,
                    Buttons = menu04_Button
                };
                var message = context.MakeMessage();
                message.Attachments.Add(menu_01_Card.ToAttachment());
                message.Attachments.Add(menu_02_Card.ToAttachment());
                message.Attachments.Add(menu_03_Card.ToAttachment());
                message.Attachments.Add(menu_04_Card.ToAttachment());
                */
                #endregion

                #region
                /*
                var message = context.MakeMessage();
                message.Attachments.Add(CardHelper.GetHeroCard("짜장면", "5000원", this.strServerUrl + "menu_01.jpg", "짜장면", "짜장면"));
                message.Attachments.Add(CardHelper.GetHeroCard("짬뽕", "6000원", this.strServerUrl + "menu_02.jpg", "짬뽕", "짬뽕"));
                message.Attachments.Add(CardHelper.GetHeroCard("탕수육", "12000원", this.strServerUrl + "menu_03.jpg", "탕수육", "탕수육"));
                message.Attachments.Add(CardHelper.GetThumbnailCard("Exit", null, null, "Exit", "Exit"));
                */
                #endregion


                message.AttachmentLayout = "carousel";
                
                await context.PostAsync(message);

                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}