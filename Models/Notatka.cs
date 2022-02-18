using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wrzesienprojekt.Models
{
    public class Notatka
    {
        public int NotatkaID { get; set; }
        public string NotatkaNazwa { get; set; }
        public string Przedmiot { get; set; }
        public DateTime DataDodania { get; set; }
        public string NazwaZdjecia { get; set; }


    }
}
