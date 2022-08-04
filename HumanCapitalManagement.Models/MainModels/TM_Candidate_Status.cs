using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_Candidate_Status
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string candidate_status_name_th { get; set; }
        [StringLength(250)]
        public string candidate_status_name_en { get; set; }
        [StringLength(500)]
        public string candidate_status_description { get; set; }
        [StringLength(10)]
        public string remark_validate { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public int? seq { get; set; }
        public virtual ICollection<TM_Candidate_Status_Next> TM_Candidate_Status_Next { get; set; }
    }
}
