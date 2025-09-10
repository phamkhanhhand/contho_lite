using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection.Emit;

namespace CT.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("ct-grid-data")]
    public class CTGridDataTagHelper : CTBaseTagHelper
    {
        // Các thuộc tính cần thiết cho input

         

        public string Type { get; set; } = "text";  // Mặc định là 'text', có thể là 'number', 'date', v.v.


        [HtmlAttributeName("Label")]
        public string Label { get; set; }  // Text của label
         
        [HtmlAttributeName("Height")] 
        public string Height { get; set; }  // Text của label


        //[HtmlAttributeName("Height")]
        //public string Height { get; set; }  // Text của label

         
        //public virtual ITemplate ToolBar { get; set; }
         
        //protected override void CreateChildControls()
        //{
        //    // Remove any controls
        //    this.Controls.Clear();

        //    // Add all content to a container.
        //    if (ToolBar != null)
        //    {
        //        var container = new Control();
        //        this.ToolBar.InstantiateIn(container);

        //        // Add container to the control collection.
        //        this.Controls.Add(container);
        //    }
        //}

        public CTMenuToolTagHelper MenuTool { get; set; }




        [HtmlAttributeName("Columns")]
        public List<KHColumn> Columns { get; set; }



        [HtmlAttributeName("EntityName")]
        public string EntityName { get; set; }

        [HtmlAttributeName("URL")]
        public string URL { get; set; }




        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
  
        }

        // Thêm script và CSS nếu có
        private void AddScriptAndCss(TagHelperOutput output)
        {
            // Thêm CSS cho input (ví dụ: dùng thư viện CSS như Bootstrap)
            output.PostElement.AppendHtml("<link rel='stylesheet' href='/css/ct-textbox.css' />");

            // Thêm script (ví dụ: validate input số, ngày tháng)
            output.PostElement.AppendHtml("<script src='/js/control/ct-textbox.js'></script>");
        }
        //public override void Process(TagHelperContext context, TagHelperOutput output)
        //{

        //}
    }


    public class KHColumn
    {
        public string DataIndex { get; set; }
        public string HeaderName { get; set; }

        public int ColumnType { get; set; }

        /// <summary>
        /// Chiều rộng cột
        /// </summary>
        public float? Width { get; set; }


        /// <summary>
        /// Hàm render (không biết có vào script không nhỉ)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual string Render(object value)
        {
            return (value != null ? value.ToString() : string.Empty);
        }

    }

    /// <summary>
    /// Dạng combobox
    /// </summary>
    public class KHComboBoxEnumColumn : KHColumn
    {

        /// <summary>
        /// Trường để gán dữ liệu vào giao diện
        /// </summary>
        /// phamkhanhhand
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string EnumName { get; set; }


        /// <summary>
        /// Trường để gán dữ liệu vào giao diện
        /// </summary>
        /// phamkhanhhand
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Url;

    }


    public class KHLinkColumn : KHColumn
    {
        public KHLinkColumn()
        {
            ColumnType = 1;//link
        }

        /// <summary>
        /// URl
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Param format string url
        /// </summary>
        public string ListParamUrl { get; set; }
    }


    public class KHDateTimeColumn : KHColumn
    {
        public KHDateTimeColumn()
        {
            ColumnType = 2;//datetime
        }

        /// <summary>
        /// Hàm render (không biết có vào script không nhỉ)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string Render(object value)
        {
            DateTime dt;

            if (value != null && DateTime.TryParse(value.ToString(), out dt))
            {
                return dt.ToString("dd/MM/yyyy");
            }

            return null;

        }

    }


}
