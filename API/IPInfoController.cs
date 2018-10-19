using DBInteraction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProxyInfoWeb.API
{
    public class IPInfoController : ApiController
    {
        IP_Info objIP_Info = null;
        public IPInfoController()  
        {
            objIP_Info = new IP_Info();  
        }

        public List<IP_Info> GetIPInfo()
        {
            DataTable dt = new DataTable();
            List<IP_Info> lstIPInfo = null;
            EncryptionLibrary.Encryption ec = new EncryptionLibrary.Encryption();
            DBOperation.sConnectionString = ec.Decrypt("37xtqIiIGXiQkQc622OS1AysWTE2NxFWWDrkzVjnSqYRs0iRV5OWMGtRX4lgGMKBje7bNqW7dVO4ndtD+yt8rANeuuGjGou5522ObtIx78K3zkSOVIF5gvWn96hy9OW0");
            string sQuery = "SELECT * FROM IP_Info";
            DBOperation.ExecuteDBOperation(sQuery, DBOperation.OperationType.SELECT, null, ref dt);

            lstIPInfo = (from DataRow dr in dt.Rows
                           select new IP_Info()
                           {
                               SerialNo = Convert.ToInt32(dr["SerialNo"]),
                               IPv4 = Convert.ToString( dr["IPv4"]),
                               Proxy = Convert.ToString(dr["Proxy"]),
                               Proxy_Port = Convert.ToString(dr["Proxy_Port"]),
                               IPv4_Address = Convert.ToString(dr["IPv4_Address"]),
                               IPv4_City = Convert.ToString(dr["IPv4_City"]),
                               IPv4_State = Convert.ToString(dr["IPv4_State"]),
                               IPv4_Country = Convert.ToString(dr["IPv4_Country"]),
                               IPv4_Category = Convert.ToString(dr["IPv4_Category"]),
                               IPv4_Speed = Convert.ToString(dr["IPv4_Speed"]),
                               ASNUM = Convert.ToString(dr["ASNUM"]),
                               ORG_NAME = Convert.ToString(dr["ORG_NAME"]),
                               POSTAL_CODE = Convert.ToString(dr["POSTAL_CODE"]),
                               LATITUDE = Convert.ToString(dr["LATITUDE"]),
                               LONGITUDE = Convert.ToString(dr["LONGITUDE"]),
                               TZ = Convert.ToString(dr["TZ"])
                           }).ToList();

            //try
            //{
            //    lstIPInfo = objIP_Info.;
            //}
            //catch
            //{
            //    lstIPInfo = null;
            //}
            return lstIPInfo;
        }  

    }
}
