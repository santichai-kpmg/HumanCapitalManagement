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
    [Table("TBmst_BUPlanner")]
    public class BUPlanner
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Planner ID")]
        public long Id { get; set; }

        public string Planner { get; set; }

        public string CostCenter { get; set; }


        
    }

    
}