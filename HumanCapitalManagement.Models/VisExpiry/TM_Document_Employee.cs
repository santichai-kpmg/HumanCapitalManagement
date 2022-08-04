using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.VisaExpiry
{
    public class TM_Document_Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(500)]
        public string doc_number { get; set; }
        public DateTime? date_of_issued { get; set; }
        public int? TM_Country_Id { get; set; }
        [ForeignKey("TM_Country_Id")]
        public virtual TM_Country TM_Country { get; set; }
        public DateTime? valid_date { get; set; }
        [StringLength(50)]
        public string active_status { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? create_date { get; set; }

        public DateTime? update_date { get; set; }
        public string update_user { get; set; }
        public int? TM_EmployeeForeign_Visa_Id { get; set; }
        [ForeignKey("TM_EmployeeForeign_Visa_Id")]
        public virtual TM_EmployeeForeign_Visa TM_EmployeeForeign_Visa { get; set; }

        public int? TM_Type_Document_Id { get; set; }
        [ForeignKey("TM_Type_Document_Id")]
        public virtual TM_Type_Document TM_Type_Document { get; set; }

    }
}
