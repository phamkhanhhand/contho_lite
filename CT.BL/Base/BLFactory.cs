using CT.BL;
using CT.Models.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class BLFactory
    {
        private static string namespaceString = Models.Enumeration.NameSpace.NameSpaceBL.GetStringValue() + ".";

        public static BaseBL CreateBL<T>()
        {
            string type = Activator.CreateInstance<T>().GetType().Name + "BL";
            Type t = Type.GetType(namespaceString + type);
            return (BaseBL)Activator.CreateInstance(t);
        }

        public static BaseBL CreateBLByType(Type type)
        {
            string typeString = type.Name + "BL";
            Type t = Type.GetType(namespaceString + typeString);
            return (BaseBL)Activator.CreateInstance(t);
        }

        public static BaseBL CreateBLByEntityName(string entityName)
        {
            var typeString = entityName + "BL";
            Type t = Type.GetType(namespaceString + typeString);
            return (BaseBL)Activator.CreateInstance(t);
        }

         
    }
}
