using OptovikNNBot.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class HelpCommand:ICommand
    {
        public string Name => "Помощь";

        public bool Contains(string command) 
        {
            return command.Contains(Name); 
        }

        public async Task Execute(ITelegramBotClient botClient, Update update) 
        {
            if (update.Message?.Chat.Id != null)
            {
                var chatId = update.Message.Chat.Id;
                var text = "Выберите интересующий раздел:";
                await botClient.SendTextMessageAsync(chatId, text, replyMarkup: HelpKeyboard.GetHelpKeyboard());
            }
        }
    }
}
