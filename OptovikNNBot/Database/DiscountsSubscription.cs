using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptovikNNBot.Database
{
    internal class DiscountsSubscription
    {
        public int Id { get; set; } 
        public long Tg_user_id { get; set; }
        public User? User { get; set; }
        public bool IsActive { get; set; }

    }
}
