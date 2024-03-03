using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OptovikNNBot.Database;
using OptovikNNBot.Keyboards;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class GetStatsCommand : ICommand
    {
        public string Name => "/getstats";

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
                            isAdmin=true;
                    }
                    if (!isAdmin) return;
                    var discSubs = new Repository<DiscountsSubscription>(context);
                    var usersCount = discSubs.GetAll().Count();
                    var activeUsers = discSubs.GetAll().Count(ds => ds.IsActive == true);
                    var text = "Статистика по пользователям, подписанным на рассылку:" +
                        $"\n\nВсего подписок: <b>{usersCount}</b>" +
                        $"\nАктивных подписок: <b>{activeUsers}</b>";
                    await botClient.SendTextMessageAsync(chatId, text, replyMarkup: MainKeyboard.GetMainKeyboard(), parseMode: ParseMode.Html);
                    
                }
                
            }
        }
    }
}
