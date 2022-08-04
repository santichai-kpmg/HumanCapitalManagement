using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.NominationMain
{
    public class PES_Nomination_Signatures
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //group head
        [StringLength(50)]
        public string Req_Approve_user { get; set; }
        public DateTime? Approve_date { get; set; }
        [StringLength(50)]
        public string Approve_user { get; set; }
        [StringLength(10)]
        public string Approve_status { get; set; }
        [StringLength(10)]
        public string Agree_Status { get; set; }
        [MaxLengthAttribute()]
        public string responses { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual PES_Nomination PES_Nomination { get; set; }

        public int? TM_PES_NMN_SignatureStep_Id { get; set; }
        [ForeignKey("TM_PES_NMN_SignatureStep_Id")]
        public virtual TM_PES_NMN_SignatureStep TM_PES_NMN_SignatureStep { get; set; }
        public int? TM_Annual_Rating_Id { get; set; }
        [ForeignKey("TM_Annual_Rating_Id")]
        public virtual TM_Annual_Rating TM_Annual_Rating { get; set; }
    }
}
