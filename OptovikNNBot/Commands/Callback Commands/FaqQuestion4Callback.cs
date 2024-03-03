using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OptovikNNBot.Commands.Callback_Commands
{
    internal class FaqQuestion4Callback : ICommand
    {
        public string Name => "faqQuestion4";

        public bool Contains(string command)
        {
           return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                string text = "<b>Как происходит отправка товара?</b>\n\n" +
                    "После подтверждения менеджером всех деталей заказа и поступления его оплаты, в течение двух рабочих дней заказ " +
                    "будет передан в отделение Почты России или Транспортной Компании, в зависимости от выбранного способа доставки.";
                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
