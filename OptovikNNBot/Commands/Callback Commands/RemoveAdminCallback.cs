using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using OptovikNNBot.Database;

namespace OptovikNNBot.Commands
{
    internal class RemoveAdminCallback: ICommand
    {
        public string Name => "remove_admin_";

        public bool Contains(string callbackData)
        {
            return callbackData.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null) 
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                var messageId = update.CallbackQuery.Message.MessageId;

                var dataParts = update.CallbackQuery.Data!.Split('_');
                var adminId = Convert.ToInt64(dataParts[2]); //Получаем id администратора из Callback.Data, которого надо удалить

                var context = new TgBotAppContext();
                
                using (context)
                {
                    var adminsRepo = new Repository<Admin>(context);
                    if(adminsRepo.GetAll().Count() == 1) 
                    {
                        var warning = "⛔️Нельзя удалить последнего администратора!";
                        await botClient.EditMessageTextAsync(chatId, messageId, warning, replyMarkup: null);
                        return;
                    }
                    var admin = adminsRepo.GetByTgId(adminId);
                    adminsRepo.Delete(admin);
                    
                    var text = $"⚠️Администратор с id: {adminId} удален";
                    await botClient.EditMessageTextAsync(chatId, messageId, text, replyMarkup: null);
                }

                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
