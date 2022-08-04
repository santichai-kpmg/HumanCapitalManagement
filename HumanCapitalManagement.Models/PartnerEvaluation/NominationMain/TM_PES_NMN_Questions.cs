using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.NominationMain
{
    public class TM_PES_NMN_Questions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(300)]
        public string stype { get; set; }
        [StringLength(3000)]
        public string stype_desc { get; set; }
        [StringLength(300)]
        public string qgroup { get; set; }
        [StringLength(300)]
        public string header { get; set; }
        [StringLength(3000)]
        public string question { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        [StringLength(10)]
        public string questions_type { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }

    }
}
