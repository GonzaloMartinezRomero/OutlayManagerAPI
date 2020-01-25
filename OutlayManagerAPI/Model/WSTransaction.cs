using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.Model
{
    public class WSTransaction
    {       
        public int ID { get; set; }
        
        public double Amount { get; set; }
      
        public DateTime Date { get; set; }
      
        public WSDetailTransaction DetailTransaction { get; set; }
    }
}
