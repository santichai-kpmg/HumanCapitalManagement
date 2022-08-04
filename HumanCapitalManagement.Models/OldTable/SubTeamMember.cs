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
    [Table("TBmst_SubTeamMember")]
    public class SubTeamMember
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Sub Team Member ID")]
        public long Id { get; set; }

        [Required]
        [Column("SubTeamID")] 
        public virtual SubTeam SubTeam { get; set; }

        public string EmpNo { get; set; }

        public string Rank { get; set; }

        [Required]
        public string CreateBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        //private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        //private DataTable dt;

        //public string getRank()
        //{
        //    dt = wsHRIS.getEmployeeInfoByEmpNo(EmpNo);
        //    return dt.Rows[0]["RankCode"].ToString();
        //}
    }
}