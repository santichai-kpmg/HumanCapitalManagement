using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Probation
{
    public class Probation_Form
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(2)] // Y=yes,N=no,O=other
        public string Assessment { get; set; }
        [StringLength(1000)]
        public string Remark { get; set; }

        [StringLength(10)] //S = Submit, A = Acknowledge ,GA = Group Head Approve , HA = HOP Approve
        public string Status { get; set; }
        [StringLength(10)]
        public string Probation_Active { get; set; }
        [StringLength(10)]
        public string Provident_Fund { get; set; }
        [StringLength(10)]
        public string Mail_Send { get; set; }
        public DateTime? Start_Pro { get; set; }
        public DateTime? End_Pro { get; set; }
        public int Count_Date_Pro { get; set; }

        [StringLength(10)]
        public string HR_No { get; set; }
        public DateTime? HR_Submit_Date { get; set; }
        [StringLength(10)]
        public string Staff_No { get; set; }
        public DateTime? Staff_Acknowledge_Date { get; set; }
        [StringLength(10)]
        public string PM_No { get; set; }
        public DateTime? PM_Submit_Date { get; set; }
        [StringLength(10)]
        public string GroupHead_No { get; set; }
        public DateTime? GroupHead_Submit_Date { get; set; }
        [StringLength(10)]
        public string HOP_No { get; set; }
        public DateTime? HOP_Submit_Date { get; set; }

        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(10)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(10)]
        public string Update_User { get; set; }
        [StringLength(100)]
        public string Extend_Form { get; set; }
        [StringLength(2)]
        public string Extend_Status { get; set; }
        [StringLength(2)]
        public string Extend_Period { get; set; }
        public virtual ICollection<Probation_Details> Probation_Details { get; set; }

        [StringLength(1000)]
        public string Remark_Revise { get; set; }
        public DateTime? Send_Mail_Date { get; set; }

        [StringLength(10)]
        public string Staff_Action { get; set; }
        [StringLength(10)]
        public string PM_Action { get; set; }
        [StringLength(10)]
        public string GroupHead_Action { get; set; }
        [StringLength(10)]
        public string HOP_Action { get; set; }

    }
}
