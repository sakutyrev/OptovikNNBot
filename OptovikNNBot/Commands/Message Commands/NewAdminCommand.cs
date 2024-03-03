using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OptovikNNBot.Database;
using OptovikNNBot.Enums;
using OptovikNNBot.Keyboards;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class NewAdminCommand : ICommand
    {
        public string Name => "/newadmin";

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
                string? username = update.Message.Chat.Username;

                using (context)
                {
                    var admins = new Repository<Admin>(context);
                    var adminsList = admins.GetAll().ToList();
                    if(adminsList == null || adminsList.Count == 0) { throw new ArgumentNullException("Ни один админ не инициализирован в БД"); }
                    
                    foreach (var admin in adminsList)
                    {
                        if (admin.TgUserId == chatId & admin.TgUserName == null) //Если функцию использует админ у которого нет значения имени пользователя в БД
                        {
                            isAdmin = true;
                            admin.TgUserName = username;
                            admins.Update(admin);
                        }
                        else if (admin.TgUserId == chatId)
                            isAdmin = true;
                    }
                    
                    if (!isAdmin) return;//Если пользователь, который ввел эту команду не является администратором
                    
                    var userStates = new Repository<UserState>(context);
                    
                    if (userStates.GetByTgId(chatId) == null) //Если записи о статусе действия еще нет, то создаем
                    {
                        var newUserState = new UserState { TgUserId = chatId, State = UserTgState.WaitingForNewAdminId.ToString() };
                        userStates.Create(newUserState);
                    }
                    else //Если запись есть, то обновляем на новый статус
                    {
                        var currentUser = userStates.GetByTgId(chatId);
                        currentUser.State = UserTgState.WaitingForNewAdminId.ToString();
                        userStates.Update(currentUser);
                    }
                    var text = "Введите телеграм id пользователя, кому вы хотите предоставить права администрирования\n\n" +
                        "⚠️Обязательно проверьте введенные данные, чтобы избежать предоставление доступа сторонним лицам!";
                    await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
                }
            }
        }
        public async Task CreateNewAdmin(ITelegramBotClient botClient, TgBotAppContext context, UserState currentUser, string newAdmin)
        {
            var admins = new Repository<Admin>(context);
            var chatId = currentUser.TgUserId;
            if(newAdmin == chatId.ToString())
            {
                var error = "⚠️Вы не можете добавить себя в администраторы!";
                await botClient.SendTextMessageAsync(chatId, error);
                return;
            }
            admins.Create(new Admin { TgUserId = Convert.ToInt64(newAdmin) });
            var text = "⚠️Новый администратор добавлен!";
            await botClient.SendTextMessageAsync(chatId, text);
        }
    }
}
