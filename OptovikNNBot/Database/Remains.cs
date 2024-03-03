using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptovikNNBot.Database
{
    internal class Remains
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Article { get; set; }
        public string? Brand { get; set; }
        public double Price { get; set; }
        public int PositionRemains { get; set; }
    }
}
