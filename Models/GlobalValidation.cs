using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxyInfoWeb.Models
{
    public class GlobalValidation
    {
        public static bool ShouldNotNull(int iVal)
        {
            if (iVal <= 0)
            {
                return false;
            }

            return true;
        }
    }
}