using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCIdentityConfirm.Models
{
    public class ocenaCompanyModel
    {
        public int companyID { get; set; }
        public string companyName { get; set; }
        public string adres { get; set; }
        public string userId { get; set; }
        public int? przelozonyId { get; set; }
        public ocenaCompany  ocena { get; set; }
    }
}