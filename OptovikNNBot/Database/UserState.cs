using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptovikNNBot.Database
{
    internal class UserState
    {
        public long TgUserId { get; set; }
        public string? State { get; set; }
    }
}
