using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Web;

namespace MVCIdentityConfirm.Models
{
    public class CreateCompanyModel
    {
        [Required(ErrorMessage = "Proszę podać nazwę firmy!")]
        public string CompanyName { get; set;}
        [Required(ErrorMessage = "Proszę podać miejscowość!")]
        public string Miejscowosc { get; set; }
        [Required(ErrorMessage = "Proszę podać województwo!")]
        public string Wojewodztwo { get; set; }
        
        public HttpPostedFileBase Logo { get; set; }
        [Required(ErrorMessage = "Proszę podać ulicę!")]
        public string Ulica { get; set; }
        
        public Nullable<int> NIP { get; set; }
        public Nullable<int> Regon { get; set; }
    }
}