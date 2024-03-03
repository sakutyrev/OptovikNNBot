using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Runtime.CompilerServices;
using Telegram.Bot.Types.Enums;
using System.IO;
using OptovikNNBot.Keyboards;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class FAQCommand:ICommand
    {
        public string Name => "Часто задаваемые вопросы";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                string text = "Какой вопрос вас интересует?";
                var chatId = update.Message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId, text, replyMarkup: FaqInlineKeyboard.GetFaqInlineKeyboard());
            }
        }
    }
}
