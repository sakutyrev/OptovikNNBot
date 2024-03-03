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
    internal class FaqQuestion6Callback : ICommand
    {
        public string Name => "faqQuestion6";

        public bool Contains(string command)
        {
           return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                string text = "<b>Можно приехать на ваш склад и самому выбрать товары?</b>\n\n" +
                    "В рабочее время Вы можете приехать на склад и сами выбрать понравившиеся товары.\n\n" +
                    "Наши менеджеры проконсультируют вас по любому возникшему вопросу.\n\n" +
                    "📍Наш адрес: г.Нижний Новгород, ул.Заярская, д.18.\n" +
                    "📍Режим работы: Пн - Чт с 9:00 до 17:45, Пт с 8:00 до 16:45, Сб - Вс Выходные дни.";
                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
