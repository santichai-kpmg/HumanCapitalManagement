using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_Education_History
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal? grade { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        [StringLength(500)]
        public string education_history_description { get; set; }

        [StringLength(10)]
        public string Ref_Cert_ID { get; set; }

        [StringLength(50)]
        public  string Degree  { get; set; }
        //[StringLength(10)]
        //public string Chronological_Id { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Universitys_Major TM_Universitys_Major { get; set; }
        public virtual TM_Candidates TM_Candidates { get; set; }
        public virtual TM_Education_Degree TM_Education_Degree { get; set; } 
    }
}
