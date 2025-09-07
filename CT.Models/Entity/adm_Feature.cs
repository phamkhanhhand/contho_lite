using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    public class adm_Feature : BaseEntity
    {
        public int FeatureID { get; set; }
        public string FeatureName { get; set; }
        public string Description { get; set; }
        public string FeatureCode { get; set; }
        public string?  FeatureURL { get; set; }


    }
}
