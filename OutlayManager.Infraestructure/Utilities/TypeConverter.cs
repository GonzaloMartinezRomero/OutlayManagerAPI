using System;

namespace OutlayManagerAPI.Infraestructure.Utilities
{
    internal static class TypeConverter
    {
        public static int? ToInteger(object obj)
        {
            string value = obj?.ToString() ?? String.Empty;

            if (Int32.TryParse(value,out int result))
                return result;
            else
                return null;
        }
    }
}
