using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models.UserOperation
{
    public class UserInfo
    {
        public int iUserId { get; set; }
        public string sEmailID { get; set; }
        public string sPassword { get; set; }
        public string sUserType { get; set; }
        public string sIsUserAlive { get; set; }
        public string sLevelID { get; set; }
        public long lMobileNo { get; set; }
        public string sCompany_Name { get; set; }
        public string sEmailID_status { get; set; }
        public string sMobileNo_Status { get; set; }
    }
}