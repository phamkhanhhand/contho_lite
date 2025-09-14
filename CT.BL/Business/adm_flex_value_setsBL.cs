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
    public class adm_flex_value_setsBL : BaseBL
    {
        protected override Type EntityType
        {
            get
            {
                return typeof(adm_flex_value_sets);
            }
        }
        protected override List<Type> GetDetailBusinessType()
        {
            //return new List<Type>() { typeof(Contract) };
            return new List<Type>() { };
        }



        public bool AddLink(ParentChildDTO parentChildDTO)
        {
            var rs = false;

            adm_flex_value_setsDL dl = new adm_flex_value_setsDL();

            //todo validate cant drop link if has value parent-child link

            rs = dl.AddLink(parentChildDTO);
             
            return rs;
        }

    }
}
