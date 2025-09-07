using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{

    /// <summary>
    /// Khóa học
    /// </summary>
    /// phamkhanhhand 13.04.2023
    public class UserContext : BaseEntity
    {


        public const string C_EmployeeID = "EmployeeID";


        public Guid? CourseID { get; set; }
        public string CourseCode{ get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Status { get; set; }
        public int MaxStudent { get; set; }
        public int MinStudent { get; set; }
        public int RealStudent { get; set; }

        /// <summary>
        /// Giảng viên hiện tại (có thể 1 lớp nhiều giảng viên)
        /// </summary>
        public Guid? TeacherID { get; set; }
    }
}
