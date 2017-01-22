using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIdentityConfirm.Models
{
    public class CompanyViewModel
    {

        public IEnumerable<SelectListItem> Company { get; set; }
        public IEnumerable<string> SelectedCompany { get; set; }
        public string search { get; set; }

        public CreateCompanyModel createModel { get; set; }
        
        public int? powrotWczesniej { get; set; }
        public int? powrotcompany { get; set; }

        
    }
}

