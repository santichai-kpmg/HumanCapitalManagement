using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Common
{
    public class TM_Pool_Rank
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string Pool_rank_name_th { get; set; }
        [StringLength(250)]
        public string Pool_rank_name_en { get; set; }
        [StringLength(250)]
        public string Pool_rank_short_name_th { get; set; }
        [StringLength(250)]
        public string Pool_rank_short_name_en { get; set; }
        [StringLength(500)]
        public string Pool_rank_description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Pool TM_Pool { get; set; }
        public virtual TM_Rank TM_Rank { get; set; }
    }
}
