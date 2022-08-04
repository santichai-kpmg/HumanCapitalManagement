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
    [Table("TBmst_Status")]
    public class Status
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Status ID")]
        public long Id { get; set; }

        public string StatusDesc { get; set; }

        public int Sort { get; set; }

        public virtual ICollection<PRForm> PRForms { get; set; }

        public Status()
        {
            PRForms = new HashSet<PRForm>();
        }
    }
}