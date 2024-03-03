using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OptovikNNBot.Commands
{
    internal class WorkingHoursCallback: ICommand
    {
        public string Name => "workingHours";

        public bool Contains(string callbackData)
        {
            return callbackData.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update) 
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                var text = "<b>Телефоны для связи</b>\n+7 (910) 381-21-21\n+7 (831) 412-15-51" +
                    "\n\n<b>Email</b>\ninfo@optovik-nn.ru" +
                    "\n\n<b>Режим работы\n</b>Пн - Чт: с 9:00 до 17:45\nПт: с 8:00 до 16:45\nСубб - Вскр: Выходной";

                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
