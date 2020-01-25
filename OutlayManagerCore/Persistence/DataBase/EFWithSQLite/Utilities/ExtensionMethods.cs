using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite.Utilities
{
    public static class ExtensionMethods
    {
        public static DateTime ToDateTime(this string str)
        {
            DateTime result = default;

            if (!String.IsNullOrEmpty(str) && !DateTime.TryParse(str, out result))
                throw new Exception("Imposible to cast to DateTime");

            return result;
        }
    }
}
