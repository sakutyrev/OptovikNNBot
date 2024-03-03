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
    internal class FaqQuestion1Callback : ICommand
    {
        public string Name => "faqQuestion1";

        public bool Contains(string command)
        {
           return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                string text = "<b>Вы продаёте пряжу только упаковками?</b>\n\n" +
                    "Вне зависимости от суммы заказа, в нашем магазине пряжа продаётся только упаковками.";
                await botClient.SendTextMessageAsync(chatId, text,parseMode: ParseMode.Html);
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
