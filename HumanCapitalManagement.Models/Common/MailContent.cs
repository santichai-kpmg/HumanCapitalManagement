using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Common
{
    public class MailContent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? nCId { get; set; }
        [StringLength(50)]
        public string mail_type { get; set; }
        [StringLength(250)]
        public string mail_type_name { get; set; }
        [StringLength(500)]
        public string mail_header { get; set; }
        [StringLength(500)]
        public string sender_name { get; set; }
        [MaxLengthAttribute()]
        public string content { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual ICollection<TM_MailContent_Cc> TM_MailContent_Cc { get; set; }
        public virtual ICollection<TM_MailContent_Cc_bymail> TM_MailContent_Cc_bymail { get; set; }
        public virtual ICollection<TM_MailContent_Param> TM_MailContent_Param { get; set; }
    }
}
