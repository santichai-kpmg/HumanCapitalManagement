using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Probation
{
    public class Action_Plans
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Seq { get; set; }
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(50)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(50)]
        public string Update_User { get; set; }
        public byte[] sfile64 { get; set; }
        [StringLength(20)]
        public string sfileType { get; set; }
        [StringLength(250)]
        public string sfile_oldname { get; set; }
        [StringLength(250)]
        public string sfile_newname { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        
        public virtual Probation_Form Probation_Form { get; set; }
    }
}
