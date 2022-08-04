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
    [Table("TBmst_EmployeeType")]
    public class EmployeeType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "EmployeeType ID")]
        public long Id { get; set; }

        public string EmployeeTypeDesc { get; set; }

        public int Sort { get; set; }

        public virtual ICollection<Position> Positions { get; set; }

        public EmployeeType()
        {
            Positions = new HashSet<Position>();
        }
    }
}