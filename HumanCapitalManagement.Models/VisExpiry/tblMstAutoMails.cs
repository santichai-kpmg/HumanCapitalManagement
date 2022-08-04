using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.VisExpiry
{
    public class tblMstAutoMails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(500)]
        public string MailSubject { get; set; }
        [StringLength(1000)]
        public string MailBody { get; set; }
        [StringLength(1000)]
        public string MailType { get; set; }
    
    }
}
