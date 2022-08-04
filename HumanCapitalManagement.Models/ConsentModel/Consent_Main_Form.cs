using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.ConsentModel
{
    public class Consent_Main_Form
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(1000)]
        public string Employee_no { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(50)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(50)]
        public string Update_User { get; set; }
        public int? TM_Consent_Form_Id { get; set; }
        [ForeignKey("TM_Consent_Form_Id")]
        public virtual TM_Consent_Form TM_Consent_Form { get; set; }
        public virtual ICollection<Consent_Asnwer> Consent_Asnwer { get; set; }

    }
}
