using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models
{
    public class LoginInfo
    {
        [Required]
        public string sUsername { get; set; }
        [Required]
        public string sPassword { get; set; }
        public string sErrorMsg { get; set; }
    }
}