using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.EducationModels
{
    public class TM_Faculty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(350)]
        public string faculty_name_en { get; set; }
        [StringLength(350)]
        public string faculty_name_th { get; set; }
        [StringLength(500)]
        public string faculty_description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
    }
}
