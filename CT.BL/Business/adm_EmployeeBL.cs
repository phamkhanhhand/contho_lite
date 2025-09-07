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
    public class adm_EmployeeBL : BaseBL
    {


        protected override Type EntityType
        {
            get
            {
                return typeof(adm_Employee);
            }
        }
        protected override List<Type> GetDetailBusinessType()
        {
            //return new List<Type>() { typeof(Contract) };
            return new List<Type>() { };
        }


        public adm_Employee GetEmployeeByUsername(string userName)
        {
            adm_EmployeeDL dl = new adm_EmployeeDL();

            return dl.GetEmployeeByUsername(userName);
        }


    }
}
