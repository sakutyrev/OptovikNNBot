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
    public class StartCommand: ICommand
    {
        public string Name => "/start";
        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                var chatId = update.Message.Chat.Id;
                var text = "Здравствуйте, чем я могу помочь?";
                await botClient.SendTextMessageAsync(chatId, text, replyMarkup: MainKeyboard.GetMainKeyboard());
            }
        }

    }
}
