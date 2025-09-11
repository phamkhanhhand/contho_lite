using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace CT.TagHelpers
{

    [HtmlTargetElement("ct-base-page")]
    public class CTBasePageTagHelper : TagHelper
    {


        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }


        [HtmlAttributeName("page-scope")]
        public string pageScope { get; set; }  // Text của label


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);


            if (String.IsNullOrWhiteSpace(pageScope))
            {

                var pageContext = ViewContext?.ViewData["ct-page"]?.ToString();

                if (!String.IsNullOrWhiteSpace(pageContext))
                {
                    this.pageScope = pageContext;
                }
            }

            string mapping = @"
<script> 
 
    document.addEventListener('DOMContentLoaded', function () {
         
     var  flexValueSetPage = new CTListForm({pageScope:'" + pageScope + @"'});
        
        flexValueSetPage. configControlsAutoConext();
    });

</script>
 ";

            output.PostElement.AppendHtml(mapping);



        }

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
