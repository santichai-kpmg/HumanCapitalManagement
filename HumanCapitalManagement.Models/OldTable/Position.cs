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
    [Table("TBtran_Position")]
    public class Position
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Position ID")]
        public long Id { get; set; }

        public string PositionTitle { get; set; }

        public string Rank { get; set; }

        public DateTime TargetPeriod { get; set; }

        public DateTime TargetPeriodTo { get; set; }

        public int Headcount { get; set; }

        public string Specify { get; set; }

        public int Headcount1 { get; set; }

        public int Headcount2 { get; set; }

        public int Headcount3 { get; set; }

        public int Headcount4 { get; set; }

        public int Headcount5 { get; set; }

        public int Headcount6 { get; set; }

        public int Headcount7 { get; set; }
        
        [Required]
        [Column("EmpTypeID")]
        public virtual EmployeeType EmpType { get; set; }

        [Required]
        [Column("PRFormID")]
        public virtual PRForm PRForm { get; set; }

        public Position() {
            TargetPeriod = DateTime.Now;
            TargetPeriodTo = DateTime.Now;
        }

    }
}