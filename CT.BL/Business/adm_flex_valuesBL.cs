using CT.DL;
using CT.Models.DTO;
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




        public bool AddLink(ParentChildDTO linkSetDTO)
        {
            var rs = false;

            adm_flex_valuesDL dl = new adm_flex_valuesDL();

            //todo validate cant drop link if has value parent-child link.
            //todo validate set_id is true

            rs = dl.AddLink(linkSetDTO);

            return rs;
        }


    }
}
