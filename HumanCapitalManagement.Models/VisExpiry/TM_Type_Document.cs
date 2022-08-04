using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.VisaExpiry
{
    public class TM_Type_Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(10)]
        public string seq { get; set; }
        [StringLength(500)]
        public string type_docname_eng { get; set; }
        [StringLength(500)]
        public string type_docname_th { get; set; }
        [StringLength(50)]
        public string active_status { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string update_date { get; set; }
        public DateTime? update_user { get; set; }
    }
}
