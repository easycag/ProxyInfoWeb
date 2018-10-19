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
       
        public HttpResponseMessage UserRegister(UserInfo objUserInfo)
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

                if (long.Equals(objUserInfo.lMobileNo,null) || objUserInfo.lMobileNo.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Mobile Number can't be blank");
                }

                
                #endregion

                #region DataBase Entry and Response

                
                if ((objUserInfo.lMobileNo != 0))
                {
                    r = new Random();
                    sMobile_OTP = r.Next().ToString();
                    sMobile_OTP = sMobile_OTP.PadLeft(6, '0');
                    sMobile_OTP = sMobile_OTP.Substring(0, 6);
                    sendSMS(objUserInfo.lMobileNo, "Your OTP for " + GlobalClass.sCompanyName + " is " + sMobile_OTP);
                }
                

                objParam[iParamCount].sParamName = "@vQueryType";
                objParam[iParamCount].sParamType = SqlDbType.VarChar;
                objParam[iParamCount].sParamValue = "REGISTERUSERDETAILS";
                iParamCount++;

                
                objParam[iParamCount].sParamName = "@vMobileNo";
                objParam[iParamCount].sParamType = SqlDbType.VarChar;
                objParam[iParamCount].sParamValue = Convert.ToString(objUserInfo.lMobileNo);
                iParamCount++;

                
                objParam[iParamCount].sParamName = "@vMobileOTP";
                objParam[iParamCount].sParamType = SqlDbType.VarChar;
                objParam[iParamCount].sParamValue = Convert.ToString(sMobile_OTP);
                iParamCount++;

                sQuery = "ProcUserRegister";
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

        public static void SendEmail(string Body, string sRequesEmail)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(GlobalClass.sSenderMail);
            message.To.Add(sRequesEmail);
            message.Subject = GlobalClass.sSubject;
            message.IsBodyHtml = true;
            message.Body = Body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = true;

            smtpClient.Host = GlobalClass.sHost;
            smtpClient.Port = Convert.ToInt32(GlobalClass.sPort);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(GlobalClass.sSenderMail, GlobalClass.sSenderPassword);
            smtpClient.Send(message);
        }

        public HttpResponseMessage ValidateLogin(string sUserName,string sPassword)
        {
            #region Declaration
            string sRetVal = "";
            DataTable dt = null;
            #endregion

            #region validation
            if (string.IsNullOrEmpty(sUserName))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Username can't be blank.");
            }

            if (string.IsNullOrEmpty(sPassword))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Password can't be blank.");
            }
            #endregion
            try
            {
                DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
                DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[2];

                structDBOperation[0].sParamName = "@vQueryType";
                structDBOperation[0].sParamType = SqlDbType.VarChar;
                structDBOperation[0].sParamValue = "GETUSERDETAILS";

                structDBOperation[1].sParamName = "@vUserName";
                structDBOperation[1].sParamType = SqlDbType.VarChar;
                structDBOperation[1].sParamValue = sUserName;

                sRetVal = DBOperation.ExecuteDBOperation("ProcGetUserInfo_App", DBOperation.OperationType.STOREDPROC, structDBOperation, ref dt);
                
                if (sRetVal == "SUCCESS")
                {
                    //Logs.StoreActivityLogsInDB(LogType.Login, GlobalData.iUserID, "Logged in successfully.", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["Password"]) != sPassword)
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Invalid Username or password.");

                        return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Invalid Username or password.");
                }

                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + ex.Message);
            }
        }


        public HttpResponseMessage ValidateOTP(UserOTP objUserOTP)
        {
            #region Declaration
            string sRetVal = "";
            DataTable dt = null;
            string sMessage = "";
            #endregion

            #region validation
            //if (string.IsNullOrEmpty(objUserOTP.sEmailID))
            //{
            //    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "User Email can't be blank.");
            //}
            //if (objUserOTP.iTypeValidate == 1 || objUserOTP.iTypeValidate == 3)
            //{
            //    if (string.IsNullOrEmpty(objUserOTP.sEmailOTP))
            //    {
            //        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Email OTP can't be blank.");
            //    }
            //}

            //if (objUserOTP.iTypeValidate == 2 || objUserOTP.iTypeValidate == 3)
            //{
            //    if (string.IsNullOrEmpty(objUserOTP.sMobileOTP))
            //    {
            //        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Mobile OTP can't be blank.");
            //    }
            //}
            #endregion
            try
            {
                DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
                DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[2];

                structDBOperation[0].sParamName = "@vQueryType";
                structDBOperation[0].sParamType = SqlDbType.VarChar;
                structDBOperation[0].sParamValue = "GETUSEROTPDETAILS";

                structDBOperation[1].sParamName = "@vUserName";
                structDBOperation[1].sParamType = SqlDbType.VarChar;
                structDBOperation[1].sParamValue = objUserOTP.sEmailID;

                sRetVal = DBOperation.ExecuteDBOperation("ProcGetUserInfo_App", DBOperation.OperationType.STOREDPROC, structDBOperation, ref dt);

                if (sRetVal == "SUCCESS")
                {
                    //Logs.StoreActivityLogsInDB(LogType.Login, GlobalData.iUserID, "Logged in successfully.", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //if (objUserOTP.iTypeValidate == 1 || objUserOTP.iTypeValidate == 3)
                        //{
                        if (!(string.IsNullOrEmpty(objUserOTP.sEmailOTP)))
                        {
                            if (Convert.ToString(dt.Rows[0]["EmailID_OTP"]) != objUserOTP.sEmailOTP)
                                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Email OTP doesn't match.");
                            else
                            {
                                sRetVal = UpdateEmailOTPStatus(objUserOTP.sEmailID);
                                if (sRetVal == "SUCCESS")
                                {
                                    sMessage += "Email Verified Successfully";
                                }
                            }
                        }
                        //}

                        //if (objUserOTP.iTypeValidate == 2 || objUserOTP.iTypeValidate == 3)
                        //{
                        if (!(string.IsNullOrEmpty(objUserOTP.sMobileOTP)))
                        {
                            if (Convert.ToString(dt.Rows[0]["Mobile_OTP"]) != objUserOTP.sMobileOTP)
                                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Mobile OTP doesn't match.");
                            else
                            {
                                sRetVal = UpdateMobileOTPStatus(objUserOTP.sEmailID);
                                if (sRetVal == "SUCCESS")
                                {
                                    sMessage += "Mobile No Verified Successfully";
                                }
                            }
                        }
                        //}
                        return Request.CreateResponse(HttpStatusCode.OK, sMessage);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Invalid Username or password.");
                }

                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + ex.Message);
            }
        }

        private string UpdateEmailOTPStatus(string sEmailID)
        {
            DataTable dt = new DataTable();
            DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[2];

                structDBOperation[0].sParamName = "@vQueryType";
                structDBOperation[0].sParamType = SqlDbType.VarChar;
                structDBOperation[0].sParamValue = "UPDATEEMAIL";

                structDBOperation[1].sParamName = "@vUserName";
                structDBOperation[1].sParamType = SqlDbType.VarChar;
                structDBOperation[1].sParamValue = sEmailID;

                sRetVal = DBOperation.ExecuteDBOperation("ProcUpdateUserStatus", DBOperation.OperationType.STOREDPROC_UPDATE, structDBOperation, ref dt);

                if (sRetVal == "SUCCESS")
                {
                    return "SUCCESS";
                }
                else
                    return "FAILURE";
        }

        private string UpdateMobileOTPStatus(string sEmailID)
        {
            DataTable dt = new DataTable();
            DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[2];

            structDBOperation[0].sParamName = "@vQueryType";
            structDBOperation[0].sParamType = SqlDbType.VarChar;
            structDBOperation[0].sParamValue = "UPDATEMOBILE";

            structDBOperation[1].sParamName = "@vUserName";
            structDBOperation[1].sParamType = SqlDbType.VarChar;
            structDBOperation[1].sParamValue = sEmailID;

            sRetVal = DBOperation.ExecuteDBOperation("ProcUpdateUserStatus", DBOperation.OperationType.STOREDPROC_UPDATE, structDBOperation, ref dt);

            if (sRetVal == "SUCCESS")
            {
                return "SUCCESS";
            }
            else
                return "FAILURE";
        }

        public HttpResponseMessage UpdateRecord(UserInfo objUserInfo)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[7];
            int iParamCount = 0;
            string sEmailID_OTP = "";
            string sMobile_OTP = "";
            string sMobile = "";
            #endregion

            #region validation

            if (string.IsNullOrEmpty(objUserInfo.sEmailID))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Email ID can't be blank");
            }

            if (string.IsNullOrEmpty(objUserInfo.sCompany_Name))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Company Name can't be blank");
            }
            if (string.IsNullOrEmpty(objUserInfo.sPassword))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Password can't be blank");
            }
            #region Email Verification
            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
            DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[2];

            structDBOperation[0].sParamName = "@vQueryType";
            structDBOperation[0].sParamType = SqlDbType.VarChar;
            structDBOperation[0].sParamValue = "GETUSERDETAILS";

            structDBOperation[1].sParamName = "@vUserName";
            structDBOperation[1].sParamType = SqlDbType.VarChar;
            structDBOperation[1].sParamValue = objUserInfo.sEmailID;

            sRetVal = DBOperation.ExecuteDBOperation("ProcGetUserInfo_App", DBOperation.OperationType.STOREDPROC, structDBOperation, ref dt);

            if (sRetVal == "SUCCESS")
            {
                //Logs.StoreActivityLogsInDB(LogType.Login, GlobalData.iUserID, "Logged in successfully.", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (dt != null && dt.Rows.Count > 0)
                {
                    sMobile = Convert.ToString( dt.Rows[0]["MobileNo"]);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "No Email ID register.");
                }
            }
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Some error occured into information fetching, Please Contact administrator.");
            #endregion

            #endregion

            if ((objUserInfo.lMobileNo != Convert.ToInt64( sMobile)))
            {
                r = new Random();
                sMobile_OTP = r.Next().ToString();
                sMobile_OTP = sMobile_OTP.PadLeft(6, '0');
                sMobile_OTP = sMobile_OTP.Substring(0, 6);
                sendSMS(objUserInfo.lMobileNo, "Your OTP for " + GlobalClass.sCompanyName + " is " + sMobile_OTP);
            }

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "UPDATEUSERDETAILS";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vEmail";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objUserInfo.sEmailID;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vPassword";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = objUserInfo.sPassword;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vMobileNo";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objUserInfo.lMobileNo);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCompanyName";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(objUserInfo.sCompany_Name);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vEmailOTP";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(sEmailID_OTP);
            iParamCount++;

            objParam[iParamCount].sParamName = "@vMobileOTP";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString(sMobile_OTP);
            iParamCount++;

            sQuery = "ProcUserRegister";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
        }

        public HttpResponseMessage ForgotPassword(UserInfo objUserInfo)
        {
            #region declaration
            DataTable dt = new DataTable();
            
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[7];
            int iParamCount = 0;
            string sOTP = "";
            string sMobile = "";
            #endregion

            #region validation

            if (string.IsNullOrEmpty(objUserInfo.sEmailID))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Email ID can't be blank");
            }
            #endregion
            #region Business Logic 
            DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
            DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[2];

            structDBOperation[0].sParamName = "@vQueryType";
            structDBOperation[0].sParamType = SqlDbType.VarChar;
            structDBOperation[0].sParamValue = "GETUSERDETAILS";

            structDBOperation[1].sParamName = "@vUserName";
            structDBOperation[1].sParamType = SqlDbType.VarChar;
            structDBOperation[1].sParamValue = objUserInfo.sEmailID;

            sRetVal = DBOperation.ExecuteDBOperation("ProcGetUserInfo_App", DBOperation.OperationType.STOREDPROC, structDBOperation, ref dt);

            if (sRetVal == "SUCCESS")
            {
                //Logs.StoreActivityLogsInDB(LogType.Login, GlobalData.iUserID, "Logged in successfully.", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (dt != null && dt.Rows.Count > 0)
                {
                    sMobile = Convert.ToString(dt.Rows[0]["MobileNo"]);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "No Email ID register.");
                }
            }
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Some error occured into information fetching, Please Contact administrator.");


            r = new Random();
            sOTP = r.Next().ToString();
            sOTP = sOTP.PadLeft(6, '0');
            sOTP = sOTP.Substring(0, 6);

            if (!(string.IsNullOrEmpty(objUserInfo.sEmailID)))
            {   
                SendEmail("Your Email OTP for password recovery is " + sOTP, objUserInfo.sEmailID);
            }
            
            if (string.IsNullOrEmpty(sMobile))
            {
                sendSMS(objUserInfo.lMobileNo, "Your OTP for password recovery is " + sOTP);
            }

            #region OTP insertion into Table for verification
            string sQuery = "ProcUserForgetPassword";
            DBOperation.StructDBOperation[] structTempDBOperation = new DBOperation.StructDBOperation[3];

            structTempDBOperation[iParamCount].sParamName = "@vQueryType";
            structTempDBOperation[iParamCount].sParamType = SqlDbType.VarChar;
            structTempDBOperation[iParamCount].sParamValue = "GETUSERDETAILS";
            iParamCount++;

            structTempDBOperation[iParamCount].sParamName = "@vEmail";
            structTempDBOperation[iParamCount].sParamType = SqlDbType.VarChar;
            structTempDBOperation[iParamCount].sParamValue = objUserInfo.sEmailID;
            iParamCount++;

            structTempDBOperation[iParamCount].sParamName = "@vOTP";
            structTempDBOperation[iParamCount].sParamType = SqlDbType.VarChar;
            structTempDBOperation[iParamCount].sParamValue = sOTP;
            iParamCount++;

            sRetVal= DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC, structTempDBOperation, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "OTP sent to registered email id or mobile number successfully.");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error in generating OTP.");
            }
            #endregion
            #endregion
        }
        
        public HttpResponseMessage ValidateForgotPasswordOTP(UserInfo_ForGetOTP objUserInfoForGetOTP)
        {
            #region Declaration
            string sRetVal = "";
            DataTable dt = null;
            #endregion

            #region validation
            if (string.IsNullOrEmpty(objUserInfoForGetOTP.sEmailID))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Email ID can't be blank.");
            }

            if (string.IsNullOrEmpty(objUserInfoForGetOTP.sOTP))
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "OTP can't be blank.");
            }
            #endregion
            try
            {
                DBOperation.sConnectionString = ec.Decrypt(GlobalClass.sConnectionString);
                DBOperation.StructDBOperation[] structDBOperation = new DBOperation.StructDBOperation[2];

                structDBOperation[0].sParamName = "@vQueryType";
                structDBOperation[0].sParamType = SqlDbType.VarChar;
                structDBOperation[0].sParamValue = "FORGOTPASSWORDOTPDETAILS";

                structDBOperation[1].sParamName = "@vUserName";
                structDBOperation[1].sParamType = SqlDbType.VarChar;
                structDBOperation[1].sParamValue = objUserInfoForGetOTP.sEmailID;

                sRetVal = DBOperation.ExecuteDBOperation("ProcGetUserInfo_App", DBOperation.OperationType.STOREDPROC, structDBOperation, ref dt);

                if (sRetVal == "SUCCESS")
                {
                    //Logs.StoreActivityLogsInDB(LogType.Login, GlobalData.iUserID, "Logged in successfully.", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["OTP"]) != objUserInfoForGetOTP.sOTP)
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Invalid OTP.");

                        return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Invalid OTP. Please Resend.");
                }

                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + ex.Message);
            }
        }

        public HttpResponseMessage ChangePassword(string sEmailID, string sPassword)
        {
            #region declaration
            DataTable dt = new DataTable();
            string sQuery = "";
            Random r = new Random();
            DBOperation.StructDBOperation[] objParam = new DBOperation.StructDBOperation[7];
            int iParamCount = 0;
            #endregion

            objParam[iParamCount].sParamName = "@vQueryType";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = "UPDATEPASSWORD";
            iParamCount++;

            objParam[iParamCount].sParamName = "@vEmail";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = sEmailID;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vPassword";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = sPassword;
            iParamCount++;

            objParam[iParamCount].sParamName = "@vMobileNo";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString("");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vCompanyName";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString("");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vEmailOTP";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString("");
            iParamCount++;

            objParam[iParamCount].sParamName = "@vMobileOTP";
            objParam[iParamCount].sParamType = SqlDbType.VarChar;
            objParam[iParamCount].sParamValue = Convert.ToString("");
            iParamCount++;

            sQuery = "ProcUserRegister";
            sRetVal = DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.STOREDPROC_UPDATE, objParam, ref dt);
            if (sRetVal == "SUCCESS")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Password Updated Successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Please Contact Administrator with below mentioned error " + sRetVal);
            }
            
        }

        public HttpResponseMessage ForgotChangePassword(string sEmailID, string sOldPassword, string sPassword)
        {
            #region declaration
            HttpResponseMessage vResponseMessage = null;           
            #endregion

            
            vResponseMessage = ValidateLogin(sEmailID, sOldPassword);
           if (vResponseMessage.StatusCode == HttpStatusCode.OK)
           {
               vResponseMessage= ChangePassword(sEmailID, sPassword);
               return vResponseMessage;
           }
           else
           {
               return vResponseMessage;
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
