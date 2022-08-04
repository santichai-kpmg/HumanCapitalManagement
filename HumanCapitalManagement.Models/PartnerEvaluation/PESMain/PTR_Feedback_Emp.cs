using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.PESMain
{
    public class PTR_Feedback_Emp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual PTR_Evaluation PTR_Evaluation { get; set; }
        public virtual TM_Feedback_Rating TM_Feedback_Rating { get; set; }
        //Main Data
        [StringLength(50)]
        public string user_no { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
        [MaxLengthAttribute()]
        public string appreciations { get; set; }
        [MaxLengthAttribute()]
        public string recommendations { get; set; }

    }
}
