using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OptovikNNBot.Commands.Callback_Commands
{
    internal class DiscCancelNoCallback:ICommand
    {
        public string Name => "discount_cancel_no";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                var messageId = update.CallbackQuery.Message.MessageId;
                var chatId = update.CallbackQuery.Message.Chat.Id;
                var callbackQueryId = update.CallbackQuery.Id;

                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
                var text = $"Хорошо! Мы будем и дальше уведомлять вас о новых скидках и акциях!";
                await botClient.EditMessageTextAsync(chatId, messageId, text, replyMarkup: null);
            }
        }
    }
}
