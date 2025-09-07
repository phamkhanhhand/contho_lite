using CT.Models.Entity;
using CT.Models.ServerObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class SalaryTableDetailBL : BaseBL
    {


        protected override Type EntityType
        {
            get
            {
                return typeof(SalaryTableDetail);
            }
            set
            {
            }

        }
        protected override List<Type> GetDetailBusinessType()
        {
            return null;
        }
         
    }
}
