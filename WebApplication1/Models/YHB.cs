using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("YHB")]
    public class YHB
    {
       
        [Key]
        [Column("USERID")]
        public string UserId { get; set; }

        [Column("USERPWD")]
        public string UserPwd { get; set; }

        
    }
}