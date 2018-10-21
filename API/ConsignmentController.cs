using DBInteraction;
using ProxyInfoWeb.Models;
using ProxyInfoWeb.Models.Consignment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProxyInfoWeb.API
{
    public class ConsignmentController : ApiController
    {
        string sRetVal = "";
        EncryptionLibrary.Encryption ec = new EncryptionLibrary.Encryption();
        public HttpResponseMessage OfferConsignment(ConsignmentDetail objConsignmentDetail)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[10];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "OFFERALOAD";
            iParamCount++;

            objParam[iParamCount].sParamName = "@iUserId";
            objParam[iParamCount].sParamType = SqlDbType.Int;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.UserID);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDepartureCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DepartureCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDestinationCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DestinationCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@dStartDate";
            objParam[iParamCount].sParamType = SqlDbType.DateTime;
            objParam[iParamCount].sParamValue = objConsignmentDetail.CStartDate.ToString("MM-dd-yyyy hh:mm:ss");
            iParamCount++;
            
            objParam[iParamCount].sParamName = "@vMaterial";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.Cmaterial;
            iParamCount++;

            objParam[iParamCount].sParamName = "@fApproximateWeight";
            objParam[iParamCount].sParamType = SqlDbType.Float;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.ApproximateWeight);
            iParamCount++;

            objParam[iParamCount].sParamName = "@dApproximateBudget";
            objParam[iParamCount].sParamType = SqlDbType.Float;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.ApproximateBudget);
            iParamCount++;

            objParam[iParamCount].sParamName = "@bLoading";
            objParam[iParamCount].sParamType = SqlDbType.Bit;
            objParam[iParamCount].sParamValue = objConsignmentDetail.Loading ? "1" : "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@bUnLoading";
            objParam[iParamCount].sParamType = SqlDbType.Bit;
            objParam[iParamCount].sParamValue = objConsignmentDetail.UnLoading ? "1" : "0";
            iParamCount++;
            
            sQuery = "ProcOfferConsignment";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Consignment Placed Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

        public HttpResponseMessage UpdateConsignment(ConsignmentDetail objConsignmentDetail)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[11];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "UPDATELOAD";
            iParamCount++;

            objParam[iParamCount].sParamName = "@iLoadId";
            objParam[iParamCount].sParamType = SqlDbType.Int;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.CId);
            iParamCount++;

            objParam[iParamCount].sParamName = "@iUserId";
            objParam[iParamCount].sParamType = SqlDbType.Int;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.UserID);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDepartureCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DepartureCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDestinationCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DestinationCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@dStartDate";
            objParam[iParamCount].sParamType = SqlDbType.DateTime;
            objParam[iParamCount].sParamValue = objConsignmentDetail.CStartDate.ToString("MM-dd-yyyy hh:mm:ss"); ;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vMaterial";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.Cmaterial;
            iParamCount++;

            objParam[iParamCount].sParamName = "@fApproximateWeight";
            objParam[iParamCount].sParamType = SqlDbType.Float;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.ApproximateWeight);
            iParamCount++;

            objParam[iParamCount].sParamName = "@dApproximateBudget";
            objParam[iParamCount].sParamType = SqlDbType.Float;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.ApproximateBudget);
            iParamCount++;

            objParam[iParamCount].sParamName = "@bLoading";
            objParam[iParamCount].sParamType = SqlDbType.Bit;
            objParam[iParamCount].sParamValue = objConsignmentDetail.Loading ? "1" : "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@bUnLoading";
            objParam[iParamCount].sParamType = SqlDbType.Bit;
            objParam[iParamCount].sParamValue = objConsignmentDetail.UnLoading ? "1" : "0";
            iParamCount++;

            sQuery = "ProcOfferConsignment";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                
                return Request.CreateResponse(HttpStatusCode.OK, "Consignment Updated Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetConsignmentDetail(ConsignmentDetail objConsignmentDetail)
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

            if (objConsignmentDetail.CId > 0)
            {
                sFilter += " AND load_id=" + objConsignmentDetail.CId;
            }

            if (objConsignmentDetail.UserID > 0)
            {
                sFilter += " AND user_id=" + objConsignmentDetail.UserID;
            }

            if (!string.IsNullOrEmpty(objConsignmentDetail.DepartureCity))
            {
                sFilter += " AND load_departure_city=" + "'" + objConsignmentDetail.DepartureCity + "'";
            }

            if (!string.IsNullOrEmpty(objConsignmentDetail.DestinationCity))
            {
                sFilter += " AND load_destination_city=" + "'" + objConsignmentDetail.DestinationCity + "'";
            }

            if (objConsignmentDetail.CStartDate > DateTime.MinValue)
            {
                sFilter += " AND load_departure_city>=" + "'" + objConsignmentDetail.CStartDate + "'";
            }

            if (sFilter.Length > 4)
            {
                sFilter = "Where " + sFilter.Substring(4);
            }
            else
            {
                sFilter = null;
            }

            #endregion
            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "GETCONSIGNMENTDETAIL";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vFilterClause";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = sFilter;
            iParamCount++;

            
            sQuery = "ProcGetOfferConsignment";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK,dt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteConsignment(ConsignmentDetail objConsignmentDetail)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[2];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "DELETELOAD";
            iParamCount++;

            objParam[iParamCount].sParamName = "@iLoadid";
            objParam[iParamCount].sParamType = SqlDbType.Int;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.CId);
            iParamCount++;

            
            sQuery = "ProcDeleteOfferConsignment";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Consignment Deleted Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }


        public HttpResponseMessage CompleteConsignment(ConsignmentDetail objConsignmentDetail)
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
            objParam[iParamCount].sParamValue = "UPDATELOADSTATUS";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vFilterClause";
            objParam[iParamCount].sParamType = SqlDbType.Int;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.CId);
            iParamCount++;

            
            sQuery = "ProcGetOfferConsignment";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {

                return Request.CreateResponse(HttpStatusCode.OK, "Consignment Completed Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

    }
}
