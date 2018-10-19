using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models.UserOperation
{
    public class UserLogin
    {
        public int iUserId { get; set; }
        public long lMobileNo { get; set; }
        public Boolean bUserVerification { get; set; }
        public int iUserPreference { get; set; }
        public int iUserOTP { get; set; }
        public DateTime dUserLastLogin { get; set; }
    }
}