using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_Candidate_PIntern_Mass
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(10)]
        public string hr_acknowledge { get; set; }
        public DateTime? acknowledge_date { get; set; }
        [StringLength(50)]
        public string acknowledge_user { get; set; }
        [StringLength(10)]
        public string submit_status { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        [StringLength(1500)]
        public string comments { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_PIntern_Status TM_PIntern_Status { get; set; }
        [Required]
        public virtual TM_PR_Candidate_Mapping TM_PR_Candidate_Mapping { get; set; }
        public virtual ICollection<TM_Candidate_PIntern_Mass_Answer> TM_Candidate_PIntern_Mass_Answer { get; set; }
        public virtual ICollection<TM_Candidate_PIntern_Mass_Approv> TM_Candidate_PIntern_Mass_Approv { get; set; }

        [StringLength(10)]
        public string confidentiality_agreement { get; set; }
        public virtual TM_Pool_Rank Recommended_Rank { get; set; }
    }
}
