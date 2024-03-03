using OptovikNNBot.Commands.Message_Commands;
using OptovikNNBot.Database;
using OptovikNNBot.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OptovikNNBot.Commands
{
    public class CommandHandler
    {
        private List<ICommand> _commands;
        private static List<Update> mailingList = new List<Update>();

        public CommandHandler()
        {
            _commands = new List<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }

        public void RemoveCommand(ICommand command)
        {
            _commands.Remove(command);
        }
        
        public async Task HandleMessage(ITelegramBotClient botClient, Update update)
        {
            var context = new TgBotAppContext();
            using (context)
            {
                var userStates = new Repository<UserState>(context);
                var currentUser = userStates.GetByTgId(update.Message!.Chat.Id);
                if (currentUser != null && currentUser.State == UserTgState.WaitingForNewAdminId.ToString())
                {
                    var newAdmin = update.Message.Text;
                    if (!newAdmin!.All(char.IsDigit))
                    {
                        var chatId = update.Message.Chat.Id;
                        var error = "Введенный id содержит буквы или иные символы, id может содержать только цифры";
                        await botClient.SendTextMessageAsync(chatId, error);
                        return;
                    }
                    NewAdminCommand newAdminCommand = new NewAdminCommand();
                    await newAdminCommand.CreateNewAdmin(botClient, context, currentUser, newAdmin!);

                    currentUser.State = UserTgState.None.ToString();
                    userStates.Update(currentUser);
                    return;
                }
                else if (currentUser != null && currentUser.State == UserTgState.ChoosingGoods.ToString())
                {
                    var searchingGood = update.Message.Text;
                    CheckAvailabilityCommand checkAvailabilityCommand = new CheckAvailabilityCommand();
                    await checkAvailabilityCommand.FindGoods(botClient, update, searchingGood!);

                    currentUser.State = UserTgState.None.ToString();
                    userStates.Update(currentUser);
                    return;
                }
                else if (currentUser != null && currentUser.State == UserTgState.WaitingForMailing.ToString())
                {
                    var chatId = currentUser.TgUserId;

                    if (update.Message.Text == "/newmailing")
                    {
                        var discSubs = new Repository<DiscountsSubscription>(context);
                        var allSubs = discSubs.GetAll().ToList();
                        if(mailingList.Count == 0)
                        {
                            var warning = "⚠️В рассылке не содержится ни одного сообщения. Добавьте какое-то сообщение прежде чем делать отправку!";
                            await botClient.SendTextMessageAsync(chatId, warning);
                            return;
                        }
                        foreach (var discSub in allSubs)
                        {
                            if (!discSub.IsActive) continue;
                            var userId = discSub.Tg_user_id;
                            foreach (var message in mailingList)
                            {
                                // await botClient.SendTextMessageAsync(userId, message.Message!.Text!);
                                await botClient.CopyMessageAsync(userId, chatId, message.Message!.MessageId);
                            }
                        }
                        var exclamation = "⚠️Рассылка успешно произведена!";
                        await botClient.SendTextMessageAsync(chatId, exclamation);
                        mailingList.Clear();

                        currentUser.State = UserTgState.None.ToString();
                        userStates.Update(currentUser);
                        return;
                    }
                    if (update.Message.Text == "/cancel")
                    {
                        var exclamation = "⚠️Отправка рассылки отменена!";
                        await botClient.SendTextMessageAsync(chatId, exclamation);
                        mailingList.Clear();

                        currentUser.State = UserTgState.None.ToString();
                        userStates.Update(currentUser);
                        return;
                    }
                        mailingList.Add(update);
                    var text = "Сообщение добавлено в рассылку!\n" +
                        "Вы можете добавить еще сообщения в рассылку или начать ее с помощью /newmailing. ";
                    await botClient.SendTextMessageAsync(chatId, text);
                }
            }
            try
            {
                foreach (var command in _commands)
                {
                    if (update.Message?.Text != null && command.Contains(update.Message.Text))
                    {
                        await command.Execute(botClient, update);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }  
        }
        
        public async Task HandleCallbackQuery(ITelegramBotClient botClient, Update update)
        {
            try
            {
                foreach (var command in _commands)
                {
                    if (update.CallbackQuery?.Data != null && command.Contains(update.CallbackQuery.Data))
                    {
                        await command.Execute(botClient, update);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        public async Task HandleMessageWithFile(ITelegramBotClient botClient, Update update)
        {
            try
            {
                var context = new TgBotAppContext();
                using (context)
                {
                    var userStates = new Repository<UserState>(context);
                    var currentUser = userStates.GetByTgId(update.Message!.Chat.Id);
                    var chatId = update.Message!.Chat.Id;
                    if (currentUser != null && currentUser.State == UserTgState.WaitingForExcelFile.ToString())
                    {
                        var sendedFile = update.Message.Document;

                        var fileId = sendedFile!.FileId;
                        var filename = sendedFile!.FileName;
                        var fileParts = filename!.Split('.');
                        var extension = fileParts[1];
                        
                        if(extension != "xls") //Проверка на соответствие формата
                        {
                            var error = "Вы отправили файл не в формате xls, загрузите новый файл или отмените загрузку с помощью /cancel";
                            await botClient.SendTextMessageAsync(chatId, error);
                            return;
                        }

                        var file = await botClient.GetFileAsync(fileId);
                        var filePath = file.FilePath;

                        //Загрузка файла с сервера Telegram, сохранение в папке проекта
                        using (var fileStream = new FileStream(filename, FileMode.Create))
                        {
                            await botClient.DownloadFileAsync(file.FilePath!, fileStream);
                        }
                        //Уведомление о том, что файл получен и выгрузка данных начата
                        var text = "Файл получен, сейчас начнется выгрузка данных, ожидайте сообщения о результате операции";
                        await botClient.SendTextMessageAsync(chatId, text);

                        UploadFileCommand uploadFileCommand = new UploadFileCommand();
                        await uploadFileCommand.RefreshStocks(botClient, update, filename);
                        
                        currentUser.State = UserTgState.None.ToString();
                        userStates.Update(currentUser);
                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
