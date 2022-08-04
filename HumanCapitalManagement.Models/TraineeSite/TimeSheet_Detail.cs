using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.TraineeSite
{
    public class TimeSheet_Detail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        public DateTime? date_start { get; set; }
        public DateTime? date_end { get; set; }
        [StringLength(50)]
        public string Engagement_Code { get; set; }
        [StringLength(1000)]
        public string title { get; set; }
        [StringLength(10)]
        public string hours { get; set; }
        [StringLength(1000)]
        public string remark { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? trainee_create_date { get; set; }
        public int? trainee_create_user { get; set; }
        public DateTime? trainee_update_date { get; set; }
        public int? trainee_update_user { get; set; }
        public virtual TimeSheet_Form TimeSheet_Form { get; set; }
        public int? TM_Time_Type_Id { get; set; }
        [ForeignKey("TM_Time_Type_Id")]
        public virtual TM_Time_Type TM_Time_Type { get; set; }
        [StringLength(10)]
        public string submit_status { get; set; }
        [StringLength(10)]
        public string approve_status { get; set; }
        [StringLength(50)]
        public string Approve_user { get; set; }
        [StringLength(1)]
        public string Source_Type { get; set; }

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
