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
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[12];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "OFFERCONSIGNMENT";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCId";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objConsignmentDetail.CId);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDepartureCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DepartureCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDestinationCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DestinationCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCDate";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.CDate.ToString("dd-MM-yyyy");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCTime";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.CTime.ToString("hh:mm:ss");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vApproximateWeight";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ApproximateWeight;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vConsignmentDescription";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ConsignmentDescription;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vLoading";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.Loading ? "1" : "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vUnLoading";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.UnLoading ? "1" : "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vApproximateBudget";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ApproximateBudget;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vUserID";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.UserID.ToString();
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
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[12];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "UPDATECONSIGNMENT";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCId";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString( objConsignmentDetail.CId);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDepartureCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DepartureCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDestinationCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DestinationCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCDate";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.CDate.ToString("dd-MM-yyyy");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCTime";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.CTime.ToString("hh:mm:ss");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vApproximateWeight";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ApproximateWeight;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vConsignmentDescription";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ConsignmentDescription;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vLoading";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.Loading ? "1" : "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vUnLoading";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.UnLoading ? "1" : "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vApproximateBudget";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ApproximateBudget;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vUserID";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.UserID.ToString();
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

        [HttpPost]
        public HttpResponseMessage GetConsignmentDetail(ConsignmentDetail objConsignmentDetail)
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
            objParam[iParamCount].sParamValue = "CONSIGNMENTDETAIL";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDepartureCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DepartureCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vDestinationCity";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.DestinationCity;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCDate";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.CDate.ToString("dd-MM-yyyy");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCTime";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.CTime.ToString("hh:mm:ss");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vApproximateWeight";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ApproximateWeight;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vConsignmentDescription";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ConsignmentDescription;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vLoading";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.Loading ? "1" : "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vUnLoading";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.UnLoading ? "1" : "0";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vApproximateBudget";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.ApproximateBudget;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vUserID";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objConsignmentDetail.UserID.ToString();
            iParamCount++;

            sQuery = "ProcGetConsignment";
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

        public HttpResponseMessage CompleteConsignment(int CId)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[1];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vCId";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(CId);
            iParamCount++;

            sQuery = "ProcConsignmentComplete";
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
