using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptovikNNBot.Database
{
    internal class User
    {
        public long TgUserId { get; set; }
        public string? Username { get; set; }

        public DiscountsSubscription? DiscountsSubscription { get; set; }
    }
}
