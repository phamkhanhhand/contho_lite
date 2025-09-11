using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection.Emit;

namespace CT.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("ct-menu-tool")]
    public class CTMenuToolTagHelper : CTBaseTagHelper
    {
        // Các thuộc tính cần thiết cho input

        [HtmlAttributeName("Text")]
        public string Text { get; set; }  // Text của label

        [HtmlAttributeName("Items")]
        public List<CTMenuItem> Items { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var content = @"  
            <div class='ct-grid-tool-bar' id='" + this.htmlID + @"'>
                <ul>
                    <li style='float: left;'>
                        <span class='search-container'>
                            <input type='text' placeholder='Tìm kiếm..' name='search'>
                            <button type='submit' class='button'>Tìm</button>
                        </span></li>
                        {0}
                </ul>
            </div>";

            var menu = "";

            for (int i = 0; i < Items.Count; i++)
            {
                var it = Items[i];

                var menuItem = "<li id='" + it.MBClientID + "'><a href='#'>" + it.Text + "</a></li>";
                menu += menuItem;
            }

            content = string.Format(content, menu);

            output.Content.AppendHtml(content);

            #region Mapping

            // Thêm script và CSS vào cuối nếu cần
            AddScriptAndCss(output);


            string mapping = @"
<script> 
{

    let temp = new KHMenuTool('" + this.htmlID + @"');
    temp.lstMenuItem = " + GetListColumnJavaScriptObject() + @"; 


        if (typeof App == 'undefined') {
                App = { };
        }
        App['" + this.htmlID + @"'] = temp; 

}

</script>
";

            output.PostElement.AppendHtml(mapping);


            #endregion
        }

        // Thêm script và CSS nếu có
        private void AddScriptAndCss(TagHelperOutput output)
        {
            // Thêm CSS cho input (ví dụ: dùng thư viện CSS như Bootstrap)
            output.PostElement.AppendHtml("<link rel='stylesheet' href='/css/ct-menu-tool.css' />");

            // Thêm script (ví dụ: validate input số, ngày tháng)
            output.PostElement.AppendHtml("<script src='/js/control/ct-menu-tool.js'></script>");
        }
        //public override void Process(TagHelperContext context, TagHelperOutput output)
        //{

        //}



        /// <summary>
        /// Tạo đối tượng javascript
        /// </summary>
        /// <returns></returns>
        /// phamkhanhhand
        private string GetListColumnJavaScriptObject()
        {
            var lstDataIndex = this.Items;

            string sJSON = "[{0}]";
            var lstChild = "";
            for (int i = 0; i < lstDataIndex.Count; i++)
            {
                var col = lstDataIndex[i];

                var coljs = @"new KHMenuItem( {
Command:'" + col.Command + @"',
id:'" + col.MBClientID + @"',
Text:'" + col.Text + @"'  
                }),";

                lstChild += coljs;

            }

            return string.Format(sJSON, lstChild);
        }


    }



    [HtmlTargetElement("ct-menu-item")]
    public class CTMenuItem
    {
        protected string _mbClientID = "id_" + Guid.NewGuid();

        public string MBClientID
        {
            get { return !string.IsNullOrEmpty(_mbClientID) ? _mbClientID : ("id_" + Guid.NewGuid()); }
            set { _mbClientID = value; }
        }


        [HtmlAttributeName("Text")]
        public string Text { get; set; }

        [HtmlAttributeName("Command")]
        public string Command { get; set; }

    }
}
