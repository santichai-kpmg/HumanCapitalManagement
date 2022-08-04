using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.NominationMain
{
    public class PES_Final_Rating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //group head
        [StringLength(50)]
        public string user_no { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual PES_Final_Rating_Year PES_Final_Rating_Year { get; set; }

        public int? Self_TM_Annual_Rating_Id { get; set; }
        [ForeignKey("Self_TM_Annual_Rating_Id")]
        public virtual TM_Annual_Rating Self_TM_Annual_Rating { get; set; }
        public int? Final_TM_Annual_Rating_Id { get; set; }
        [ForeignKey("Final_TM_Annual_Rating_Id")]
        public virtual TM_Annual_Rating Final_TM_Annual_Rating { get; set; }
    }
}
