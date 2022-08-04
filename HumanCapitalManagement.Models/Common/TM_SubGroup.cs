using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Common
{
    public class TM_SubGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string head_user_no { get; set; }
        [StringLength(50)]
        public string head_user_id { get; set; }
        [StringLength(50)]
        public string sub_group_code { get; set; }
        [StringLength(250)]
        public string sub_group_name_th { get; set; }
        [StringLength(250)]
        public string sub_group_name_en { get; set; }
        [StringLength(250)]
        public string sub_group_short_name_th { get; set; }
        [StringLength(250)]
        public string sub_group_short_name_en { get; set; }
        [StringLength(500)]
        public string sub_group_description { get; set; }
        public int? seq { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Divisions TM_Divisions { get; set; }
    }
}
