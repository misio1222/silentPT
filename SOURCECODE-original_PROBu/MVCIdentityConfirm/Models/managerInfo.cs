using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCIdentityConfirm.Models
{
    public class managerInfo
    {
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public string wydzial { get; set; }
        public string firma { get; set; }
        public string stanowisko { get; set; }
        public int id { get; set; }
        public ocenaPrzelozony oceny {get; set;}
        public int companyId { get; set; }
    }
}