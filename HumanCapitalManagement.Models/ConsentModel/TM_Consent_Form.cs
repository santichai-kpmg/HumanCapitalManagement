using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.ConsentModel
{
    public class TM_Consent_Form
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
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
        public virtual ICollection<TM_Consent_Question> TM_Consent_Question { get; set; }

    }
}
