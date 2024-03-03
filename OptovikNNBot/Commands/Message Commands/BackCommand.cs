using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using OptovikNNBot.Keyboards;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class BackCommand:ICommand
    {
        public string Name => "⬅️Назад";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                var chatId = update.Message.Chat.Id;
                string text = "Чем еще я могу быть полезен?";
                await botClient.SendTextMessageAsync(chatId: chatId, text, replyMarkup: MainKeyboard.GetMainKeyboard());
            }
        }
    }
}
