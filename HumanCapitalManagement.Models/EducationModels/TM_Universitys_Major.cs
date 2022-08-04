using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.EducationModels
{
    public class TM_Universitys_Major
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(350)]
        public string universitys_major_name_en { get; set; }
        [StringLength(350)]
        public string universitys_major_name_th { get; set; }
        [StringLength(500)]
        public string universitys_major_description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Universitys_Faculty TM_Universitys_Faculty { get; set; }
        public virtual TM_Major TM_Major { get; set; }
    }
}
