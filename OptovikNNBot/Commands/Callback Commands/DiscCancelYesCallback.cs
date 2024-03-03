using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OptovikNNBot.Commands.Message_Commands;
using OptovikNNBot.Database;
using OptovikNNBot.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OptovikNNBot.Commands.Callback_Commands
{
    internal class DiscCancelYesCallback : ICommand
    {
        public string Name => "discount_cancel_yes";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                //Создание подключения к БД
                var context = new TgBotAppContext();

                var chatId = update.CallbackQuery.Message.Chat.Id;
                var userId = chatId;
                var messageId = update.CallbackQuery.Message.MessageId;
                var username = update.CallbackQuery.Message.Chat.Username;
                GetSubscriptionCommand getSub = new GetSubscriptionCommand();

                using (context)
                {
                    var discSubs = new Repository<DiscountsSubscription>(context);
                    var subscription = discSubs.GetAll().FirstOrDefault(ds => ds.Tg_user_id == userId);
                    subscription!.IsActive = false;
                    discSubs.Update(subscription);
                }
                
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
                
                var text = $"Подписка на рассылку отключена! \r\rЕсли вы вновь захотите получать получать уведомления, " +
                    $"вы всегда сможете подключить подписку заново нажав на кнопку \"{getSub.Name}\"";
                await botClient.EditMessageTextAsync(chatId, messageId, text, replyMarkup: null);
            }
        }
    }
}
