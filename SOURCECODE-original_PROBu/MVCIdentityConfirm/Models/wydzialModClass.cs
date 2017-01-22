using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MVCIdentityConfirm.Models
{
    public class wydzialModClass
    {
        public string wydzialName { get; set; }
        public string przelozony { get; set; }
        public string stanowisko { get; set; }
        public int przelozonyID { get; set; }
       
    }
}