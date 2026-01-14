using System.Data.Entity;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class SchoolDbContext : DbContext
    {
        // 构造函数：指定连接字符串名称（与Web.config中一致）
        public SchoolDbContext() : base("name=SchoolDbContext")
        {
            // 可选：禁用延迟加载（按需配置）
            this.Configuration.LazyLoadingEnabled = false;
        }

        // 映射JSXXB表的数据集
        public DbSet<JSXXB> JSXXB { get; set; }

        // 添加用户表数据集
        public DbSet<YHB> YHB { get; set; }
    }
}