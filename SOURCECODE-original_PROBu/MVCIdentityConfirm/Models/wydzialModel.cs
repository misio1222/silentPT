using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIdentityConfirm.Models
{
    public class wydzialModel
    {
        
        public Wydzial wydz {get; set; }
        [Required]
        public Przelozony przelozony { get; set; }
    }
}