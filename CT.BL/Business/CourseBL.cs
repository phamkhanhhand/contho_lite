using CT.Models.Entity;
using CT.Models.ServerObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class CourseBL : BaseBL
    {


        protected override Type EntityType
        {
            get
            {
                return typeof(Course);
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
