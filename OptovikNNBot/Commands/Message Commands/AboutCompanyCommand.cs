using OptovikNNBot.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OptovikNNBot.Commands
{
    public class AboutCompanyCommand:ICommand
    {
        public string Name => "О компании";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                var chatId = update.Message.Chat.Id;
                var text = "Что именно вас интересует?";
                await botClient.SendTextMessageAsync(chatId, text, replyMarkup: AboutCompanyInlineKeyboard.GetAboutInlineKeyboard());
            }
        }
    }
}
