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
    [Table("TBmst_Division")]
    public class Division
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Division ID")]
        public long Id { get; set; }

        public string BU { get; set; }

        public string DivisionName { get; set; }
        public string devision_code { get; set; }

        public int? Sort { get; set; }
        
        public virtual ICollection<SubTeam> SubTeams { get; set; }

        public Division()
        {
            SubTeams = new HashSet<SubTeam>();
        }

        public bool haveSubDivision()
        {
            if (SubTeams.Count() > 0)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}