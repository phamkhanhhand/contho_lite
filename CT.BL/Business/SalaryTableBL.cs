using CT.Models.Entity;
using CT.Models.ServerObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class SalaryTableBL : BaseBL
    {


        protected override Type EntityType
        {
            get
            {
                return typeof(SalaryTable);
            }  
        }
        protected override List<Type> GetDetailBusinessType()
        {
            return new List<Type>() { typeof(SalaryTableDetail) };
        }


         

    }
}
