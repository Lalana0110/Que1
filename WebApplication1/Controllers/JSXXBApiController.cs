
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;
using System.Data.Entity;
using System.Linq.Dynamic.Core; // 引入 Dynamic LINQ
using OfficeOpenXml;
using WebApplication1.Filters;
using System.IO;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using WebApplication1.Models.DTOs; // 引入 DTO 命名空间
using System.Data.Entity.SqlServer; // 引入 SqlFunctions，用于 EF 6.x

namespace WebApplication1.Controllers
{
    [ApiAuthorize] 
    public class JSXXBApiController : ApiController
    {
        private readonly SchoolDbContext _dbContext = new SchoolDbContext();

        //教师信息排序
        [HttpGet]
        [Route("api/JSXXBApi/GetTeachers")]
        public IHttpActionResult GetTeachers(
            string keyword = "",    // 搜索关键词（工号或姓名）
            string gender = "",     // 性别筛选
            int page = 1,
            int pageSize = 10,
            string sortBy = "ZGH",
            string sortOrder = "asc")
        {
            try
            {
                // 1. 准备基础查询
                var query = _dbContext.JSXXB.AsQueryable();

                // 2. 动态添加过滤条件
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(t =>
                        (t.ZGH != null && t.ZGH.Contains(keyword)) ||
                        (t.XM != null && t.XM.Contains(keyword))
                    );
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    query = query.Where(t => t.XB == gender);
                }

                // 3. 计算总记录数
                var totalCount = query.Count();

                // 4. 修复排序逻辑：ZGH字段特殊处理为数值排序
                IQueryable<JSXXB> sortedQuery;

                if (sortBy.Equals("ZGH", StringComparison.OrdinalIgnoreCase))
                {
                    // 使用LINQ在内存中排序
                    var list = query.ToList(); // 先获取数据到内存
                    //升序
                    if (sortOrder.ToLower() == "asc")
                    {
                        sortedQuery = list.OrderBy(t =>
                        {
                            if (int.TryParse(t.ZGH, out int zghNum))
                                return zghNum;
                            return int.MaxValue; // 非数字排到最后
                        }).AsQueryable();
                    }
                    //降序
                    else
                    {
                        sortedQuery = list.OrderByDescending(t =>
                        {
                            if (int.TryParse(t.ZGH, out int zghNum))
                                return zghNum;
                            return int.MinValue; // 非数字排到最前
                        }).AsQueryable();
                    }
                }
                else
                {
                    // 其他字段使用标准字符串排序
                    sortedQuery = query.OrderBy($"{sortBy} {sortOrder}");
                }

                // 5. 执行分页和投影
                var result = sortedQuery
                    //分页
                    .Skip((page - 1) * pageSize)                    
                    .Take(pageSize)
                    .Select(t => new
                    {
                        ZGH = t.ZGH,
                        XM = t.XM,
                        XB = t.XB,
                        CSRQ = t.CSRQ,
                        LXDH = t.LXDH,
                        ZZMM = t.ZZMM,
                        MZ = t.MZ
                    }).ToList();

                return Ok(new
                {
                    data = result,// 实际数据
                    totalCount, // 总记录数
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    currentPage = page,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTeachers Fatal Error: {ex}");
                return InternalServerError(ex);
            }
        }

        // 添加新增教师接口
        [HttpPost]
        [Route("api/JSXXBApi/Add")]
        public IHttpActionResult AddTeacher([FromBody] JSXXB newTeacher)
        {
            try
            {
                var existing = _dbContext.JSXXB.FirstOrDefault(t => t.ZGH == newTeacher.ZGH);
                if (existing != null)
                {
                    return Content(HttpStatusCode.BadRequest, new { Message = "职工号已存在" });
                }
                _dbContext.JSXXB.Add(newTeacher);
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // 添加删除教师接口
        [HttpPost]
        [Route("api/JSXXBApi/Delete")]
        public IHttpActionResult DeleteTeacher([FromBody] string zgh)
        {
            try
            {
                var teacher = _dbContext.JSXXB.FirstOrDefault(t => t.ZGH == zgh);
                if (teacher == null)
                {
                    return NotFound();
                }

                _dbContext.JSXXB.Remove(teacher);
                _dbContext.SaveChanges();

                return Ok(new { Success = true, Message = "删除成功" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // 编辑教师信息接口
        [HttpPost]
        [Route("api/JSXXBApi/Update")]
        public IHttpActionResult UpdateTeacher([FromBody] JSXXB updatedTeacher)
        //[FromBody] ASP.NET Web API 从 HTTP 请求的正文（Body）​ 中获取数据
        {
            try
            {
                var existingTeacher = _dbContext.JSXXB
                    .FirstOrDefault(t => t.ZGH == updatedTeacher.ZGH);

                if (existingTeacher == null)
                {
                    return NotFound();
                }

                existingTeacher.CSRQ = updatedTeacher.CSRQ;
                existingTeacher.XB = updatedTeacher.XB;
                existingTeacher.LXDH = updatedTeacher.LXDH;
                existingTeacher.ZZMM = updatedTeacher.ZZMM;
                existingTeacher.MZ = updatedTeacher.MZ;

                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is DbEntityValidationException dbEx)
                {
                    var errorMessages = dbEx.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    return BadRequest("数据校验失败：" + fullErrorMessage);
                }
                return InternalServerError(ex);
            }
        }

        // 添加导出Excel接口
        [HttpGet]
        [Route("api/JSXXBApi/ExportExcel")]
        public IHttpActionResult ExportExcel()
        {
            try
            {
                //设置许可证
                ExcelPackage.License.SetNonCommercialPersonal("My Name");

                var teachers = _dbContext.JSXXB
                    .Select(t => new
                    {
                        职工号 = t.ZGH,
                        姓名 = t.XM,
                        性别 = t.XB,
                        出生日期 = t.CSRQ,
                        联系电话 = t.LXDH,
                        政治面貌 = t.ZZMM,
                        民族 = t.MZ
                    })
                    .ToList();
                //创建excel文件和工作表
                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("教师信息");

                    // 添加表头
                    string[] headers = { "职工号", "姓名", "性别", "出生日期", "联系电话", "政治面貌", "民族" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    // 填充数据
                    for (int i = 0; i < teachers.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = teachers[i].职工号;
                        worksheet.Cells[i + 2, 2].Value = teachers[i].姓名;
                        worksheet.Cells[i + 2, 3].Value = teachers[i].性别;

                        // 强制设置 Excel 单元格显示格式为 yyyy-MM-dd
                        if (DateTime.TryParse(teachers[i].出生日期, out DateTime dateValue))
                        {
                            worksheet.Cells[i + 2, 4].Value = dateValue;
                            worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "yyyy-MM-dd";
                        }
                        else
                        {
                            worksheet.Cells[i + 2, 4].Value = teachers[i].出生日期;
                        }

                        worksheet.Cells[i + 2, 5].Value = teachers[i].联系电话;
                        worksheet.Cells[i + 2, 6].Value = teachers[i].政治面貌;
                        worksheet.Cells[i + 2, 7].Value = teachers[i].民族;
                    }
                    //自动调整列宽
                    worksheet.Cells.AutoFitColumns();

                    var fileBytes = package.GetAsByteArray();

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(fileBytes)
                    };
                    result.Content.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = $"教师信息_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                        };
                    result.Content.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    //返回文件下载
                    return ResponseMessage(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ----------------- 功能 2: 导入功能 -----------------

        // 2.1 下载导入模板
        [HttpGet]
        [Route("api/JSXXBApi/DownloadTemplate")]
        public IHttpActionResult DownloadTemplate()
        {
            try
            {
                ExcelPackage.License.SetNonCommercialPersonal("My Name");
                using (var package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("教师导入模板");

                    string[] headers = { "职工号(必填)", "姓名(必填)", "性别", "出生日期(yyyy-MM-dd)", "联系电话", "政治面貌", "民族" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = headers[i];
                    }

                    // 强制设置模板中日期列的 Excel 格式
                    ws.Column(4).Style.Numberformat.Format = "yyyy-MM-dd";

                    ws.Cells["D1"].AddComment("请使用 'yyyy-MM-dd' 格式填写", "系统");

                    ws.Cells.AutoFitColumns();

                    var fileBytes = package.GetAsByteArray();

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(fileBytes)
                    };
                    result.Content.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = $"教师信息导入模板_{DateTime.Now:yyyyMMdd}.xlsx"
                        };
                    result.Content.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                    return ResponseMessage(result);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // 2.2 导入文件接口
        [HttpPost]
        [Route("api/JSXXBApi/ImportExcel")]
        public async Task<IHttpActionResult> ImportExcel()
        {
            //检查是否为文件上传请求
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }
            //设置文件保存路径
            var root = HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);
            //接收上传的文件
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                
                var resultDto = new ImportResultDto { Details = new List<ImportDetailDto>() };

                //数据处理逻辑
                foreach (var file in provider.FileData)
                {
                    var localFileName = file.LocalFileName;

                    using (var stream = new FileStream(localFileName, FileMode.Open))
                    {
                        ExcelPackage.License.SetNonCommercialPersonal("My Name");
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            int rowCount = worksheet.Dimension.Rows;
                            //遍历excel每行
                            for (int row = 2; row <= rowCount; row++)
                            {
                                var detail = new ImportDetailDto();
                                string zghStr = null;

                                try
                                {
                                    // 1. 读取关键字段
                                    zghStr = worksheet.Cells[row, 1].GetValue<string>()?.Trim() ?? string.Empty;
                                    string xm = worksheet.Cells[row, 2].GetValue<string>()?.Trim() ?? string.Empty;

                                    //验证必填字段
                                    if (string.IsNullOrWhiteSpace(zghStr) || string.IsNullOrWhiteSpace(xm))
                                    {
                                        detail.ZGH = zghStr;
                                        detail.XM = xm;
                                        throw new Exception("职工号或姓名不能为空。");
                                    }

                                    detail.ZGH = zghStr;
                                    detail.XM = xm;

                                    // 2. 读取其他数据
                                    string xb = worksheet.Cells[row, 3].GetValue<string>()?.Trim();
                                    string csrqRaw = worksheet.Cells[row, 4].GetValue<string>()?.Trim();
                                    string lxdh = worksheet.Cells[row, 5].GetValue<string>()?.Trim();
                                    string zzmm = worksheet.Cells[row, 6].GetValue<string>()?.Trim();
                                    string mz = worksheet.Cells[row, 7].GetValue<string>()?.Trim();

                                    string csrqFormatted = null;

                                    // 3. 校验 CSRQ 格式
                                    if (!string.IsNullOrEmpty(csrqRaw))
                                    {
                                        if (DateTime.TryParse(csrqRaw, out DateTime parsedDate))
                                        {
                                            csrqFormatted = parsedDate.ToString("yyyy-MM-dd");
                                        }
                                        else if (double.TryParse(csrqRaw, out double excelDateValue))
                                        {
                                            try
                                            {
                                                var dateFromSerial = DateTime.FromOADate(excelDateValue);
                                                csrqFormatted = dateFromSerial.ToString("yyyy-MM-dd");
                                            }
                                            catch
                                            {
                                                throw new Exception($"出生日期无法识别为有效日期。原始值: {csrqRaw}");
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception($"出生日期格式错误，请使用 yyyy-MM-dd 格式。原始值: {csrqRaw}");
                                        }
                                    }
                                    //判断是新增还是更新
                                    var existingTeacher = _dbContext.JSXXB.FirstOrDefault(t => t.ZGH == zghStr);

                                    if (existingTeacher == null)
                                    {
                                        // 新增
                                        var newTeacher = new JSXXB
                                        {
                                            ZGH = zghStr,
                                            XM = xm,
                                            XB = xb,
                                            CSRQ = csrqFormatted,
                                            LXDH = lxdh,
                                            ZZMM = zzmm,
                                            MZ = mz
                                        };
                                        _dbContext.JSXXB.Add(newTeacher);
                                        detail.Status = "新增";
                                    }
                                    else
                                    {
                                        // 更新
                                        existingTeacher.XM = xm;
                                        if (!string.IsNullOrEmpty(xb)) existingTeacher.XB = xb;
                                        if (!string.IsNullOrEmpty(csrqFormatted)) existingTeacher.CSRQ = csrqFormatted;
                                        if (!string.IsNullOrEmpty(lxdh)) existingTeacher.LXDH = lxdh;
                                        if (!string.IsNullOrEmpty(zzmm)) existingTeacher.ZZMM = zzmm;
                                        if (!string.IsNullOrEmpty(mz)) existingTeacher.MZ = mz;

                                        _dbContext.Entry(existingTeacher).State = EntityState.Modified;
                                        detail.Status = "更新";
                                    }

                                    _dbContext.SaveChanges();
                                    detail.IsSuccess = true;
                                    detail.Message = detail.Status + "成功";
                                    resultDto.SuccessCount++;

                                }
                                catch (Exception ex)
                                {
                                    detail.IsSuccess = false;
                                    detail.Status = "失败";
                                    detail.Message = ex.Message;
                                    resultDto.FailCount++;
                                }
                                finally
                                {
                                    resultDto.Details.Add(detail);
                                }
                            }
                        }
                    }
                }

                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                // 清理上传的临时文件
                foreach (var file in provider.FileData)
                {
                    if (File.Exists(file.LocalFileName))
                    {
                        File.Delete(file.LocalFileName);
                    }
                }
            }
        }

        // ----------------- 功能 3: 批量编辑功能 -----------------
        [HttpPost]
        [Route("api/JSXXBApi/BatchUpdate")]
        public IHttpActionResult BatchUpdate([FromBody] BatchUpdateDto dto)
        {
            if (dto == null || dto.ZghList == null || dto.ZghList.Count == 0)
            {
                return BadRequest("请选择需要批量修改的记录。");
            }

            if (dto.UpdateFields == null || dto.UpdateFields.Count == 0)
            {
                return BadRequest("请至少选择一个要修改的字段。");
            }

            try
            {
                // ZghList 是 string 列表，直接使用 string 匹配
                var teachersToUpdate = _dbContext.JSXXB.Where(t => dto.ZghList.Contains(t.ZGH)).ToList();

                if (!teachersToUpdate.Any())
                {
                    return Content(HttpStatusCode.NotFound, new { Message = "未找到任何匹配的教师记录进行批量修改。" });
                }

                foreach (var teacher in teachersToUpdate)
                {
                    if (dto.UpdateFields.ContainsKey("XB"))
                    {
                        teacher.XB = dto.UpdateFields["XB"];
                    }
                    if (dto.UpdateFields.ContainsKey("ZZMM"))
                    {
                        teacher.ZZMM = dto.UpdateFields["ZZMM"];
                    }
                    if (dto.UpdateFields.ContainsKey("MZ"))
                    {
                        teacher.MZ = dto.UpdateFields["MZ"];
                    }
                    _dbContext.Entry(teacher).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                return Ok(new { Success = true, Message = $"成功批量更新 {teachersToUpdate.Count} 条记录。" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}