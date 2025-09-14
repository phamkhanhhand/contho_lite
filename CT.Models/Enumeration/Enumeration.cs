using CT.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Enumeration
{
    /// <summary>
    /// scoope
    /// </summary>
    public static class API_SCOPES
    {
        public const string VIEW = "VIEW";
        public const string CREATE = "CREATE";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
        public const string IMPORT = "IMPORT";
        public const string EXPORT = "EXPORT";
        public const string SEND = "SEND";
        public const string APPROVE = "APPROVE";  
    }



    /// <summary>
    /// NameSpace của các loại
    /// </summary>
    /// phamkhanhhand 22.07.2023
    public enum NameSpace
    {
        [StringValue("CT.Models.Entity")]
        NameSpaceEntity,

        [StringValue("CT.Models")]
        AssemblyEntity,

        [StringValue("CT.BL")]
        NameSpaceBL,

        [StringValue("CT.DL")]
        NameSpaceDL,

        [StringValue("CT.UI.UserControl")]
        NameSpaceUC,

    }


    /// <summary>
    /// Trạng thái data do api trả về
    /// </summary>
    public enum ServiceResult
    {
        Success = 1,
        Fail = 0,
        Authen = -2,
    }



    /// <summary>
    /// Trạng thái data do api trả về
    /// </summary>
    /// phamkhanhhand Dec 29, 2024
    public enum FilterOperator
    {
        Equal = 1,              // Giống (==)
        NotEqual = 2,           // Khác (!=)
        GreaterThan = 3,        // Lớn hơn (>)
        LessThan = 4,           // Nhỏ hơn (<)
        GreaterThanOrEqual = 5, // Lớn hơn hoặc bằng (>=)
        LessThanOrEqual = 6,    // Nhỏ hơn hoặc bằng (<=)
        Contains = 7,           // Chứa (dành cho chuỗi)
        StartsWith = 8,         // Bắt đầu với (dành cho chuỗi)
        EndsWith = 9,            // Kết thúc với (dành cho chuỗi)
        Custom = 10            // Kết thúc với (dành cho chuỗi)
    }

    public enum DataType
    {

        /// <summary>
        /// Kiểu dữ liệu chuỗi ký tự (string)
        /// </summary>
        String = 0,      // Chuỗi (String)

        /// <summary>
        /// Kiểu dữ liệu số nguyên và số thực (int, float, double, long, v.v.)
        /// </summary>
        Number = 1,      // Số (integer, floating point)

        /// <summary>
        /// Kiểu dữ liệu tiền tệ (decimal hoặc float cho tài chính)
        /// </summary>
        Currency = 2,    // Tiền tệ (decimal)


        /// <summary>
        /// Kiểu dữ liệu Boolean (true/false)
        /// </summary>
        Boolean = 4,     // Boolean (true/false)

        /// <summary>
        /// Kiểu dữ liệu ngày tháng (DateTime)
        /// </summary>
        DateTime = 5,    // Ngày tháng (DateTime)

        /// <summary>
        /// Kiểu dữ liệu đối tượng chung (object)
        /// </summary>
        Object = 6,      // Đối tượng (Object)

        /// <summary>
        /// Kiểu dữ liệu danh sách (List, Array, Collection)
        /// </summary>
        Collection = 7,  // Danh sách, Mảng (Array, List, Collection)

        /// <summary>
        /// Kiểu dữ liệu kiểu enum (Enumeration)
        /// </summary>
        Enum = 8,        // Enum (Enumeration)

        /// <summary>
        /// Kiểu dữ liệu kiểu từ điển (Dictionary)
        /// </summary>
        Dictionary = 9,  // Từ điển (Dictionary)

        /// <summary>
        /// Kiểu dữ liệu đối tượng JSON (thường là String hoặc JObject trong một số trường hợp)
        /// </summary>
        Json = 10,       // JSON (String hoặc JObject)

        /// <summary>
        /// Kiểu dữ liệu kiểu null (Nullable)
        /// </summary>
        Null = 11,       // Nullable (Null value)

        /// <summary>
        /// Kiểu dữ liệu byte (byte array)
        /// </summary>
        ByteArray = 12   // Mảng byte (ByteArray)
    }



    /// <summary>
    /// Loại login
    /// </summary>
    /// phamkhanhhand 26.07.2023
    public enum LoginType
    {
        /// <summary>
        /// Login bằng web
        /// </summary>
        MainWeb = 0,

        /// <summary>
        /// Login bằng API
        /// </summary>
        Api = 1,
    }


    /// <summary>
    /// Trạng thái entity
    /// </summary>
    public enum EntityState
    {
        None = 0,
        Add = 1,
        Update = 2,
        Delete = 3,
        Duplicate = 4,
    }


    /// <summary>
    /// Trạng thái entity
    /// </summary>
    public static class EnableFlag
    {
        public const string Enable = "Y";
        public const string Disable = "N"; 
    }


    /// <summary>
    /// Lỗi validate entity
    /// </summary>
    public enum ValidationErrorType : int
    {
        Require = 0,
        Duplicate = 1,
        Compare = 2,
        Custom = 3,
        MaxLenght = 4,
        MaxValue = 5,
        MinValue = 6,
    }


    /// <summary>
    /// Loại query database
    /// </summary>
    public enum QueryType : int
    {
        Paging = 0,
        GetByID = 1,
        Insert = 2,
        Update = 3,
        Delete = 4,
        GetDetailByMasterID = 5,
    }

    /// <summary>
    /// Kết quả trả về khi login
    /// </summary>
    /// phamkhanhhand 13.05.2023
    public enum LoginResult : int
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 0,

        /// <summary>
        /// Tài khoản bị khóa
        /// </summary>
        Inactive = 1,

        /// <summary>
        /// Username/pass không hợp lệ
        /// </summary>
        InputNotValid = 2,

        /// <summary>
        /// Lỗi không xác định
        /// </summary>
        Other = 5,
    }

    /// <summary>
    /// Loại cột
    /// </summary>
    /// phamkhanhhand 16.07.2023
    public enum ColumnType : int
    {

        /// <summary>
        /// Thường
        /// </summary>
        Normal = 0,

        /// <summary>
        /// link
        /// </summary>
        Link = 1,

        /// <summary>
        /// Checkbox
        /// </summary>
        Checkbox = 10,

        /// <summary>
        /// CheckboxRoot trong tree (khác ở chỗ check cha thì check con luôn
        /// </summary>
        CheckboxRoot = 11,

        /// <summary>
        /// Ngày
        /// </summary>
        Date = 2,

        /// <summary>
        /// Enum
        /// </summary>
        ComboboxEnum = 3,


        Rownum = 4,
    }

    /// <summary>
    /// Type cache
    /// </summary>
    /// phamkhanhhand 13.05.2023
    public enum CacheType : int
    {
        /// <summary>
        /// Cache dành cho token
        /// </summary>
        GeneralSetting = 0,

        /// <summary>
        /// Cache dành cho các cái không đặt tên
        /// </summary>
        Other = 1,

    }

    #region Cũng dùng như enume, nhưng là string

    /// <summary>
    /// Tên cache, tập trung ở đây đỡ lẫn
    /// </summary>
    /// phamkhanhhand 13.05.2023
    public static class CacheName
    {
        /// <summary>
        /// Tên token
        /// </summary>
        public static string LoginSession = "loginToken_";

    }


    /// <summary>
    /// title bảng log (nhìn kỹ Log chứ không phải Login nhé)
    /// </summary>
    /// phamkhanhhand 13.05.2023
    public static class LogTitle
    {

        /// <summary>
        /// Title của bảng log khi log title
        /// </summary>
        public static string Token = "Token";

        /// <summary>
        /// Load page base
        /// </summary>
        public static string LoadPage = "LoadPage";

    }

    #endregion



    #region Extendtion for enum

    public static class StringValueAttributeExtend
    {

        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }

    #endregion

}
