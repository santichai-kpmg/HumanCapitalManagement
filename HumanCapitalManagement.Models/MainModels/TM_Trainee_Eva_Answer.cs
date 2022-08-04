using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_Trainee_Eva_Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(1000)]
        public string inchage_comment { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Trainee_Eva TM_Trainee_Eva { get; set; }
        public virtual TM_Eva_Rating trainee_rating { get; set; }
        public virtual TM_Eva_Rating inchage_rating { get; set; }
        public virtual TM_Evaluation_Question TM_Evaluation_Question { get; set; }
    }
}
