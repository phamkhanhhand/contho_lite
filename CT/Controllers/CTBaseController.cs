using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CT.Models.Entity;
using CT.BL;
using CT.Models.Enumeration;
using CT.Models.ServerObject;
using CT.ImExReport;
using CT.Auth;


namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class CTBaseController<T> : ControllerBase where T : BaseEntity
    {
        private readonly BaseBL bl = BLFactory.CreateBLByType(typeof(T));


        /// <summary>
        /// save = update/insert
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Permission(uri: null, API_SCOPES.UPDATE, API_SCOPES.CREATE)]
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

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(uri: null, API_SCOPES.CREATE)]
        public virtual IActionResult Create([FromBody] T entity)
        {
            var rs = bl.Insert<T>(entity);

            return Ok(rs);
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        [Permission(uri: null, API_SCOPES.UPDATE)]
        public virtual IActionResult Update([FromBody] T entity)
        {
            var rs = bl.Update<T>(entity);


            return Ok(rs);

        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission(uri: null, API_SCOPES.DELETE)]
        public virtual IActionResult Delete(int id)
        {
            var rs = bl.DeleteByID<T>(id);

            return Ok(rs);

        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Permission(uri: null, API_SCOPES.VIEW)]
        public virtual IActionResult Get(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rs = bl.GetDetailFormData<T>(id);
            return Ok(rs);
        }

          
        /// <summary>
        /// search paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchParam">Search param type json</param>
        /// <returns></returns>
        [Permission(uri: null, API_SCOPES.VIEW)]
        [HttpGet("search")]
        public virtual IActionResult SearchPaging(string searchParam = "", int page = 1, int pageSize = 10)
        {
            var rs = bl.GetPagingByType<T>(page, pageSize, searchParam);
            //var rs = bl.GetPagingByType<Employee>(page, pageSize);

            return Ok(rs);
        }


        /// <summary>
        /// Export
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("export")]
        [Permission(uri: null, API_SCOPES.EXPORT)]
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
        [HttpGet("download-template-import-file")]
        [Permission(uri: null, API_SCOPES.IMPORT)]
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
        [Permission(uri: null, API_SCOPES.IMPORT)]
        public bool Import(IFormFile file, [FromForm] string otherInfo)
        {
            // Gọi service trong thư viện và trả về FileResult
            var lst = ImportExportHelper.ImportFromScreen<T>(file);


            bl.SaveListToDB<T>(lst);


            return true;  // Trả về file Excel


        }
    }

}