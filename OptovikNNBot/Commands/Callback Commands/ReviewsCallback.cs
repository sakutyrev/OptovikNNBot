using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Windows.Input;
using Telegram.Bot.Types.Enums;
using OptovikNNBot.Keyboards;

namespace OptovikNNBot.Commands.Callback_Commands
{
    internal class ReviewsCallback:ICommand
    {
        public string Name => "reviews";

        public bool Contains(string callbackData)
        {
            return callbackData.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId,
                            "<b>Ознакомьтесь с отзывыми о нас или оставьте свой в сервисах ниже:</b>",
                            ParseMode.Html,
                            replyMarkup: ReviewsInlineKeyboard.GetReviewsKeyboard());
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
