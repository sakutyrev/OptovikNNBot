using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OptovikNNBot.Enums;
using OptovikNNBot.Database;
using OptovikNNBot.Keyboards;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class RemoveAdminCommand : ICommand
    {
        public string Name => "/removeadmin";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {

            if (update.Message?.Chat.Id != null)
            {
                //Создание подключения к БД
                var context = new TgBotAppContext();
                
                bool isAdmin = false;
                var chatId = update.Message.Chat.Id;
                
                using (context)
                {
                    var admins = new Repository<Admin>(context);
                    var adminsList = admins.GetAll().ToList();
                    if(adminsList == null || adminsList.Count == 0) { throw new ArgumentNullException("Ни один админ не инициализирован в БД"); }
                    
                    foreach (var admin in adminsList)
                    {
                        if (admin.TgUserId == update.Message.Chat.Id) 
                            isAdmin=true;
                    }
                    
                    if (!isAdmin) return;//Если пользователь, который ввел эту команду не является администратором
                    
                    var userStates = new Repository<UserState>(context);
                    
                    var text = "Выберите админа, которого вы хотите удалить:";
                    await botClient.SendTextMessageAsync(chatId, text, replyMarkup:RemoveAdminInlinekeyboard.GetRemoveAdminInlineKeyboard());
                }
            }
        }
    }
}
