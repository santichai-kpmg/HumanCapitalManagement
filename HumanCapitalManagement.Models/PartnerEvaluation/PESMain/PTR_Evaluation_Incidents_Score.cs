using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.PESMain
{
    public class PTR_Evaluation_Incidents_Score
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [MaxLengthAttribute()]
        public string answer { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public int? PTR_Evaluation_Approve_Id { get; set; }
        [ForeignKey("PTR_Evaluation_Approve_Id")]
        public virtual PTR_Evaluation_Approve PTR_Evaluation_Approve { get; set; }
        public int? TM_PTR_Eva_Incidents_Id { get; set; }
        [ForeignKey("TM_PTR_Eva_Incidents_Id")]
        public virtual TM_PTR_Eva_Incidents TM_PTR_Eva_Incidents { get; set; }
        public int? TM_PTR_Eva_Incidents_Score_Id { get; set; }
        [ForeignKey("TM_PTR_Eva_Incidents_Score_Id")]
        public virtual TM_PTR_Eva_Incidents_Score TM_PTR_Eva_Incidents_Score { get; set; }
    }
}
