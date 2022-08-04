using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.eGreetings
{
    public class eGreetings_Main
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(10)]
        public string Emp_No { get; set; }
        public int Remaining_Rights { get; set; }
        [StringLength(10)]
        public string Status { get; set; }
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(10)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(10)]
        public string Update_User { get; set; }
        [StringLength(1000)]
        public string Remark { get; set; }
        public int? TM_eGreetings_Peroid_Id { get; set; }
        [ForeignKey("TM_eGreetings_Peroid_Id")]
        public virtual TM_eGreetings_Peroid TM_eGreetings_Peroid { get; set; }
        //public virtual eGreetings_Detail eGreetings_Detail { get; set; }
    }
}
