using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.ConsentModel
{
    public class Consent_Asnwer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(2)] //Y=Yes,N=No
        public string Asnwer { get; set; }
       
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

        public int? Consent_Main_Form_Id { get; set; }
        [ForeignKey("Consent_Main_Form_Id")]
        public virtual Consent_Main_Form Consent_Main_Form { get; set; }

        public int? TM_Consent_Question_Id { get; set; }
        [ForeignKey("TM_Consent_Question_Id")]
        public virtual TM_Consent_Question TM_Consent_Question { get; set; }

    }
}
