using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.PESMain
{
    public class PTR_Evaluation_Approve
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //group head
        [StringLength(50)]
        public string Req_Approve_user { get; set; }
        public DateTime? Approve_date { get; set; }
        [StringLength(50)]
        public string Approve_user { get; set; }
        [StringLength(10)]
        public string Approve_status { get; set; }
        [MaxLengthAttribute()]
        public string responses { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_PTR_Eva_ApproveStep TM_PTR_Eva_ApproveStep { get; set; }
        public virtual TM_Annual_Rating Annual_Rating { get; set; }
        public virtual PTR_Evaluation PTR_Evaluation { get; set; }
        public virtual ICollection<PTR_Evaluation_Incidents_Score> PTR_Evaluation_Incidents_Score { get; set; }
    }
}
