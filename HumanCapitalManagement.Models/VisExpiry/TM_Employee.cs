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
    public class TM_Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(100)]
        public string em_code { get; set; }
        public int? TM_Prefix_Id { get; set; }
        [ForeignKey("TM_Prefix_Id")]
        public virtual TM_Prefix_Visa TM_Prefix_Visa { get; set; }
        [StringLength(500)]
        public string em_name_eng { get; set; }
        [StringLength(500)]
        public string em_name_th { get; set; }
        [StringLength(500)]
        public string em_middle { get; set; }
        [StringLength(500)]
        public string em_lastname_eng { get; set; }
        [StringLength(500)]
        public string em_lastname_th { get; set; }
        [StringLength(500)]
        public string remark { get; set; }
        [StringLength(500)]
        public string em_telephone { get; set; }
        [StringLength(500)]
        public string em_mail { get; set; }
        [StringLength(50)]
        public string active_status { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        [StringLength(50)]
        public string family_group { get; set; }
    

    }
}
