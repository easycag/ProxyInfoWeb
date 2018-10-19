using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models.Consignment
{
    public class ConsignmentDetail
    {
        public int CId;
        public int UserID;
        public string DepartureCity;
        public string DestinationCity;
        public DateTime CDate;
        public string Cmaterial;
        public string ApproximateWeight;
        public string ApproximateBudget;
        public bool Loading;
        public bool UnLoading;
        public int Cstatus;
        public DateTime CdeliveryDate;
        public int CupdateCount;
        public DateTime CcreateDate;
        public long CtotalAmount;
        public long CpendingAmount;
        
    }
}
