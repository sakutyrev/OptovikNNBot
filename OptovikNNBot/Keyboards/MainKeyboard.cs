using OptovikNNBot.Commands;
using OptovikNNBot.Commands.Message_Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace OptovikNNBot.Keyboards
{
    public static class MainKeyboard
    {
        public static ReplyKeyboardMarkup GetMainKeyboard()
        {
            AboutCompanyCommand aboutCompany = new AboutCompanyCommand();
            HelpCommand helpCommand = new HelpCommand();
            DiscountsCommand discountsCommand = new DiscountsCommand();
            GetSubscriptionCommand getSubscriptionCommand = new GetSubscriptionCommand();
            CheckAvailabilityCommand checkAvailabilityCommand = new CheckAvailabilityCommand();
            
            ReplyKeyboardMarkup mainKeyboard = new(new[]
                {
                    new KeyboardButton[]{checkAvailabilityCommand.Name},
                    new KeyboardButton[]{aboutCompany.Name},
                    new KeyboardButton[]{helpCommand.Name},
                    new KeyboardButton[]{discountsCommand.Name, getSubscriptionCommand.Name}
                })
            { ResizeKeyboard = true };
            return mainKeyboard;
        }
    }
}
