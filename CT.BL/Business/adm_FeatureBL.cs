using CT.DL;
using CT.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class adm_FeatureBL : BaseBL
    {
        /// <summary>
        /// Gán quyền
        /// </summary>
        /// <param name="featureID"></param>
        /// <returns></returns>
        /// thupth 23.11.2024
        public List<ViewFeatureEmployee> AssignPermissionFeatureEmployee(int featureID)
        {
            adm_FeatureDL dl = new adm_FeatureDL();
            return dl.AssignPermissionFeatureEmployee(featureID);
        }

        public List<ViewFeatureEmployee> RokePermissionFeatureEmployee(int featureID)
        {
            adm_FeatureDL dl = new adm_FeatureDL();
            return dl.RevokePermissionFeatureEmployee(featureID);
        }

        public List<ViewFeatureEmployee> GetPermisionByFeatureID(int featureID)
        {

            adm_FeatureDL dl = new adm_FeatureDL();


            return dl.GetPermisionByFeatureID(featureID);

        }
    }
}
