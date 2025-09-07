using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CT.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("ct-textbox")]
    public class CTTextBoxTagHelper : TagHelper
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Placeholder { get; set; }
        public string Label { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Tạo div cha
            output.TagName = "div";
             
            output.TagMode = TagMode.StartTagAndEndTag; // <- quan trọng!

            output.Attributes.SetAttribute("class", "form-group");

            // Tạo HTML nội dung
            var labelHtml = $"<label for='{Id}'>{Label}</label>";
            var inputHtml = $"<input type='text' class='form-control' id='{Id}' name='{Name}' value='{Value}' placeholder='{Placeholder}' />";

            output.Content.SetHtmlContent(labelHtml + inputHtml);

            //output.Content.SetContent(inputHtml);
        }
    }
}
