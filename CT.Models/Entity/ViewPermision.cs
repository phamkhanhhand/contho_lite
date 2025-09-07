using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{ 
    public class ViewPermision : BaseEntity
    {
         
        public string Username { get; set; }
        public string FeatureName { get; set; }
        public string FunctionCode { get; set; }
        public string FeatureCode { get; set; }
    }
}
