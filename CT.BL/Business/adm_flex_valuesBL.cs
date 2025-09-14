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
    public class adm_flex_valuesBL : BaseBL
    { 
        protected override Type EntityType
        {
            get
            {
                return typeof(adm_flex_values);
            }
        }
        protected override List<Type> GetDetailBusinessType()
        {
            //return new List<Type>() { typeof(Contract) };
            return new List<Type>() { };
        } 


    }
}
