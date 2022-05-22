using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerCore.Infraestructure.Persistence.Model
{
    internal class Entity
    {
        public int ID { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Entity entity)
                return entity.ID == this.ID;

            return false;
        }

        public override int GetHashCode()
        {
            return this.ID.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return this.ID.ToString();
        }
    }
}
