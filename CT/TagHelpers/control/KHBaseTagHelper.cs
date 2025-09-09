using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers; 

namespace KH.ITFamily.FileCenter.Base.KHControl
{

    //[HtmlTargetElement("kh-base")]
    public class KHBaseTagHelper : TagHelper
    {
        /// <summary>
        /// ID ở dưới client
        /// </summary>
        /// phamkhanhhand 05.11.2022
        /// 
        [HtmlAttributeName("htmlID")]
        public string htmlID { get; set; } = Guid.NewGuid().ToString();

          

        public string htmlIDStr
        {
            get { return "'" + htmlID + "'"; }
        }
    }

}
