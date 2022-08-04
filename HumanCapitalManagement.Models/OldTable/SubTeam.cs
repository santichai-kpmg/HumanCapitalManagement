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
    [Table("TBmst_SubTeam")]
    public class SubTeam
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "SubTeam ID")]
        public long Id { get; set; }

        [Required]
        [Column("DivisionID")] 
        public virtual Division Division { get; set; }

        public string SubDivision { get; set; }

        [Required]
        public string CreateBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public virtual ICollection<SubTeamMember> TeamMembers { get; set; }

        public SubTeam()
        {
            TeamMembers = new HashSet<SubTeamMember>();
        }

    }
}