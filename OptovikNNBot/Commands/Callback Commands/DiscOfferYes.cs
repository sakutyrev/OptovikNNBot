using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OptovikNNBot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = OptovikNNBot.Database.User;

namespace OptovikNNBot.Commands.Callback_Commands
{
    internal class DiscOfferYes : ICommand
    {
        public string Name => "discounts_yes";

        public bool Contains(string command)
        {
            return command.Contains(Name);
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery?.Message?.Chat.Id != null)
            {
                //Создание подключения к БД
                var context = new TgBotAppContext(); 

                var chatId = update.CallbackQuery.Message.Chat.Id;
                var userId = chatId;
                var messageId = update.CallbackQuery.Message.MessageId;
                var username = update.CallbackQuery.Message.Chat.Username;

                //Обращение к БД. Внесение пользователя в список получающих рассылку
                using (context)
                {
                    var users = new Repository<User>(context);
                    var discSubs = new Repository<DiscountsSubscription>(context);
                    var currentUser = users.GetByTgId(userId);
                   
                    if (currentUser == null) //Если этот пользователь еще не регистрировал подписку на рассылку
                    {
                        //Создание нового пользователя в таблице Users
                        User newUser = new User { TgUserId = userId, Username = username };
                        users.Create(newUser);

                        //Создание новой подписки в таблице DiscSubscriptions
                        DiscountsSubscription newSubscription = new DiscountsSubscription { Tg_user_id = userId, IsActive = true };
                        discSubs.Create(newSubscription);

                        var text = "Подписка оформлена! Мы будем присылать вам информацию о новых скидках и акциях!";
                        await botClient.EditMessageTextAsync(chatId, messageId, text, replyMarkup: null);
                    }
                    else 
                    {
                        var subscription = discSubs.GetAll().FirstOrDefault(ds => ds.Tg_user_id == currentUser.TgUserId);
                        string text;
                        if (subscription!.IsActive == false)
                        {
                            subscription.IsActive = true;
                            discSubs.Update(subscription);
                            text = "Подписка снова подключена! Мы будем присылать вам информацию о новых скидках и акциях!";
                            await botClient.EditMessageTextAsync(chatId, messageId, text, replyMarkup: null);
                            return;
                        }
                        text = $"Похоже, что вы уже подписаны на рассылку";
                        await botClient.EditMessageTextAsync(chatId, messageId, text, replyMarkup: null);
                    }
                }
                //Ответ на посланный callback для того, чтоб telegram считал запрос выполненным
                var callbackQueryId = update.CallbackQuery.Id;
                await botClient.AnswerCallbackQueryAsync(callbackQueryId);
            }
        }
    }
}
