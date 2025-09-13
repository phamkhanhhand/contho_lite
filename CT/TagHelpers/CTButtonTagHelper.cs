using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CT.TagHelpers
{

    [HtmlTargetElement("ct-button")]
    public class CTButtonTagHelper : CTBaseTagHelper
    {
        // Thuộc tính "Text" để hiển thị văn bản trên nút
        public string Text { get; set; }

        // Thuộc tính "CssClass" để thêm lớp CSS vào nút
        public string CssClass { get; set; }

        // Thuộc tính "Type" để xác định loại button (submit, button, reset)
        public string Type { get; set; } = "button"; // Mặc định là "button"

        // Thuộc tính "Id" để chỉ định id cho button
        public string Id { get; set; }

        // Thuộc tính "Disabled" để vô hiệu hóa nút nếu cần
        public bool Disabled { get; set; }

        // Thuộc tính "OnClick" để thêm hành động JavaScript vào sự kiện click
        public string OnClick { get; set; }

        // Phương thức xử lý thẻ <button> khi được render ra
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            // Thiết lập tên thẻ là <button>
            output.TagName = "button";

            // Thiết lập thuộc tính "type" cho button
            output.Attributes.SetAttribute("type", Type);

            // Thiết lập thuộc tính "id" cho button nếu có
            if (!string.IsNullOrEmpty(Id))
            {
                output.Attributes.SetAttribute("id", Id);
            }

            // Thiết lập lớp CSS cho button
            output.Attributes.SetAttribute("class", CssClass ?? "btn btn-primary");

            // Nếu thuộc tính "Disabled" là true, thêm thuộc tính disabled
            if (Disabled)
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }

            // Nếu có thuộc tính "OnClick", thêm sự kiện onclick cho button
            if (!string.IsNullOrEmpty(OnClick))
            {
                output.Attributes.SetAttribute("onclick", OnClick);
            }

            // Đặt nội dung văn bản của button từ thuộc tính "Text"
            output.Content.SetContent(Text);



            #region Mapping

            // Thêm script và CSS vào cuối nếu cần
            AddScriptAndCss(output);


            string mapping = @"
<script>

//  window.onload = function () 
{ 
    if (typeof App == 'undefined') {
    App = [];
    }

 
    let temp = new KHButton(" + htmlIDStr + @");
    App[" + htmlIDStr + @"] = temp;

};

</script> ";

            output.PostElement.AppendHtml(mapping);


            #endregion

        }

        private void AddScriptAndCss(TagHelperOutput output)
        {
            // Thêm CSS cho input (ví dụ: dùng thư viện CSS như Bootstrap)
            output.PostElement.AppendHtml("<link rel='stylesheet' href='/css/ct-button.css' />");

            // Thêm script (ví dụ: validate input số, ngày tháng)
            output.PostElement.AppendHtml("<script src='/js/control/ct-button.js'></script>");
        }
    }

}
