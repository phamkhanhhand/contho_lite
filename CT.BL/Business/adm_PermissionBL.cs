using CT.DL;
using CT.Models.Entity;
using CT.UserContext.CurrentContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class adm_PermissionBL :BaseBL
    {
        // Lấy quyền hạn của nhân viên theo tính năng



        /// <summary>
        /// Check permision
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// phamkhanhhand Sep 13, 2025
        public bool CheckPermision(string userName, List<string> listScope, string uri)
        {

            //var currentUserID = CurrentUserService.GetCurrentProfileEmployee()?.EmployeeID;

            adm_EmployeeDL dl = new adm_EmployeeDL();

            return dl.CheckPermision(userName, listScope, uri); 
        }

    }
}
