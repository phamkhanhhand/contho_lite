using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models
{

    /// <summary>
    /// Attribute Giá trị string
    /// </summary>
    /// phamkhanhhand 22.07.2023
    public class StringValueAttribute : Attribute
    {

        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        public string StringValue { get; protected set; }
    }



}
