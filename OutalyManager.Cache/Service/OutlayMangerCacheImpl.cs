using OutalyManager.Cache.Abstract;
using OutalyManager.Cache.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutalyManager.Cache.Service
{
    public static class OutlayMangerCacheImpl
    {
        public static IOutlayManagerCache GetService() => new OutlayManagerCacheService();
    }
}
