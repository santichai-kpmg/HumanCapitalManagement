using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.NominationMain
{
    public class PES_Nomination_Competencies
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [MaxLengthAttribute()]
        public string answer { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual PES_Nomination PES_Nomination { get; set; }
        public int? TM_PES_NMN_Competencies_Id { get; set; }
        [ForeignKey("TM_PES_NMN_Competencies_Id")]
        public virtual TM_PES_NMN_Competencies TM_PES_NMN_Competencies { get; set; }
        public int? TM_PES_NMN_Competencies_Rating_Id { get; set; }
        [ForeignKey("TM_PES_NMN_Competencies_Rating_Id")]
        public virtual TM_PES_NMN_Competencies_Rating TM_PES_NMN_Competencies_Rating { get; set; }
    }
}
