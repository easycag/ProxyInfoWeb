using DBInteraction;
using ProxyInfoWeb.Models;
using ProxyInfoWeb.Models.UserOperation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ProxyInfoWeb.API
{
    public class UserOperationController : ApiController
    {
        string sRetVal = "";
        EncryptionLibrary.Encryption ec = new EncryptionLibrary.Encryption();

        //***************************************** User Login **************************************************
        //      UserLogin   :   Login/Signup user with mobile number                                            *
        //      ValidateOTP :   Validate register mobile number with OTP                                        *
        //      UpdateUserPreference    :   Update user preference of user i.e. Company Owner/ Fleet Owner      *
        //                                                                                                      *
        //*******************************************************************************************************

        public HttpResponseMessage UserLogin(UserLogin objUserLogin)
        {
            try
            {
                #region declaration
                DataTable dt = new DataTable();
                string sQuery = "";
                Random r = new Random();
                DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
                DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[3];
                int iParamCount = 0;
                string sMobile_OTP = "";

                #endregion

                #region validation

                if (long.Equals(objUserLogin.lMobileNo,null) || objUserLogin.lMobileNo.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Mobile Number can't be blank");
                }

                
                #endregion

                #region DataBase Entry and Response

                
                if ((objUserLogin.lMobileNo != 0))
                {
                    r = new Random();
                    sMobile_OTP = r.Next().ToString();
                    sMobile_OTP = sMobile_OTP.PadLeft(6, '0');
                    sMobile_OTP = sMobile_OTP.Substring(0, 6);
                    sendSMS(objUserLogin.lMobileNo, "Your OTP for " + GlobalClass.sCompanyName + " is " + sMobile_OTP);
                }
                

                objParam[iParamCount].sParamName = "@vQueryType";
                objParam[iParamCount].sParamType = SqlDbType.VarChar;
                objParam[iParamCount].sParamValue = "USERLOGIN";
                iParamCount++;

                
                objParam[iParamCount].sParamName = "@iMobileNo";
                objParam[iParamCount].sParamType = SqlDbType.BigInt;
                objParam[iParamCount].sParamValue = Convert.ToString(objUserLogin.lMobileNo);
                iParamCount++;

                
                objParam[iParamCount].sParamName = "@iMobileOTP";
                objParam[iParamCount].sParamType = SqlDbType.Int;
                objParam[iParamCount].sParamValue = sMobile_OTP;
                iParamCount++;

                sQuery = "ProcUserLogin";
                sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
                if (sRetVal == "SUCCESS")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
                }
                #endregion
            }
            catch(Exception e1)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + e1.Message);
            }
        }

        public void sendSMS(long lMobileNo, string sMessage)
        {
            #region Declaration
            //Your user name
            string user = "";
            //Your authentication key
            string key = "6a0945b45bXX";
            //Multiple mobiles numbers separated by comma
            string mobile = lMobileNo.ToString();
            //Sender ID,While using route4 sender id should be 6 characters long.
            string senderid = "SGRMND";
            //Your message to send, Add URL encoding here.
            string message = HttpUtility.UrlEncode(sMessage);

            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            #endregion

            sbPostData.AppendFormat("user={0}", user);
            sbPostData.AppendFormat("&key={0}", key);
            sbPostData.AppendFormat("&mobile={0}", mobile);
            sbPostData.AppendFormat("&message={0}", message);
            sbPostData.AppendFormat("&senderid={0}", senderid);
            sbPostData.AppendFormat("&accusage={0}", "1");


            try
            {
                user = ConfigurationManager.AppSettings["SMSuser"];
                //Call Send SMS API
                string sendSMSUri = "http://103.233.79.246/submitsms.jsp?";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }
        }

        public HttpResponseMessage ValidateOTP(UserLogin objUserOTP)
        {
            #region Declaration
            string sRetVal = "";
            DataTable dt = null;
           
            #endregion

           try
            {
                if (long.Equals(objUserOTP.lMobileNo, null) || objUserOTP.lMobileNo.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Mobile Number can't be blank");
                }

                DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
                DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[3];

                structDBOperation[0].sParamName = "@vQueryType";
                structDBOperation[0].sParamType = SqlDbType.VarChar;
                structDBOperation[0].sParamValue = "GETUSEROTPDETAILS";

                structDBOperation[1].sParamName = "@iMobileNo";
                structDBOperation[1].sParamType = SqlDbType.BigInt;
                structDBOperation[1].sParamValue = Convert.ToString(objUserOTP.lMobileNo);

                structDBOperation[2].sParamName = "@iMobileOTP";
                structDBOperation[2].sParamType = SqlDbType.Int;
                structDBOperation[2].sParamValue = Convert.ToString(objUserOTP.iUserOTP);

                sRetVal = DBOperation.ExecuteDBOperation("ProcUserLogin", DBOperation.OperationType.STOREDPROC, structDBOperation, ref dt);

                if (sRetVal == "SUCCESS")
                {
                    //Logs.StoreActivityLogsInDB(LogType.Login, GlobalData.iUserID, "Logged in successfully.", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (dt != null && dt.Rows.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, "Mobile No Verified Successfully");
                    else
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Mobile OTP doesn't match.");
                }

                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + ex.Message);
            }
        }

        public HttpResponseMessage UpdateUserPreference(UserLogin objUserLogin)
        {
            try
            {
                #region declaration
                DataTable dt = new DataTable();
               
                Random r = new Random();
                DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
                DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[3];
                int iParamCount = 0;
                

                #endregion

                #region validation

                if (long.Equals(objUserLogin.lMobileNo, null) || objUserLogin.lMobileNo.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Mobile Number can't be blank");
                }


                #endregion

                #region DataBase Entry and Response

                objParam[iParamCount].sParamName = "@vQueryType";
                objParam[iParamCount].sParamType = SqlDbType.VarChar;
                objParam[iParamCount].sParamValue = "UPDATEPREFERENCE";
                iParamCount++;


                objParam[iParamCount].sParamName = "@iMobileNo";
                objParam[iParamCount].sParamType = SqlDbType.BigInt;
                objParam[iParamCount].sParamValue = Convert.ToString(objUserLogin.lMobileNo);
                iParamCount++;


                objParam[iParamCount].sParamName = "@iUserPreference";
                objParam[iParamCount].sParamType = SqlDbType.Int;

                objParam[iParamCount].sParamValue = Convert.ToString(objUserLogin.iUserPreference);
                iParamCount++;

               
                sRetVal = DBOperation.ExecuteDBOperation("ProcUserLogin", DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
                if (sRetVal == "SUCCESS")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
                }
                #endregion
            }
            catch (Exception e1)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + e1.Message);
            }
        }


        //***************************************** User Info **************************************************
        //      UserInfo  :   Insert User Info                                             *
        //      ValidateOTP :   Validate register mobile number with OTP                                        *
        //      UpdateUserPreference    :   Update user preference of user i.e. Company Owner/ Fleet Owner      *
        //                                                                                                      *
        //*******************************************************************************************************



        public HttpResponseMessage UpdateUserInfo(UserInfo objUserInfo)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            int iParamCount = 0;
            #endregion
            
            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[8];

            

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "USERINFO";
            iParamCount++;

            objParam[iParamCount].sParamName = "@iuser_id";
            objParam[iParamCount].sParamType = SqlDbType.Int;
            objParam[iParamCount].sParamValue = Convert.ToString(objUserInfo.iUserId);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vuser_name";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objUserInfo.sUser_Name;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vuser_email_id";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objUserInfo.sEmailID;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vuser_address";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objUserInfo.sUser_Address;
            iParamCount++;

            objParam[iParamCount].sParamName = "@iuser_zip";
            objParam[iParamCount].sParamType = SqlDbType.Int;
            objParam[iParamCount].sParamValue = Convert.ToString(objUserInfo.sUser_Zip);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vuser_gst_pan";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objUserInfo.sUser_Gst_Pan;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vuser_company_name";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objUserInfo.sCompany_Name;
            iParamCount++;

            sQuery = "ProcUserInfo";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "User Info Updated Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }
        
        public HttpResponseMessage SaveRating(UserRating objUserRating)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[4];
            int iParamCount = 0;
            #endregion

            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "RATE";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vRaterID";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString( objUserRating.RaterID);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vRating";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objUserRating.Rating);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vUserID";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objUserRating.UserID);
            iParamCount++;


            sQuery = "ProcRatingOperation";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Rated Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

    }
}
