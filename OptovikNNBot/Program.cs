using OptovikNNBot.Commands;
using OptovikNNBot.Commands.Callback_Commands;
using OptovikNNBot.Commands.Message_Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

internal class Program
{
    private static void Main(string[] args)
    {
        //Настройка подключения бота
        var token = Environment.GetEnvironmentVariable("tgBotToken");
        var tgBotClient = new TelegramBotClient(token: token!);

        using CancellationTokenSource cts = new();

        ReceiverOptions receiverOptions = new () 
        {
            AllowedUpdates = Array.Empty<UpdateType>() 
        };

        tgBotClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler:HandlePollingErrorAsync,
            receiverOptions:receiverOptions,
            cancellationToken: cts.Token
            );

        User me = tgBotClient.GetMeAsync().Result;
        //Вывод информации о начале работы бота в консоль
        Console.WriteLine($"Бот @{me.Username} начал работу");
        
        Console.WriteLine("Нажмите Esc для завершения работы бота");
        
        //Проверка ввода клавиши Esc
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); 
            if (keyInfo.Key != ConsoleKey.Escape) 
            {
                Console.WriteLine("Нажата клавиша: " + keyInfo.Key + ". Для завершения работы нажмите клавишу Escape");
                continue;
            }
            Console.WriteLine("Нажата клавиша Escape, завершение работы...");
            break;
        }

        // Отправка запроса на остановку бота после завершения его работы
        cts.Cancel();
        
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            #region handlers declaration
            CommandHandler commandHandler = new CommandHandler();
            
            //Объявление текстовых команд
            commandHandler.AddCommand(new StartCommand());
            commandHandler.AddCommand(new AboutCompanyCommand());
            commandHandler.AddCommand(new HelpCommand());
            commandHandler.AddCommand(new FAQCommand());
            commandHandler.AddCommand(new GuaranteeInfoCommand());
            commandHandler.AddCommand(new OrderInfoCommand());
            commandHandler.AddCommand(new PaymentInfoCommand());
            commandHandler.AddCommand(new DeliveryInfoCommand());
            commandHandler.AddCommand(new BackCommand());
            commandHandler.AddCommand(new DiscountsCommand());
            commandHandler.AddCommand(new GetSubscriptionCommand());
            commandHandler.AddCommand(new GetStatsCommand());
            commandHandler.AddCommand(new NewAdminCommand());
            commandHandler.AddCommand(new RemoveAdminCommand());
            commandHandler.AddCommand(new UploadFileCommand());
            commandHandler.AddCommand(new CancelCommand());
            commandHandler.AddCommand(new CheckAvailabilityCommand());
            commandHandler.AddCommand(new MailingCommand());

            //Объявление callback команд
            commandHandler.AddCommand(new AddressCallback());
            commandHandler.AddCommand(new ReviewsCallback());
            commandHandler.AddCommand(new WorkingHoursCallback());
            commandHandler.AddCommand(new FaqQuestion1Callback());
            commandHandler.AddCommand(new FaqQuestion2Callback());
            commandHandler.AddCommand(new FaqQuestion3Callback());
            commandHandler.AddCommand(new FaqQuestion4Callback());
            commandHandler.AddCommand(new FaqQuestion5Callback());
            commandHandler.AddCommand(new FaqQuestion6Callback());
            commandHandler.AddCommand(new DiscOfferYes());
            commandHandler.AddCommand(new DiscOfferNo());
            commandHandler.AddCommand(new DiscCancelYesCallback());
            commandHandler.AddCommand(new DiscCancelNoCallback());
            commandHandler.AddCommand(new RemoveAdminCallback());

            #endregion

            if (update.Type == UpdateType.Message)
            {
                await commandHandler.HandleMessage(botClient, update);
            }
            if(update.Type == UpdateType.Message && update.Message!.Type == MessageType.Document)
            {
                await commandHandler.HandleMessageWithFile(botClient, update);
            }
            if(update.Type == UpdateType.CallbackQuery) 
            {
                await commandHandler.HandleCallbackQuery(botClient, update);
            }
            return;
        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}