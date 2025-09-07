using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 
using CT.Models.Entity;
using CT.BL;
using CT.Models.Enumeration;
using CT.Models.ServerObject;
using CT.ImExReport;


namespace KH.Usermanager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<T> : ControllerBase where T : BaseEntity
    {
        private readonly BaseBL bl = BLFactory.CreateBLByType(typeof(T));


        /// <summary>
        /// Lưu, dùng chung cho update, insert
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public virtual IActionResult Save([FromBody] T entity)
        {
            var rs = new ServiceData();

            if (entity.EntityState == EntityState.Update)
            {
                var rsUpdate = bl.Update<T>(entity);
                rs.Data = rsUpdate;
            }
            else if (entity.EntityState == EntityState.Add)
            {
                var newID = bl.Insert<T>(entity);
                rs.Data = newID;
            }

            return Ok(rs);

        }


        // Phương thức để thêm
        [HttpPost]
        public virtual IActionResult Create([FromBody] T entity)
        {
            var rs = bl.Insert<T>(entity);

            return Ok(rs);
        }

        // Phương thức để sửa
        //[HttpPut("{id}")]
        [HttpPut]
        public virtual IActionResult Update([FromBody] T entity)
        {
            var rs = bl.Update<T>(entity);


            return Ok(rs);

        }
         

        // Phương thức để xóa
        [HttpDelete("{id}")]
        public virtual IActionResult Delete(int id)
        {
            var rs = bl.DeleteByID<T>(id);

            return Ok(rs);

        }


        // Phương thức để lấy theo ID (có thể được override)
        //testok
        [HttpGet("{id}")]
        public virtual IActionResult Get(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rs = bl.GetDetailFormData<T>(id);
            return Ok(rs);
        }
        // Phương thức để lấy tất cả và phân trang
        //testok
        //[HttpGet("list")]//thêm list gọi qua gateway cho dễ
        public virtual IActionResult Get(int page = 1, int pageSize = 10)
        {
            var rs = bl.GetPagingByType<T>(page, pageSize);
            //var rs = bl.GetPagingByType<Employee>(page, pageSize);

            return Ok(rs);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchParam">Search param type json</param>
        /// <returns></returns>
        [HttpGet("SearchPaging")]
        public virtual IActionResult SearchPaging(string searchParam = "", int page = 1, int pageSize = 10)
        {
            var rs = bl.GetPagingByType<T>(page, pageSize, searchParam);
            //var rs = bl.GetPagingByType<Employee>(page, pageSize);

            return Ok(rs);
        }


        [HttpGet("export")]
        public IActionResult DownloadExcel(int page = 1, int pageSize = 10)
        {
            // Gọi service trong thư viện và trả về FileResult
            var fileResult = ImportExportHelper.ExportScreen<T>();
            return fileResult;  // Trả về file Excel
        }

        /// <summary>
        /// Download file mẫu để import
        /// Dowload temp file for importting
        /// </summary>
        /// <returns></returns>
        /// phamkhanhhand Nov 23, 2024
        [HttpGet("downloadTempImport")]
        public IActionResult DownloadTempImport()
        {
            // Gọi service trong thư viện và trả về FileResult
            var fileResult = ImportExportHelper.DonwloadTempFile<T>();
            return fileResult;  // Trả về file Excel
        }

        /// <summary>
        /// Download file mẫu để import
        /// Dowload temp file for importting
        /// </summary>
        /// <returns></returns>
        /// phamkhanhhand Nov 23, 2024
        [HttpPost("import")]
        public   bool Import(IFormFile file, [FromForm] string otherInfo)
        {
            // Gọi service trong thư viện và trả về FileResult
            var lst =  ImportExportHelper.ImportFromScreen<T>(file);


            bl.SaveListToDB<T>(lst);


            return true;  // Trả về file Excel



            //// Xử lý file (lưu vào thư mục)
            //var filePath = Path.Combine("C:\\path_to_save\\", file.FileName);
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}


        }
    }

}