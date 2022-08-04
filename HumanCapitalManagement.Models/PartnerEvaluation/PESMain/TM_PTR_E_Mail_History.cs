using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.PESMain
{
    public class TM_PTR_E_Mail_History
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(10)]
        public string sent_status { get; set; }
        [MaxLengthAttribute()]
        public string descriptions { get; set; }
        [MaxLengthAttribute()]
        public string mail_to { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual MailContent MailContent { get; set; }
        public virtual PTR_Evaluation_Approve PTR_Evaluation_Approve { get; set; }
    }
}
