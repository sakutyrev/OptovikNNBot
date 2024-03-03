using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace OptovikNNBot.Keyboards
{
    public static class ReviewsInlineKeyboard
    {
        public static InlineKeyboardMarkup GetReviewsKeyboard()
        {
          InlineKeyboardMarkup reviewsKeyboard = new(new[]
                   {
            new[]
            {
                    InlineKeyboardButton.WithUrl("Отзывы ВКонтакте", "https://vk.com/optovik_nn?w=app6326142_-139175168"),
            },
            new[]
            {
                    InlineKeyboardButton.WithUrl("Отзывы в Яндекс", "https://yandex.ru/maps/org/optovik_nn/1698900539/reviews/?indoorLevel=1&ll=44.004603%2C56.290909&z=17"),
            },
            new[]
            {
                    InlineKeyboardButton.WithUrl("Отзывы в Гугл", "https://g.page/optnn?share"),
            },
            new[]
            {
                    InlineKeyboardButton.WithUrl("Отзывы в 2ГИС", "https://2gis.ru/n_novgorod/firm/2674541559818104/tab/reviews"),
            }
                });
            return reviewsKeyboard;
        } 
    }
}
