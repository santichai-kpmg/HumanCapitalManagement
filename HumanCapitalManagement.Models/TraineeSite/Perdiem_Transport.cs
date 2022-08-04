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
    public class Perdiem_Transport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        public DateTime? date_start { get; set; }
        public DateTime? date_end { get; set; }
        [StringLength(50)]
        public string Engagement_Code { get; set; }
        [StringLength(1000)]
        public string remark { get; set; }
        [StringLength(5)]
        public string Type_of_withdrawal { get; set; }
        [StringLength(100)]
        public string Company { get; set; }
        [StringLength(10)]
        public string Reimbursable { get; set; }
        [StringLength(1000)]
        public string Business_Purpose { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? trainee_create_date { get; set; }
        public int? trainee_create_user { get; set; }
        public DateTime? trainee_update_date { get; set; }
        public int? trainee_update_user { get; set; }
        [StringLength(10)]
        public string submit_status { get; set; }
        [StringLength(10)]
        public string approve_status { get; set; }
        [StringLength(50)]
        public string Approve_user { get; set; }
        [StringLength(1)]
        public string Source_Type { get; set; }

        public int? TM_PR_Candidate_Mapping_Id { get; set; }
        [ForeignKey("TM_PR_Candidate_Mapping_Id")]
        public virtual TM_PR_Candidate_Mapping TM_PR_Candidate_Mapping { get; set; }

        public DateTime? submit_date { get; set; }
        public DateTime? approve_date { get; set; }

        public DateTime? review_date { get; set; }
        [StringLength(50)]
        public string review_user { get; set; }
        
        public DateTime? paid_date { get; set; }
        [StringLength(50)]
        public string paid_user { get; set; }


    }
}
