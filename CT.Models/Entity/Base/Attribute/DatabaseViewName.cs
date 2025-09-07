using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{

    /// <summary>
    /// có thể là view hoặc tên bảng, khi entity trong c# khác với trong database
    /// table name or view name in database
    /// </summary>
    public class DatabaseViewName : Attribute
    {
         

        public DatabaseViewName(string viewName)
        {
            this.ViewName = viewName;
        }

        public string ViewName { get; set; }
    }
}
