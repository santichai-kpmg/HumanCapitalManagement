using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.NominationMain
{
    public class PES_Final_Rating_Year
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? evaluation_year { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        [StringLength(1500)]
        public string comments { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual ICollection<PES_Final_Rating> PES_Final_Rating { get; set; }
       public virtual ICollection<PES_Nomination_KPIs> PES_Nomination_KPIs { get; set; }
    }
}
