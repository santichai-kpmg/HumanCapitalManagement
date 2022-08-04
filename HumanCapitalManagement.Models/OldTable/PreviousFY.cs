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
    [Table("TBmst_PreviousFY")]
    public class PreviousFY
    {
        [Key, Column(Order = 1)]
        [Required]
        public string EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string EmployeeSurname { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string Pool { get; set; }

        [Required]
        public string UnitGroup { get; set; }

        [Required]
        public string Rank { get; set; }

        [Required]
        public string RankCode { get; set; }

        [Required]
        public string FullRank { get; set; }
        [Key, Column(Order = 2)]
        public int? nYear { get; set; }
   
        public string group_code { get; set; }
    }
}