using CT.DL;
using CT.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class adm_FunctionBL : BaseBL
    {
        public object GetPermisionByMyFunctionID(int functionID)
        {

            adm_FunctionDL dl = new adm_FunctionDL();


            return dl.GetPermisionByMyFunctionID(functionID);

        }
    }
}
