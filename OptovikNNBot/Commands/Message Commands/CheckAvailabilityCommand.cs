 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Runtime.CompilerServices;
using Telegram.Bot.Types.Enums;
using System.IO;
using OptovikNNBot.Keyboards;
using OptovikNNBot.Database;
using OptovikNNBot.Enums;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class CheckAvailabilityCommand:ICommand
    {
        public string Name => "Наличие товаров";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.Message?.Chat.Id != null)
            {
                string text = "Пожалуйста, введите название интересующего вас товара, мы предложим вам все подходящие товары";
                
                var chatId = update.Message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId, text);

                var context = new TgBotAppContext();
                using (context)
                {
                    var userStates = new Repository<UserState>(context);

                    if (userStates.GetByTgId(chatId) == null) //Если записи о статусе действия еще нет, то создаем
                    {
                        var newUserState = new UserState { TgUserId = chatId, State = UserTgState.ChoosingGoods.ToString() };
                        userStates.Create(newUserState);
                    }
                    else //Если запись есть, то обновляем на новый статус
                    {
                        var currentUser = userStates.GetByTgId(chatId);
                        currentUser.State = UserTgState.ChoosingGoods.ToString();
                        userStates.Update(currentUser);
                    }
                }
            }
        }
        public async Task FindGoods(ITelegramBotClient botClient, Update update, string searchingGood)
        {
            var context = new TgBotAppContext();
            var chatId = update.Message!.Chat.Id;
            using (context)
            {
                var goodsList = context.Remains
                        .Where(yr => yr.Name!.ToLower().Contains(searchingGood.ToLower()))
                        .ToList();
                if (searchingGood.Length <= 3 ||goodsList == null || goodsList.Count == 0) 
                {
                    var error = "Товаров, удовлетворяющих вашему запросу не было найдено\n" +
                        "Проверьте введенный текст и повторите поиск заново";
                    await botClient.SendTextMessageAsync(chatId, error);
                    return;
                }
                var text = "Товары, которые мы нашли по вашему запросу:\n" +
                    "➖➖➖➖➖➖➖➖➖➖➖➖➖\n";

                int count = 0;
                foreach (var good in goodsList.Take(15))
                {
                    count++;
                    text += $"<b>{count})</b> {good.Name}\n" +
                        $"📎<b>В наличии:</b> {good.PositionRemains} шт.\n" +
                        $"💲<b>Оптовая цена:</b> {good.Price} рублей\n" +
                        $"➖➖➖➖➖➖➖➖➖➖➖➖➖\n";
                }
                await botClient.SendTextMessageAsync(chatId, text, ParseMode.Html);
            }
        }
    }
}
