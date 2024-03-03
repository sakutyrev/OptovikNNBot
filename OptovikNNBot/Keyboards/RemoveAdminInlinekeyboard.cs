using OptovikNNBot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace OptovikNNBot.Keyboards
{
    internal class RemoveAdminInlinekeyboard
    {
        public static InlineKeyboardMarkup GetRemoveAdminInlineKeyboard()
        {

            var context = new TgBotAppContext();
            var adminsRepo = new Repository<Admin>(context);
            var admins = adminsRepo.GetAll().ToList();

            var buttons = new List<InlineKeyboardButton[]>();
            foreach (var admin in admins)
            {
                buttons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData(text: "id: " + admin.TgUserId.ToString() + "("+ admin.TgUserName+")", callbackData: $"remove_admin_{admin.TgUserId}")
                });
            }

            var rmAdminKeyboard = new InlineKeyboardMarkup(buttons.ToArray());
            return rmAdminKeyboard;
        }
    }
}
