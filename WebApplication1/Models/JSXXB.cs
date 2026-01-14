using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    /// <summary>
    /// 教师表（JSXXB）实体类
    /// </summary>
    [Table("JSXXB")]
    public class JSXXB
    {
        /// <summary>
        /// 主键ID，自增
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 职工号
        /// </summary>
        [Column("ZGH")]
        [StringLength(450)]
        public string ZGH { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("XM")]
        [StringLength(2000)]
        public string XM { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Column("XB")]
        [StringLength(2000)]
        public string XB { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Column("CSRQ")]
        [StringLength(2000)]
        public string CSRQ { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Column("LXDH")]
        [StringLength(2000)]
        public string LXDH { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Column("EMLDZ")]
        [StringLength(2000)]
        public string EMLDZ { get; set; }

        /// <summary>
        /// 教职工类别
        /// </summary>
        [Column("JZGLB")]
        [StringLength(2000)]
        public string JZGLB { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [Column("BM")]
        [StringLength(2000)]
        public string BM { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        [Column("KS")]
        [StringLength(450)]
        public string KS { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        [Column("ZW")]
        [StringLength(2000)]
        public string ZW { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [Column("ZC")]
        [StringLength(2000)]
        public string ZC { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [Column("XL")]
        [StringLength(2000)]
        public string XL { get; set; }

        /// <summary>
        /// 教育经历/进修
        /// </summary>
        [Column("JXYJFX")]
        [StringLength(2000)]
        public string JXYJFX { get; set; }

        /// <summary>
        /// 专业名称
        /// </summary>
        [Column("ZYMC")]
        [StringLength(2000)]
        public string ZYMC { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        [Column("BYYX")]
        [StringLength(2000)]
        public string BYYX { get; set; }

        /// <summary>
        /// 人事主管
        /// </summary>
        [Column("RSZGH")]
        [StringLength(2000)]
        public string RSZGH { get; set; }

        /// <summary>
        /// 教师编
        /// </summary>
        [Column("JSJB")]
        [StringLength(2000)]
        public string JSJB { get; set; }

        /// <summary>
        /// 是否外聘
        /// </summary>
        [Column("SFWP")]
        [StringLength(1)]
        public string SFWP { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        [Column("ZZMM")]
        [StringLength(2000)]
        public string ZZMM { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        [Column("MZ")]
        [StringLength(2000)]
        public string MZ { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        [Column("GZH")]
        [StringLength(450)]
        public string GZH { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("BZ")]
        [StringLength(2000)]
        public string BZ { get; set; }

        /// <summary>
        /// 调动状态
        /// </summary>
        [Column("DQZT")]
        [StringLength(2000)]
        public string DQZT { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [Column("SFZHM")]
        [StringLength(2000)]
        public string SFZHM { get; set; }

        /// <summary>
        /// 所在部门编码
        /// </summary>
        [Column("XZNSBM")]
        [StringLength(2000)]
        public string XZNSBM { get; set; }

        /// <summary>
        /// 教研
        /// </summary>
        [Column("JYZ")]
        [StringLength(200)]
        public string JYZ { get; set; }

        /// <summary>
        /// 录入日期（可空）
        /// </summary>
        [Column("CLRQ")]
        public DateTime? CLRQ { get; set; }

        /// <summary>
        /// 教师卡
        /// </summary>
        [Column("JKAP")]
        [StringLength(50)]
        public string JKAP { get; set; }

        /// <summary>
        /// 是否在职
        /// </summary>
        [Column("SFZB")]
        [StringLength(100)]
        public string SFZB { get; set; }

        /// <summary>
        /// 学院代码
        /// </summary>
        [Column("XLDM")]
        [StringLength(100)]
        public string XLDM { get; set; }

        /// <summary>
        /// 基地代码
        /// </summary>
        [Column("JBDM")]
        [StringLength(100)]
        public string JBDM { get; set; }
    }
}