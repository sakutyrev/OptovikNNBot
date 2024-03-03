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
    public static class HelpKeyboard
    {
        public static ReplyKeyboardMarkup GetHelpKeyboard()
        {
            FAQCommand fAQCommand = new FAQCommand();
            OrderInfoCommand orderInfoCommand = new OrderInfoCommand();
            GuaranteeInfoCommand guaranteeInfoCommand = new GuaranteeInfoCommand();
            PaymentInfoCommand paymentInfoCommand = new PaymentInfoCommand();
            DeliveryInfoCommand deliveryInfoCommand = new DeliveryInfoCommand();
            BackCommand backCommand = new BackCommand();

            ReplyKeyboardMarkup helpKeyboard = new(new[]
            {
                    new KeyboardButton[]{fAQCommand.Name},
                    new KeyboardButton[]{orderInfoCommand.Name,guaranteeInfoCommand.Name},
                    new KeyboardButton[]{paymentInfoCommand.Name, deliveryInfoCommand.Name},
                    new KeyboardButton[]{backCommand.Name}
                })
            { ResizeKeyboard = true };
            return helpKeyboard;
        }
    }
        
}
