using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PreInternAssessment
{
    public class TM_PInternAssessment_Activities
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string Activities_name_en { get; set; }
        [StringLength(250)]
        public string Activities_name_th { get; set; }
        [StringLength(250)]
        public string Activities_short_name_en { get; set; }
        [StringLength(250)]
        public string Activities_short_name_th { get; set; }
        [MaxLengthAttribute()]
        public string Activities_descriptions { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        [StringLength(10)]
        public string Seq { get; set; }
    }
}
