using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data;
using System.Globalization;

namespace HumanCapitalManagement.Models.OldTable
{
    [Table("TBmst_AttachFile")]
    public class AttachFile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "AttachFile ID")]
        public long Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
        
        public virtual PRForm PRForm { get; set; }

        public AttachFile()
        {
            
        }
    }
}