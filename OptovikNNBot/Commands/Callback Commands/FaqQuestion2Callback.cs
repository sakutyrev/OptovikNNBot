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
    internal class FaqQuestion2Callback : ICommand
    {
        public string Name => "faqQuestion2";

        public bool Contains(string command)
        {
           return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                string text = "<b>Когда заказ считается оптовым?</b>\n\n" +
                    "Все товары в магазине представлены с оптовыми ценами.\n\n" +
                    "Товары доступны по оптовым ценам при покупке от 3000 рублей. При покупке на меньшую сумму на каждый товар будет добавлена розничная наценка 20%.";
                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
