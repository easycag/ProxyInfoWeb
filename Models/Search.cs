using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models
{
    public class SearchModel
    {
        public string sProxy = "";
        public string sStatus = "";
        public string sSpeed = "";
        public string sCity = "";
        public string sState = "";
        public string sCountry = "";
        public string[] sStateVal;
        public string[] sCountryVal;
        public string[] sStatusVal;
        public string[] sSpeedVal;
        public string[] sCityVal;
    }
}