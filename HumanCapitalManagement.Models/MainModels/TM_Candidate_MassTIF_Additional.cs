using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_Candidate_MassTIF_Additional
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(1000)]
        public string other_answer { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Candidate_MassTIF TM_Candidate_MassTIF { get; set; }
        public virtual TM_Additional_Questions TM_Additional_Questions { get; set; }
        public virtual ICollection<TM_Candidate_MassTIF_Adnl_Answer> TM_Candidate_MassTIF_Adnl_Answer { get; set; }
    }
}
