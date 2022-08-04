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
    public class TM_Candidate_PIntern_Mass_Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(1000)]
        public string answer { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Candidate_PIntern_Mass TM_Candidate_PIntern_Mass { get; set; }
        public virtual TM_PIntern_Rating TM_PIntern_Rating { get; set; }
        public virtual TM_PIntern_Mass_Question TM_PIntern_Mass_Question { get; set; }
    }
}
