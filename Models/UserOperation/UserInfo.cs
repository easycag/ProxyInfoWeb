using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models.UserOperation
{
    public class UserInfo
    {
        public int iUserId { get; set; }
        public long lMobileNo { get; set; }
        public string sUser_Name { get; set; }
        public string sEmailID { get; set; }
        public string sUser_Address { get; set; }
        public int sUser_Zip { get; set; }
        public string sUser_Gst_Pan { get; set; }
        public string sCompany_Name { get; set; }
    }
        
}