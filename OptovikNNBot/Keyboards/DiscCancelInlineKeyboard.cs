using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace OptovikNNBot.Keyboards
{
    internal class DiscCancelInlineKeyboard
    {
        public static InlineKeyboardMarkup GetDiscCancelInlineKeyboard()
        {

            InlineKeyboardMarkup discCancelKeyboard = new(new[]
            {
                new []{ InlineKeyboardButton.WithCallbackData(text: "Да", callbackData: "discount_cancel_yes"),
                        InlineKeyboardButton.WithCallbackData(text: "Нет", callbackData: "discount_cancel_no")},
            });
            return discCancelKeyboard;
        }
    }
}
