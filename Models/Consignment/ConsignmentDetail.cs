using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models.Consignment
{
    public class ConsignmentDetail
    {
        public int CId;
        public string DepartureCity;
        public string DestinationCity;
        public DateTime CDate;
        public DateTime CTime;
        public DateTime CreatedDateTime;
        public DateTime LastLoggedInDateTime;
        public string ApproximateWeight;
        public string ConsignmentDescription;
        public bool Loading;
        public bool UnLoading;
        public string ApproximateBudget;
        public int UserID;
        public int BidID;
        public string Status; //Pending and Completed
    }
}