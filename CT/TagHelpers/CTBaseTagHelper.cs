using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;


namespace CT.TagHelpers
{

    //[HtmlTargetElement("ct-base")]
    public class CTBaseTagHelper : TagHelper
    {


        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var pageContext = ViewContext?.ViewData["ct-page"]?.ToString();

            if (!String.IsNullOrWhiteSpace(pageContext))
            { 
                output.Attributes.SetAttribute("ct-page", pageContext);
            }

            if (!String.IsNullOrWhiteSpace(ctName))
            {
                output.Attributes.SetAttribute("ct-name", ctName);
            }

        }


        /// <summary>
        /// ID ở dưới client
        /// </summary>
        /// phamkhanhhand 05.11.2022
        /// 
        [HtmlAttributeName("ct-name")]
        public string ctName { get; set; } = Guid.NewGuid().ToString();



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
