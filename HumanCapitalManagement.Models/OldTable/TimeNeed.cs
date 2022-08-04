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
    [Table("TBmst_TimeNeed")]
    public class TimeNeed
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Time Need ID")]
        public long Id { get; set; }

        public string TimeNeedDesc { get; set; }

        public int Sort { get; set; }

        //public virtual ICollection<PRForm> PRForms { get; set; }

        public TimeNeed()
        {
            //PRForms = new HashSet<PRForm>();
        }
    }
}