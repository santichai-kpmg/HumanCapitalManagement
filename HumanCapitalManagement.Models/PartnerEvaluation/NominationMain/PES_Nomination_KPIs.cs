using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.NominationMain
{
    public class PES_Nomination_KPIs
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
        public int? seq { get; set; }
        [StringLength(1000)]
        public string target { get; set; }
        [StringLength(1000)]
        public string target_max { get; set; }
        [StringLength(1000)]
        public string actual { get; set; }
        [StringLength(1000)]
        public string group_actual { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual PES_Final_Rating_Year PES_Final_Rating_Year { get; set; }
        public int? TM_KPIs_Base_Id { get; set; }
        [ForeignKey("TM_KPIs_Base_Id")]
        public virtual TM_KPIs_Base TM_KPIs_Base { get; set; }
        [StringLength(1000)]
        public string group_target { get; set; }
        [StringLength(1000)]
        public string group_target_max { get; set; }

    }
}
