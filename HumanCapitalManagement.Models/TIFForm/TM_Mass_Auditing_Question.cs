using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.TIFForm
{
    public class TM_Mass_Auditing_Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(1000)]
        public string header { get; set; }
        [StringLength(1000)]
        public string question { get; set; }
        [StringLength(1000)]
        public string answer_guideline { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }

        public string group_question { get; set; }

        public virtual TM_Mass_TIF_Form TM_Mass_TIF_Form { get; set; }
        public virtual TM_Mass_Question_Type TM_Mass_Question_Type { get; set; }
    }
}