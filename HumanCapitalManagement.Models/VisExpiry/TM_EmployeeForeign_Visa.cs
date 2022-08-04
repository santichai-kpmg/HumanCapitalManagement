
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.VisExpiry
{
    public class TM_EmployeeForeign_Visa
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(500)]

        public string EmployeeNo { get; set; }
        public int? TM_Company_Id { get; set; }
        [ForeignKey("TM_Company_Id")]
        public virtual TM_Company_Visa TM_Company_Visa { get; set; }
        public int? TM_Prefix_Id { get; set; }
        [ForeignKey("TM_Prefix_Id")]
        public virtual TM_Prefix_Visa TM_Prefix_Visa { get; set; }
       
        [StringLength(500)]
        public string Employeename_ENG { get; set; }
        [StringLength(500)]
        public string Employeesurname_ENG { get; set; }
        [StringLength(500)]
        public string Employee_Middle_ENG { get; set; }
        [StringLength(500)]
        public string Employeename_TH { get; set; }
        [StringLength(500)]
        public string Employeesurname_TH { get; set; }
      
        [StringLength(500)]
        public string Employee_telephone { get; set; }

        [StringLength(500)]
        public string Employee_email { get; set; }
        [StringLength(50)]
        public string active_status { get; set; }
        [StringLength(50)]
        public string staff_No { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string update_user { get; set; }
        [StringLength(50)]
        public string family_group { get; set; }
        
        public int? TM_Remark_Id { get; set; }
        [ForeignKey("TM_Remark_Id")]
        public virtual TM_Remark_Visa TM_Remark_Visa { get; set; }
    }
}
