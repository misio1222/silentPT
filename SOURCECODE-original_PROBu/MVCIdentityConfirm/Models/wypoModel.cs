using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCIdentityConfirm.Models
{
    public class wypoModel
    {
        public int id { get; set; }
        public string userId { get; set; }
        public int wydzialId { get; set; }
        public string content { get; set; }
        public int companyId { get; set; }
        public bool logOrName { get; set; }
        public string userName { get; set; }
        public string mojeId { get; set; }
        public string DateTi { get; set; }
        public int like { get; set; }
        public int notLike { get; set; }
        public List<forumImages> Image { get; set; }
     }
}