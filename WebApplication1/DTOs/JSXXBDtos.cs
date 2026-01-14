//创建一个专门的 DTO 文件来存放数据传输对象（ImportResultDto, BatchUpdateDto 等）。
using System.Collections.Generic;

namespace WebApplication1.Models.DTOs
{
    // === 导入相关 DTO ===

    public class ImportResultDto
    {
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public List<ImportDetailDto> Details { get; set; }
    }

    public class ImportDetailDto
    {
        public string ZGH { get; set; }
        public string XM { get; set; }
        public bool IsSuccess { get; set; }
        public string Status { get; set; } // "新增", "更新", "失败"
        public string Message { get; set; } // 错误原因
    }

    // === 批量编辑 DTO ===

    public class BatchUpdateDto
    {
        public List<string> ZghList { get; set; }
        // 键是字段名 (如 XB, ZZMM)，值是新的值
        public Dictionary<string, string> UpdateFields { get; set; }
    }
}