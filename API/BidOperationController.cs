using DBInteraction;
using ProxyInfoWeb.Models;
using ProxyInfoWeb.Models.Bid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProxyInfoWeb.API
{
    public class BidOperationController : ApiController
    {
        private const string ProcBidOperation_Bid = "ProcBidOperation_Delete";
        string sRetVal = "";
        EncryptionLibrary.Encryption ec = new EncryptionLibrary.Encryption();

        //***************************************** Place Bid **************************************************
        //      PlaceBid     :   Place the bid for a specific consignment                                       *
        //      GetBidDetail :   Get the bid detail on the basis of different filter Crieteria                  *
        //      UpdateBid    :   Update Bid Details. Only Bid amount can modify                                 *
        //      DeleteBid    :   Delete bid on the basis of bid ID                                              *
        //
        //*******************************************************************************************************

            /// Function Name : Place Bid
            /// Parameter Details 
            /// 1. BidDetail DTO
            /// 
            /// Functionality
            /// This function accept Bid details DTO and store into DB
            /// Value of Load ID, User ID, Amount is mandatory
            
        public HttpResponseMessage PlaceBid(BidDetail objBidDetail)
        {           
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[9];
            int iParamCount = 0;
            #endregion


            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "PLACEBID";
            iParamCount++;

            objParam[iParamCount].sParamName = "@bid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@cid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue =Convert.ToString( objBidDetail.CId);
            iParamCount++;

            objParam[iParamCount].sParamName = "@bidamount";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue =Convert.ToString( objBidDetail.BidAmount);
            iParamCount++;

            objParam[iParamCount].sParamName = "@userid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue =Convert.ToString(objBidDetail.UserID );
            iParamCount++;

            objParam[iParamCount].sParamName = "@viewed";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objBidDetail.Viewed;
            iParamCount++;

            objParam[iParamCount].sParamName = "@status";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue =Convert.ToString( objBidDetail.Status);
            iParamCount++;

            objParam[iParamCount].sParamName = "@creationdatetime";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.CreationDateTime.ToString("dd-MM-yyyy") );
            iParamCount++;

            objParam[iParamCount].sParamName = "@BidUpdateDateTime";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.BidUpdateDateTime.ToString("dd-MM-yyyy")); ;
            iParamCount++;

            sQuery = "ProcBidOperation";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Bid Placed Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }


        /// Function Name : Get Bid Details
        /// Parameter Details 
        /// 1. BidDetail DTO
        /// 
        /// Functionality
        /// This function accept Bid details DTO and fetch the details from DB on below filters
        /// Bid ID
        /// Load ID
        /// User ID
        /// Amount 
        /// View Status
        
        [HttpPost]
        public HttpResponseMessage GetBidDetail(BidDetail objBidDetail)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[2];
            int iParamCount = 0;
            string sFilter = "";
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            #region Prepare Query Filter
            if (objBidDetail.BidID > 0)
            {
                sFilter += " AND bid_id=" + objBidDetail.BidID;
            }
            if (objBidDetail.CId > 0)
            {
                sFilter += " AND load_id=" + objBidDetail.CId;
            }
            if (objBidDetail.BidAmount > 0)
            {
                sFilter += " AND Bid_Amount=" + objBidDetail.BidAmount;
            }
            if (objBidDetail.UserID > 0)
            {
                sFilter += " AND User_ID=" + objBidDetail.UserID;
            }
            if (!string.IsNullOrEmpty( objBidDetail.Viewed ))
            {
                sFilter += " AND Viewed=" + objBidDetail.Viewed;
            }
            
            if (sFilter.Length > 4)
            {
                sFilter="Where " +  sFilter.Substring(4);
            }
            #endregion

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "CONSIGNMENTDETAIL";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vFilterClause";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = sFilter;
            iParamCount++;


            sQuery = "ProcGetBidDetail";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

        /// Function Name : Accept Bid Details
        /// Parameter Details 
        /// 1. BidDetail DTO
        /// 
        /// Functionality
        /// This function accept Bid details DTO and Modify the Bid and Load details based on the Bid ID
        /// 

        public HttpResponseMessage AcceptBid(BidDetail objBidDetail)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[2];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "ACCEPTBID";
            iParamCount++;

            objParam[iParamCount].sParamName = "@bid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.BidID);
            iParamCount++;

            sQuery = ProcBidOperation_Bid;
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Bid Accepted Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

        /// Function Name : Reject Bid Details
        /// Parameter Details 
        /// 1. BidDetail DTO
        /// 
        /// Functionality
        /// This function accept Bid details DTO and mark the bid as rejected based on the Bid ID
        /// 
        public HttpResponseMessage RejectBid(BidDetail objBidDetail)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[2];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "REJECTBID";
            iParamCount++;

            objParam[iParamCount].sParamName = "@bid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue =Convert.ToString( objBidDetail.BidID);
            iParamCount++;

           
            sQuery = ProcBidOperation_Bid;
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Bid Rejected Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

        /// Function Name : Update Bid 
        /// Parameter Details 
        /// 1. BidDetail DTO
        /// 
        /// Functionality
        /// This function accept Bid details DTO and update the amount detail into DB based on Bid ID
        
        public HttpResponseMessage UpdateBid(BidDetail objBidDetail)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[9];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "UPDATEBID";
            iParamCount++;

            objParam[iParamCount].sParamName = "@bid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.BidID);
            iParamCount++;

            objParam[iParamCount].sParamName = "@cid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.CId);
            iParamCount++;

            objParam[iParamCount].sParamName = "@bidamount";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.BidAmount);
            iParamCount++;

            objParam[iParamCount].sParamName = "@userid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.UserID);
            iParamCount++;

            objParam[iParamCount].sParamName = "@viewed";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objBidDetail.Viewed;
            iParamCount++;

            objParam[iParamCount].sParamName = "@status";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.Status);
            iParamCount++;

            objParam[iParamCount].sParamName = "@creationdatetime";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.CreationDateTime.ToString("dd-MM-yyyy"));
            iParamCount++;

            objParam[iParamCount].sParamName = "@BidUpdateDateTime";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.BidUpdateDateTime.ToString("dd-MM-yyyy")); ;
            iParamCount++;

            sQuery = "ProcBidOperation";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Bid Updated Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }


        /// Function Name : Delete Bid 
        /// Parameter Details 
        /// 1. BidDetail DTO
        /// 
        /// Functionality
        /// This function accept Bid details DTO and delete the bid into DB based on Bid ID
        
        public HttpResponseMessage DeleteBid(BidDetail objBidDetail)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[2];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            if(GlobalValidation.ShouldNotNull(objBidDetail.BidID))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Bid ID validation failed" );
            }

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "DELETEBID";
            iParamCount++;

            objParam[iParamCount].sParamName = "@bid";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objBidDetail.BidID);
            iParamCount++;

           
            sQuery = ProcBidOperation_Bid;
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Bid Deleted Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

    }
}
