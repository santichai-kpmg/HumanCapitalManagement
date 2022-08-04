using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public  class TM_WorkExperience
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? seq { get; set; }
        [StringLength(500)]
        public string CompanyName { get; set; }
        [StringLength(500)]
        public string  JobPosition { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [StringLength(10)]
        public string   TypeOfRelatedToJob { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public decimal? base_salary { get; set; }
        public decimal? transportation { get; set; }
        public decimal? mobile_allowance { get; set; }
        public decimal? position_allowance { get; set; }
        public decimal? other_allowance { get; set; }
        public decimal? annual_leave { get; set; }
        public decimal? variable_bonus { get; set; }
        public decimal? expected_salary { get; set; }
        //public int? TM_Candidates_Id { get; set; }

        //[ForeignKey("TM_Candidates_Id")]
        public virtual TM_Candidates TM_Candidates { get; set; }

        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
    }
}
