using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace OptovikNNBot.Keyboards
{
    public static class FaqInlineKeyboard
    {
        public static InlineKeyboardMarkup GetFaqInlineKeyboard()
        {
            
            InlineKeyboardMarkup faqKeyboard = new(new[]
            {

                new []{ InlineKeyboardButton.WithCallbackData(text: "Вы продаёте пряжу только упаковками?", callbackData: "faqQuestion1") },
                new []{ InlineKeyboardButton.WithCallbackData(text: "Когда заказ считается оптовым?", callbackData: "faqQuestion2") },
                new []{ InlineKeyboardButton.WithCallbackData(text: "Сколько будет стоить доставка?", callbackData: "faqQuestion3") },
                new []{ InlineKeyboardButton.WithCallbackData(text: "Как происходит отправка товара?", callbackData: "faqQuestion4") },
                new []{ InlineKeyboardButton.WithCallbackData(text: "Вы работаете с \"Наложенным Платежом\"?", callbackData: "faqQuestion5") },
                new []{ InlineKeyboardButton.WithCallbackData(text: "Можно ли самому приехать на склад?", callbackData: "faqQuestion6") }

            });
            return faqKeyboard;
        }
    }
}
