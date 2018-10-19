using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models
{
    public class GlobalClass
    {
        public static string sCompanyName = "EasyCag";
        public static string sConnectionString = "u81mG5FlAm/7IgFAcx0Nk+wzUmQ8eBRQJTyX6l3G7dN+4EWNm4rFfqanVkQaIGgOyHcW4nDdzQBBq9um3MNaIPA/o9EYnx8erF7ZUc+8cjY=";
        //Mail Fields
        public static string sGreeting = "Dear ";
        public static string sBody = "";
        public static string sSenderMail = "aashishgupta1@gmail.com";
        public static string sSenderPassword = "anuaashish123@";
        public static string sSignature = "";
        public static string sSubject = "OTP for " + sCompanyName + " Registeration";
        public static string sHost = "smtp.gmail.com";
        public static string sPort = "587";
    }
}