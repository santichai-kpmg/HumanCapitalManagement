using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Probation
{
    public class Probation_Details
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Seq { get; set; }
        [StringLength(2)] //Y=pass,N=not pass,O=other
        public string Assessment { get; set; }
        [StringLength(1000)]
        public string Remark { get; set; }
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(10)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(10)]
        public string Update_User { get; set; }
        public int? Probation_Form_Id { get; set; }
        [ForeignKey("Probation_Form_Id")]
        public virtual Probation_Form Probation_Form { get; set; }
        public int? TM_Probation_Question_Id { get; set; }
        [ForeignKey("TM_Probation_Question_Id")]
        public virtual TM_Probation_Question TM_Probation_Question { get; set; }
    }
}
