using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class PaymentInfoCommand : ICommand
    {
        public string Name => "Условия оплаты";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                string text = "<b>1️⃣ Оплата наличными</b>\n\n" +
                    "Оплата наличными доступна только при самовывозе из магазина.\n\n" +
                    "<b>2️⃣ Оплата банковской картой</b>\n\n" +
                    "Вы можете сделать перевод для оплаты заказа в банкомате или интернет-банке.\n\n" +
                    "<b>3️⃣ Оплата с расчетного счета</b>\n\n" +
                    "Юридические лица могут производить оплату по безналичному расчету. " +
                    "Для оплаты по счету пришлите запрос с реквизитами организации и адресом доставки на нашу почту info@optovik-nn.ru";
               
                var chatId = update.Message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
            }
        }
    }
}
