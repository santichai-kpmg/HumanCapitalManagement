using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models._360Feedback.MiniHeart
{
    public class MiniHeart_Main
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
        public int? TM_MiniHeart_Peroid_Id { get; set; }
        [ForeignKey("TM_MiniHeart_Peroid_Id")]
        public virtual TM_MiniHeart_Peroid TM_MiniHeart_Peroid { get; set; }
        //public virtual MiniHeart_Detail MiniHeart_Detail { get; set; }
    }
}
