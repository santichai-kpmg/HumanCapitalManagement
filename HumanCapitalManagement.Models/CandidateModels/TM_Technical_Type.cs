using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.CandidateModels
{
    public class TM_Technical_Type
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string technical_type_code { get; set; }
        [StringLength(250)]
        public string technical_type_name_th { get; set; }
        [StringLength(250)]
        public string technical_type_name_en { get; set; }
        [StringLength(250)]
        public string technical_type_short_name_th { get; set; }
        [StringLength(250)]
        public string technical_type_short_name_en { get; set; }
        [StringLength(500)]
        public string technical_type_description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public int? seq { get; set; }
    }
}
