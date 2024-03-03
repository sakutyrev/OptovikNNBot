using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace OptovikNNBot.Commands
{
    internal class AddressCallback: ICommand
    {
        public string Name => "location";

        public bool Contains(string callbackData)
        {
            return callbackData.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null) 
            {
                var chatId = update.CallbackQuery.Message.Chat.Id;
                await botClient.SendVenueAsync(chatId: chatId, 56.290849576492825, 44.00454614211522, "ОптовикНН",
                            "Нижний Новгород ул. Заярская, дом 18", googlePlaceType: "store");
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
