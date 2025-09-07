using CT.DL;
using CT.Models.Entity;
using CT.Models.ServerObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class ImportMappingBL : BaseBL
    {


        protected override Type EntityType
        {
            get
            {
                return typeof(ImportMapping);
            }
        }
        protected override List<Type> GetDetailBusinessType()
        {
            //return new List<Type>() { typeof(Contract) };
            return new List<Type>() { };
        }
         

    }
}
