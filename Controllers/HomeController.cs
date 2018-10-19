using DBInteraction;
using EncryptionLibrary;
using ProxyInfoWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProxyInfoWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            string strQuery = "";
            string strRetVal = "";
            SearchModel objSearchModel = new SearchModel();
            EncryptionLibrary.Encryption ec = new EncryptionLibrary.Encryption();
            DBOperation.sConnectionString = ec.Decrypt("37xtqIiIGXiQkQc622OS1AysWTE2NxFWWDrkzVjnSqYRs0iRV5OWMGtRX4lgGMKBje7bNqW7dVO4ndtD+yt8rANeuuGjGou5522ObtIx78K3zkSOVIF5gvWn96hy9OW0");

            #region City
            strQuery = "SELECT DISTINCT ipv4_city AS Cities FROM  IP_Info order by ipv4_city asc";
            strRetVal = DBOperation.ExecuteDBOperation(strQuery, DBOperation.OperationType.SELECT, null, ref dt);
            objSearchModel.sCityVal = new string[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                objSearchModel.sCity = "-----SELECT-----";
                objSearchModel.sCityVal = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objSearchModel.sCityVal[i] = Convert.ToString(dt.Rows[i][0]);
                }
            }

            #endregion

            #region State
            dt = new DataTable();
            objSearchModel.sState = "-----SELECT-----";
            objSearchModel.sStateVal = new string[0];
            //objSearchModel.sStateVal = new string[1];
            strQuery = "SELECT DISTINCT ipv4_state as States FROM  IP_Info  order by ipv4_state asc";
            strRetVal = DBOperation.ExecuteDBOperation(strQuery, DBOperation.OperationType.SELECT, null, ref dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                objSearchModel.sStateVal = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objSearchModel.sStateVal[i] = Convert.ToString(dt.Rows[i][0]);
                }
            }
            #endregion


            #region Country
            dt = new DataTable();
            objSearchModel.sCountry = "-----SELECT-----";
            objSearchModel.sCountryVal = new string[0];
            //objSearchModel.sCountryVal = new string[1];
            strQuery = "SELECT DISTINCT ipv4_country as Countries FROM  IP_Info  order by ipv4_country asc";
            strRetVal = DBOperation.ExecuteDBOperation(strQuery, DBOperation.OperationType.SELECT, null, ref dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                objSearchModel.sCountryVal = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objSearchModel.sCountryVal[i] = Convert.ToString(dt.Rows[i][0]);
                }
            }
            #endregion


            #region Speed
            dt = new DataTable();
            objSearchModel.sSpeed = "-----SELECT-----";
            objSearchModel.sSpeedVal = new string[0];
            strQuery = "SELECT DISTINCT IPv4_Speed AS Speed FROM  IP_Info order BY IPv4_Speed asc";
            strRetVal = DBOperation.ExecuteDBOperation(strQuery, DBOperation.OperationType.SELECT, null, ref dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                objSearchModel.sSpeedVal = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objSearchModel.sSpeedVal[i] = Convert.ToString(dt.Rows[i][0]);
                }
            }
            #endregion


            #region Status
            dt = new DataTable();
            objSearchModel.sStatusVal = new string[0];
            objSearchModel.sStatus = "-----SELECT-----";
            strQuery = "SELECT DISTINCT ipv4_category AS Status FROM  IP_Info order BY ipv4_category asc";
            strRetVal = DBOperation.ExecuteDBOperation(strQuery, DBOperation.OperationType.SELECT, null, ref dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                objSearchModel.sStatusVal = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objSearchModel.sStatusVal[i] = Convert.ToString(dt.Rows[i][0]);
                }
            }
            #endregion

            
            return View("Index",objSearchModel);
        }

        public ActionResult Login()
        {
            LoginInfo objLoginInfo = new LoginInfo();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginInfo objLoginInfo)
        {
            string sRetVal = "";
            try
            {
                sRetVal = ValidateUser(objLoginInfo.sUsername, objLoginInfo.sPassword);
                if (sRetVal != "SUCCESS")
                {
                    objLoginInfo.sErrorMsg = sRetVal;
                    return View("Login", objLoginInfo);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("Login", objLoginInfo);
            }
            
        }

        public string ValidateUser(string sUserName, string sPassword)
        {
            //string strQuery = "";
            string sRetVal = "";
            DataTable dt = null;
            try
            {
                DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[2];

                structDBOperation[0].sParamName = "@vQueryType";
                structDBOperation[0].sParamType = SqlDbType.VarChar;
                structDBOperation[0].sParamValue = "GETUSERDETAILS";

                structDBOperation[1].sParamName = "@vUserName";
                structDBOperation[1].sParamType = SqlDbType.VarChar;
                structDBOperation[1].sParamValue = sUserName;

                sRetVal = DBOperation.ExecuteDBOperation("ProcGetUserInfo", DBOperation.OperationType.STOREDPROC, structDBOperation, ref dt);

                if (sRetVal == "SUCCESS")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["Password"]) != sPassword)
                            return "Invalid Username or password.";
                        return "SUCCESS";
                    }
                    else
                        return "Invalid Username or password.";
                }

                return sRetVal;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public ActionResult FetchDetail(SearchModel objSearchModel)
        {
            return View("Index", objSearchModel);
        }
    }
}
