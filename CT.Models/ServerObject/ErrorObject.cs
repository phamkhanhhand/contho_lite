using CT.Models.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.ServerObject
{
    public class ErrorObject
    {
        public ValidationErrorType ErrorType { get; set; }
        public string Text { get; set; }
    }
}
