using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection.Emit;

namespace CT.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("ct-textbox")]
    public class CTTextboxTagHelper : CTBaseTagHelper
    {
        // Các thuộc tính cần thiết cho input


        ///// <summary>
        ///// Label
        ///// </summary>
        ///// phamkhanhhand 29.03.2023
        //[HtmlAttributeName("text")]


        public string Type { get; set; } = "text";  // Mặc định là 'text', có thể là 'number', 'date', v.v.


        [HtmlAttributeName("Label")]
        public string Label { get; set; }  // Text của label
        public string Placeholder { get; set; }  // Placeholder cho input
        public string Value { get; set; }  // Giá trị mặc định cho input
        public string CssClass { get; set; }  // Thêm lớp CSS cho input

        [HtmlAttributeName("KHSetField")]
        public string KHSetField { get; set; }  // Thêm lớp CSS cho input


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.TagName = "div";  // Sử dụng thẻ div cho phần wrapper
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "ct-textbox " + CssClass);  // Tùy chỉnh class của div

            // Đảm bảo có id cho thẻ input nếu không có
            if (!output.Attributes.ContainsName("id"))
            {
                output.Attributes.SetAttribute("id", htmlID);
            }


            // Tạo thẻ <label> cho input
            if (!string.IsNullOrEmpty(Label))
            {
                var labelTag = new TagBuilder("label");
                labelTag.Attributes.Add("for", "ip_" + htmlID);
                labelTag.InnerHtml.Append(Label);

                output.Content.AppendHtml(labelTag);
            }


            var input = new TagBuilder("input");

            // Cài đặt các thuộc tính cho thẻ input
            //output.TagName = "input";  // Đảm bảo thẻ là <input>
            input.Attributes.Add("type", Type);
            //output.Attributes.SetAttribute("value", Value ?? "");
            input.Attributes.Add("value", Value ?? "");
            input.Attributes.Add("placeholder", Placeholder ?? "");
            input.Attributes.Add("class", CssClass ?? "ct-textbox-input");
            input.Attributes.Add("khsetfield", KHSetField);
            input.Attributes.Add("id", "ip_" + htmlID);

            output.Content.AppendHtml(input);

            #region Mapping

            // Thêm script và CSS vào cuối nếu cần
            AddScriptAndCss(output);


            string mapping = @"
<script>
 
    if (typeof App == 'undefined') {
    App = [];
    }
 
    let temp = new CTTextBox(" + htmlIDStr + @");
    App[" + htmlIDStr + @"] = temp;
   
</script> ";

            output.PostElement.AppendHtml(mapping);


            #endregion
        }

        // Thêm script và CSS nếu có
        private void AddScriptAndCss(TagHelperOutput output)
        {
            // Thêm CSS cho input (ví dụ: dùng thư viện CSS như Bootstrap)
            output.PostElement.AppendHtml("<link rel='stylesheet' href='/css/ct-textbox.css' />");

            // Thêm script (ví dụ: validate input số, ngày tháng)
            //output.PostElement.AppendHtml("<script src='/js/control/ct-textbox.js'></script>");
        }
        //public override void Process(TagHelperContext context, TagHelperOutput output)
        //{

        //}
    }
}
