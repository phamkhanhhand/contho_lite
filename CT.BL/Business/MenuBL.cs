using CT.DL;
using CT.Models.Entity;
using CT.Models.ServerObject;
using CT.UserContext.CurrentContext;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class MenuBL : BaseBL
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
            return new List<Type>() { };
        }

        /// <summary>
        /// Lấy tất cả menu của người dùng đang đăng nhập
        /// </summary>
        /// <returns></returns>
        public List<adm_Feature> GetAllMenuByCurrentUser()
        {
            var employeeID = CurrentUserHelper.GetCurrentProfileEmployee()?.EmployeeID;

            if (employeeID != null)
            {

                var dl = new MenuDL();

                return dl.GetMenuByEmployeeID(employeeID.Value);
            }
            else
            {
                return null;
            }
             
        }

        public List<ViewPermision> GetAllPermisionByFeatureByCurrentUser(string featureCode)
        {
            var employeeID = CurrentUserHelper.GetCurrentProfileEmployee()?.EmployeeID;

            if (employeeID != null)
            {
                var dl = new MenuDL();
                return dl.GetAllPermisionByFeatureAndEmployeeID(employeeID.Value,featureCode);
            }
            else
            {
                return null;
            }

        }

    }
}
