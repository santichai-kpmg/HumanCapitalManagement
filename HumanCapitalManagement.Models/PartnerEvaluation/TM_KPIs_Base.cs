using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation
{
    public class TM_KPIs_Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string kpi_name_en { get; set; }
        [StringLength(500)]
        public string kpi_description { get; set; }
        [StringLength(250)]
        public string kpi_xlxs { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public int? seq { get; set; }
        [StringLength(10)]
        public string type_of_kpi { get; set; }
        public int? base_min { get; set; }
        public int? base_max { get; set; }
    }
}
