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
    internal class FaqQuestion3Callback : ICommand
    {
        public string Name => "faqQuestion3";

        public bool Contains(string command)
        {
           return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                string text = "<b>Сколько будет стоить доставка?</b>\n\n" +
                    "Точную стоимость доставки можно узнать после оформления заказа, т.к. " +
                    "стоимость зависит от места назначения, веса и габаритов посылки.\n" +
                    "Менеджер свяжется с вами, уточнит комплектацию и выберет наиболее оптимальный способ доставки. ";
                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
