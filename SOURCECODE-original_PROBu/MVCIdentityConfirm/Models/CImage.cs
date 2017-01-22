using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCIdentityConfirm.Models
{
    public class CImage
    {
        public string imageFName { get; set; }
        public int imageSize { get; set; }
        public byte[] imageData { get; set; }
        public HttpPostedFile file { get; set; }
    }
}