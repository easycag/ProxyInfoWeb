using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models.Bid
{
    public class BidDetail
    {
        public int BidID;
        public int CId;
        public double BidAmount;
        public int UserID;
        public string Viewed;
        public char Status; // A and R and D(Delete)
        public DateTime CreationDateTime;
        public DateTime BidUpdateDateTime;
    }
}