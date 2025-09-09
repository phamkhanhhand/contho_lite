using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KH.ITFamily.FileCenter.Base.KHControl
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("kh-combobox")]
    public class KHComboboxTagHelper : KHBaseTagHelper
    {
        // Thuộc tính để chứa danh sách các options
        public List<string> Options { get; set; }

        // Thuộc tính để chứa giá trị đã chọn
        public string SelectedValue { get; set; }

        // Override Process để tạo HTML cho ComboBox
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select"; // Tạo thẻ select

            // Thiết lập các thuộc tính cho thẻ select
            output.Attributes.SetAttribute("class", "combo-box");

            // Tạo các thẻ option từ danh sách Options
            foreach (var option in Options)
            {
                var optionTag = new TagBuilder("option");
                optionTag.InnerHtml.Append(option);

                // Nếu giá trị của option trùng với SelectedValue thì đặt là selected
                if (option == SelectedValue)
                {
                    optionTag.Attributes.Add("selected", "selected");
                }

                output.Content.AppendHtml(optionTag);
            }


            #region Mapping

            // Thêm script và CSS vào cuối nếu cần
            AddScriptAndCss(output);


            string mapping = @"
<script>

    if (typeof App == 'undefined') {
    App = [];
    }
    let temp = new TextBox(" + htmlIDStr + @");
    App[" + htmlIDStr + @"] = temp;

</script> ";

            output.PostElement.AppendHtml(mapping);


            #endregion

        }


        private void AddScriptAndCss(TagHelperOutput output)
        {
            // Thêm CSS cho input (ví dụ: dùng thư viện CSS như Bootstrap)
            output.PostElement.AppendHtml("<link rel='stylesheet' href='/css/kh-combobox.css' />");

            // Thêm script (ví dụ: validate input số, ngày tháng)
            output.PostElement.AppendHtml("<script src='/js/kh-combobox.js'></script>");

        }
    }
}
