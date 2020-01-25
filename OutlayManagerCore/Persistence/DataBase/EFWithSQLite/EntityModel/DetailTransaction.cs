using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Persistence.DataBase.EFWithSQLite.EntityModel
{
    public class DetailTransaction : Entity
    {  
        public string Code { get; set; }
        public string Description { get; set; }
        public string TypeTransaction { get; set; }
    }
}
