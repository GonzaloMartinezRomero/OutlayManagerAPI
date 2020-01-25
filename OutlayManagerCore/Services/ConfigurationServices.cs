using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Services
{
    public class ConfigurationServices
    {
        public enum TypesProviders
        {
            MEMORY,
            SQLITE_ON_EF
        }

        public string PathConnection { get; set; }
        public TypesProviders Provider { get; set; }
    }
}
