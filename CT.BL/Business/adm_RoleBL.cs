using CT.DL;
using CT.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class adm_RoleBL : BaseBL // Kế thừa từ BaseBL
    {
       
        protected override Type EntityType
        {
            get
            {
                return typeof(adm_Role); 
            }
        }

        
        protected override List<Type> GetDetailBusinessType()
        {
            
            return new List<Type>() { };
        }






        /// <summary>
        /// Lấy danh sách quyền theo tính năng
        /// </summary>
        /// <param name="featureID"></param>
        /// <returns></returns>
        /// hathu 12.11.2024
        public List<ViewPermision> GetPermisionByFeatureID(int featureID)
        {

            adm_RoleDL roleDL = new adm_RoleDL();

            return roleDL.GetPermisionByFeatureID(featureID);


        }

        /// hathu 22.11.2024
        public List<ViewFeatureRole> AssignPermission(int roleID)
        {

            adm_RoleDL roleDL = new adm_RoleDL();

            return roleDL.AssignPermission(roleID);


        }
        public List<ViewFeatureRole> RevokePermission(int roleID)
        {
            adm_RoleDL roleDL = new adm_RoleDL();

            return roleDL.RevokePermission(roleID);
        }

        public List<ViewFeatureRole> GetPermissionFeatureRole(int roleID)
        {
            adm_RoleDL roleDL = new adm_RoleDL();

            return roleDL.GetPermissionFeatureRole(roleID);
        }
    }
}
