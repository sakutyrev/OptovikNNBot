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
    internal class DiscountsCommand : ICommand
    {
        public string Name => "Текущие акции";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                var chatId = update.Message.Chat.Id;
                var text = "В данный момент скидки представлены на следующие товары: ";
                await botClient.SendTextMessageAsync(chatId, text);
                text = "В данный момент чат-бот работает в тестовом режиме, поэтому не может предоставлять актуальную информацию о скидках на товары. " +
                    "\n\nПодписывайтесь на новостную рассылку Оптовик-НН чтобы не упустить свежие акции и скидки!";
                await botClient.SendTextMessageAsync(chatId, text);
                var discOffer = "Вы хотели бы получать сообщения о появлении новых акций и скидок?";
                await botClient.SendTextMessageAsync(chatId, discOffer, replyMarkup: DiscOfferInlineKeyboard.GetDiscOfferInlineKeyboard());
            }
        }
    }
}
