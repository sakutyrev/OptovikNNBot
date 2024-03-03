using OptovikNNBot.Database;
using OptovikNNBot.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace OptovikNNBot.Commands.Message_Commands
{
    internal class UploadFileCommand : ICommand
    {
        public string Name => "/uploadfile";

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
                        var newUserState = new UserState { TgUserId = chatId, State = UserTgState.WaitingForExcelFile.ToString() };
                        userStates.Create(newUserState);
                    }
                    else //Если запись есть, то обновляем на новый статус
                    {
                        var currentUser = userStates.GetByTgId(chatId);
                        currentUser.State = UserTgState.WaitingForExcelFile.ToString();
                        userStates.Update(currentUser);
                    }
                    var text = "Пришлите файл для обновления остатков в формате .xls или отмените операцию с помощью /cancel\n\n";
                    await botClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Html);
                }
            }
        }

        //Метод выгружает данные из скачанного файла выгрузки в БД, а затем удаляет этот файл
        public async Task RefreshStocks(ITelegramBotClient botClient, Update update, string filename)
        {
            // путь к файлу
            var path = System.IO.Path.GetFullPath(filename);

            // создаем поток для чтения данных из файла
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                // создаем объект класса HSSFWorkbook, представляющий Excel-файл
                HSSFWorkbook workbook = new HSSFWorkbook(fileStream);

                // получаем первый лист из файла
                ISheet sheet = workbook.GetSheetAt(0);

                // получаем количество строк в листе
                int rowCount = sheet.LastRowNum + 1;

                var context = new TgBotAppContext();

                using(context)
                {
                    for (int i = 14; i < rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        if (row != null)
                        {
                            // считываем данные из ячейки с индексом 1 (2 колонка - название товара)
                            ICell cell2 = row.GetCell(1);
                            string name = cell2.StringCellValue.Trim();

                            // считываем данные из ячейки с индексом 2 (3 колонка - остаток)
                            ICell cell3 = row.GetCell(2);
                            double stockValue = cell3.NumericCellValue;

                            // считываем данные из ячейки с индексом 4 (5 колонка - артикул)
                            ICell cell5 = row.GetCell(4);
                            string? article = cell5.StringCellValue.Trim();

                            // считываем данные из ячейки с индексом 5 (6 колонка - бренд)
                            ICell cell6 = row.GetCell(5);
                            string? brand = cell6.StringCellValue.Trim();
                            
                            // считываем данные из ячейки с индексом 14 (15 колонка - цена)
                            ICell cell15 = row.GetCell(14);
                            double price = cell15.NumericCellValue;
                            if (price == 0) continue;

                            var remaisRepo = new Repository<Remains>(context);
                            Remains remains = context.Remains.FirstOrDefault(yr => yr.Name == name)!;

                            if (remains == null)
                            {
                                remains = new Remains
                                {
                                    Name = name,
                                    PositionRemains = Convert.ToInt32(stockValue),
                                    Article = article,
                                    Brand = brand,
                                    Price = price
                                };

                                remaisRepo.Create(remains);
                            }

                            remains.PositionRemains = Convert.ToInt32(stockValue);
                            remaisRepo.Update(remains);
                        }
                    }
                }
            }
            System.IO.File.Delete(path); //Удаление файла

            var chatId = update.Message!.Chat.Id;
            var text = "Загрузка данных прошла успешно!";
            await botClient.SendTextMessageAsync(chatId, text);
        }
    }
}
