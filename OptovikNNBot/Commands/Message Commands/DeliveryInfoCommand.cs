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
    internal class DeliveryInfoCommand:ICommand
    {
        public string Name => "Условия доставки";

        public bool Contains(string command) 
        {
            return command.Contains(Name); 
        }

        public async Task Execute(ITelegramBotClient botClient, Update update) 
        {
            if (update.Message?.Chat.Id != null)
            {
                string text = "<b>1️⃣ Самовывоз из магазина</b>\n\n" +
                    "Наш адрес: г.Нижний Новгород, ул.Заярская, д.18.\n" +
                    "Режим работы: Пн - Чт с 9:00 до 17:45, Пт с 8:00 до 16:45, Сб - Вс Выходные дни.\n\n" +
                    "<b>2️⃣ Почта России / Транспортные Компании</b>\n\n" +
                    "После подтверждения менеджером всех деталей заказа и поступления его оплаты, в течение двух рабочих дней " +
                    "заказ будет передан в отделение Почты России или Транспортной Компании, в зависимости от выбранного способа доставки.\n" +
                    "После отправки менеджер сообщит Вам трек-номер для отслеживания посылки.\n\n" +
                    "В случае необходимости, в рабочее время Вы можете связаться с нами по телефону или по электронной почте " +
                    "info@optovik-nn.ru и наши менеджеры сообщат Вам на какой стадии доставки находится посылка.";
                
                var chatId = update.Message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
            }
             
        }
    }
}
