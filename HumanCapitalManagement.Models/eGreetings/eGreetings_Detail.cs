using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.eGreetings
{
    public class eGreetings_Detail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(2)] //1:น้อยกว่า 2:เท่ากัน 3:มากกว่า
        public string Rank { get; set; }
        [StringLength(10)] 
        public string Emp_No { get; set; }
        [StringLength(1000)]
        public string Reason { get; set; }
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(10)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(10)]
        public string Update_User { get; set; }
        public int? eGreetings_Main_Id { get; set; }
        [ForeignKey("eGreetings_Main_Id")]
        public virtual eGreetings_Main eGreetings_Main { get; set; }
        public int? TM_eGreetings_Question_Id { get; set; }
        [ForeignKey("TM_eGreetings_Question_Id")]
        public virtual TM_eGreetings_Question TM_eGreetings_Question { get; set; }
        [StringLength(1)] 
        public string Show_Name { get; set; }

    }
}
