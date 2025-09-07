using CT.Models.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DL
{
    public class DLFactory
    {
        private static string namespaceString = Models.Enumeration.NameSpace.NameSpaceDL.GetStringValue() + ".";
        public static BaseDL CreateDL<T>()
        {
            string type = Activator.CreateInstance<T>().GetType().Name + "DL";
            Type t = Type.GetType(namespaceString + type);
             return (BaseDL) Activator.CreateInstance(t);
        }

        public static BaseDL CreateDLByType(Type type)
        {
            string typeString = type.Name + "DL";
            Type t = Type.GetType(namespaceString + typeString);
            return (BaseDL)Activator.CreateInstance(t);
        }

        public static BaseDL CreateDLByEntityName(string entityName)
        {
            var typeString = entityName + "DL";
            Type t = Type.GetType(namespaceString + typeString);
            return (BaseDL)Activator.CreateInstance(t);
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static BaseDL CreateEmployeeDL()
        //{

        //    Type t = Type.GetType("DL.EmployeeDL");
        //    return (BaseDL)Activator.CreateInstance(t);
        //}
    }
}
