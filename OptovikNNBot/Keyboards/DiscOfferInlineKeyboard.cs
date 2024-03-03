using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace OptovikNNBot.Keyboards
{
    internal class DiscOfferInlineKeyboard
    {
        public static InlineKeyboardMarkup GetDiscOfferInlineKeyboard()
        {

            InlineKeyboardMarkup discOfferKeyboard = new(new[]
            {
                new []{ InlineKeyboardButton.WithCallbackData(text: "Да", callbackData: "discounts_yes"),
                        InlineKeyboardButton.WithCallbackData(text: "Нет", callbackData: "discounts_no")},
            });
            return discOfferKeyboard;
        }
    }
}
