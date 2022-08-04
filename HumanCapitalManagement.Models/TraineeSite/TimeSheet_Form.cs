using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.TraineeSite
{
    public class TimeSheet_Form
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(10)]
        public string hr_acknowledge { get; set; }
        public DateTime? acknowledge_date { get; set; }
        [StringLength(50)]
        public string acknowledge_user { get; set; }
        [StringLength(10)]
        public string submit_status { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        [StringLength(1500)]
        public string comments { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public int? TM_TimeSheet_Status_Id { get; set; }
        [ForeignKey("TM_TimeSheet_Status_Id")]
        public virtual TM_TimeSheet_Status TM_TimeSheet_Status { get; set; }
        public int? TM_PR_Candidate_Mapping_Id { get; set; }
        [ForeignKey("TM_PR_Candidate_Mapping_Id")]
        public virtual TM_PR_Candidate_Mapping TM_PR_Candidate_Mapping { get; set; }
        [StringLength(50)]
        public string req_Approve_user { get; set; }
        public DateTime? Approve_date { get; set; }
        [StringLength(50)]
        public string Approve_user { get; set; }
        [StringLength(10)]
        public string Approve_status { get; set; }
        [StringLength(500)]
        public string Approve_remark { get; set; }
        public virtual ICollection<TimeSheet_Detail> TimeSheet_Detail { get; set; }
        public DateTime? trainee_create_date { get; set; }
        public int? trainee_create_user { get; set; }
        public DateTime? trainee_update_date { get; set; }
        public int? trainee_update_user { get; set; }



    }


}
