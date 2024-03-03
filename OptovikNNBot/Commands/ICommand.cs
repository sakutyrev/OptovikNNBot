using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OptovikNNBot.Commands
{
    public interface ICommand
    {
        string Name { get; } // Название команды
        bool Contains(string command); // Проверка, содержится ли команда в тексте сообщения или callback data
        Task Execute(ITelegramBotClient botClient, Update update); // Выполнение команды
    }
}
