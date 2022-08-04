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
    [Table("TBmst_Education")]
    public class Education
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Education ID")]
        public long Id { get; set; }

        public string EducationDesc { get; set; }

        public int Sort { get; set; }

        //public virtual ICollection<PRForm> PRForms { get; set; }

        public Education()
        {
            //PRForms = new HashSet<PRForm>();
        }
    }
}