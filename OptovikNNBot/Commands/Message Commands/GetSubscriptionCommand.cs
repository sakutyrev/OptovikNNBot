using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OptovikNNBot.Database;
using OptovikNNBot.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = OptovikNNBot.Database.User;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class GetSubscriptionCommand : ICommand
    {
        public string Name => "Подписаться на рассылку";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                //Создание подключения к БД
                var context = new TgBotAppContext();

                var chatId = update.Message.Chat.Id;
                var userId = chatId;
                var username = update.Message.Chat.Username;

                using (context)
                {
                    var discSubs = new Repository<DiscountsSubscription>(context);
                    var subscription = discSubs.GetAll().FirstOrDefault(ds => ds.Tg_user_id == userId);
                    if (subscription != null && subscription.IsActive != false)
                    {
                        var text = $"Похоже, что вы уже получаете нашу рассылку. Вы хотели бы отписаться от рассылки?";
                        await botClient.SendTextMessageAsync(chatId, text, replyMarkup: DiscCancelInlineKeyboard.GetDiscCancelInlineKeyboard());
                    }
                    else
                    {
                        var discOffer = "Вы хотели бы получать сообщения о появлении новых акций и скидок?";
                        await botClient.SendTextMessageAsync(chatId, discOffer, replyMarkup: DiscOfferInlineKeyboard.GetDiscOfferInlineKeyboard());
                    }
                }
                
            }
        }
    }
}
