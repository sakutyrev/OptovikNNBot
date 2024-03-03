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
    internal class GuaranteeInfoCommand:ICommand
    {
        public string Name => "Гарантия на товар";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                string text = "Потребитель может обменять товар на аналогичный у продавца в течение 14 дней после покупки при выполнении следующих условий:\n\n" +
                    "1️⃣ Наличие товарного/кассового чека\n" +
                    "2️⃣ Товарные свойства сохранены\n" +
                    "Если аналогичного товара нет в продаже, покупатель вправе потребовать возврать уплаченной суммы.\n\n" +
                    "Если товар не соответствует индивидуально-потребительским свойствам покупателя, он может отказаться от него. " +
                    "В данном случае продавец в течение 10-ти дней с момента требования обязан вернуть оплату.";
                var chatId = update.Message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
            }
        }
    }
}
