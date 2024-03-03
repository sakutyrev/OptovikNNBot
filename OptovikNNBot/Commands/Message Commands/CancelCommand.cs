using OptovikNNBot.Database;
using OptovikNNBot.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OptovikNNBot.Commands
{
    public class CancelCommand: ICommand
    {
        public string Name => "/cancel";
        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                var context = new TgBotAppContext();

                bool isAdmin = false;
                var chatId = update.Message.Chat.Id;
                string? username = update.Message.Chat.Username;

                using (context)
                {
                    var admins = new Repository<Admin>(context);
                    var adminsList = admins.GetAll().ToList();
                    if (adminsList == null || adminsList.Count == 0) { throw new ArgumentNullException("Ни один админ не инициализирован в БД"); }

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
                    var currentUser = userStates.GetByTgId(chatId);

                    if (currentUser.State == UserTgState.None.ToString()) return;

                    currentUser.State = UserTgState.None.ToString();
                    userStates.Update(currentUser);

                    var text = "⚠️Операция отменена";
                    await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
                }
            }
        }

    }
}
