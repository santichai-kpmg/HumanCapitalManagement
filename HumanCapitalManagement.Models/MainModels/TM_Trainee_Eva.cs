using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_Trainee_Eva
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
        [StringLength(2500)]
        public string trainee_learned { get; set; }
        [StringLength(2500)]
        public string trainee_done_well { get; set; }
        [StringLength(2500)]
        public string trainee_developmental { get; set; }
        [StringLength(2500)]
        public string incharge_comments { get; set; }
        public DateTime? incharge_update_date { get; set; }
        [StringLength(50)]
        public string incharge_update_user { get; set; }
        [StringLength(10)]
        public string hiring_status { get; set; }
        [StringLength(10)]
        public string approve_type { get; set; }
        [StringLength(10)]
        public string confidentiality_agreement { get; set; }
        //In charge
        [StringLength(50)]
        public string req_incharge_Approve_user { get; set; }
        public DateTime? incharge_Approve_date { get; set; }
        [StringLength(50)]
        public string incharge_Approve_user { get; set; }
        [StringLength(10)]
        public string incharge_Approve_status { get; set; }
        [StringLength(500)]
        public string incharge_Approve_remark { get; set; }
        //Performance Manager
        [StringLength(50)]
        public string req_mgr_Approve_user { get; set; }
        public DateTime? mgr_Approve_date { get; set; }
        [StringLength(50)]
        public string mgr_Approve_user { get; set; }
        [StringLength(10)]
        public string mgr_Approve_status { get; set; }
        [StringLength(500)]
        public string mgr_Approve_remark { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }

        public int? TM_Trainee_HiringRating_Id { get; set; }
        [ForeignKey("TM_Trainee_HiringRating_Id")]
        public virtual TM_Trainee_HiringRating TM_Trainee_HiringRating { get; set; }
        public virtual TM_PR_Candidate_Mapping TM_PR_Candidate_Mapping { get; set; }
        public virtual List<TM_Trainee_Eva_Answer> TM_Trainee_Eva_Answer { get; set; }
        public virtual TM_TraineeEva_Status TM_TraineeEva_Status { get; set; }
    }
}
