using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class OrderInfoCommand: ICommand
    {
        public string Name => "Оформление заказа";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                string text = "<b>Оформить заказ возможно на нашем сайте Оптовик-НН</b>\n\n" +
                    "🔎 <a href = \"https://optovik-nn.ru\">Сайт Оптовик-НН</a>\n\n" +
                    "1️⃣ Контактный номер телефона\n" +
                    "2️⃣ ФИО\n" +
                    "3️⃣ Электронная почта (при наличии)\n\n" +
                    "После оформления заказа, менеджер-консультант свяжется с Вами и уточнит правильность его комплектации," +
                    "а также подберёт удобный способ оплаты и доступные варианты доставки.";
                var chatId = update.Message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html, disableWebPagePreview:true);
            }
        }
    }
}
