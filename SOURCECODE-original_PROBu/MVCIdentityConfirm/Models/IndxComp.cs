using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIdentityConfirm.Models
{
    public class IndxComp
    {
        public Company cmpIn { get; set; }
        public string img { get; set; }
        public List<SelectListItem> wydzial { get; set; }


    }
}