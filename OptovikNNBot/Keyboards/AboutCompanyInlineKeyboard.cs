using Telegram.Bot.Types.ReplyMarkups;

namespace OptovikNNBot.Keyboards
{
    public static class AboutCompanyInlineKeyboard
    {
        public static InlineKeyboardMarkup GetAboutInlineKeyboard()
        {
            InlineKeyboardMarkup aboutCompanyKeyboard = new(new[]
            {
                new []{ InlineKeyboardButton.WithCallbackData(text: "Контакты и график работы📞", callbackData: "workingHours") },
                new []{ InlineKeyboardButton.WithCallbackData(text: "Местоположение склада📍", callbackData:"location")},
                new []{ InlineKeyboardButton.WithCallbackData(text: "Отзывы⭐️", callbackData: "reviews") }

            });
            return aboutCompanyKeyboard;
        }
    }
}
