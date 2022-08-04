using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.PESMain
{
    public class PTR_Evaluation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        [MaxLengthAttribute()]
        public string comments { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        //Main Data
        [StringLength(50)]
        public string user_no { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
        [Required]
        public virtual PTR_Evaluation_Year PTR_Evaluation_Year { get; set; }
        public virtual TM_PTR_Eva_Status TM_PTR_Eva_Status { get; set; }

        //public virtual TM_Annual_Rating Self_Annual_Rating { get; set; }

        //public virtual TM_Annual_Rating Final_Annual_Rating { get; set; }

        public virtual ICollection<PTR_Evaluation_Answer> PTR_Evaluation_Answer { get; set; }
        public virtual ICollection<PTR_Evaluation_Incidents> PTR_Evaluation_Incidents { get; set; }
        public virtual ICollection<PTR_Evaluation_KPIs> PTR_Evaluation_KPIs { get; set; }
        public virtual ICollection<PTR_Evaluation_File> PTR_Evaluation_File { get; set; }
        public virtual ICollection<PTR_Evaluation_Approve> PTR_Evaluation_Approve { get; set; }
        public virtual ICollection<PTR_Feedback_Emp> PTR_Feedback_Emp { get; set; }
        public virtual ICollection<PTR_Feedback_UnitGroup> PTR_Feedback_UnitGroup { get; set; }
        public virtual ICollection<PTR_Evaluation_Authorized> PTR_Evaluation_Authorized { get; set; }
        public virtual ICollection<PTR_Evaluation_AuthorizedEva> PTR_Evaluation_AuthorizedEva { get; set; }
        [StringLength(500)]
        public string other_roles { get; set; }

    }
}
