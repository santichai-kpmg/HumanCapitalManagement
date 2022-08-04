using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.NominationMain
{
    public class PES_Nomination
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        [MaxLengthAttribute()]
        public string comments { get; set; }
        //detail
        [MaxLengthAttribute()]
        public string DEVELOPMENT_AREAS { get; set; }
        public DateTime? Year_joined_KPMG { get; set; }
        public int? working_with_KPMG { get; set; }
        public int? working_outside_KPMG { get; set; }
        public DateTime? being_AD_Director { get; set; }
        public int? being_ADDirector { get; set; }
        [MaxLengthAttribute()]
        public string Professional_Qualifications { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        //Main Data
        [StringLength(50)]
        public string user_no { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
        [StringLength(500)]
        public string other_roles { get; set; }
        [Required]
        public virtual PES_Nomination_Year PES_Nomination_Year { get; set; }
        public int? TM_PES_NMN_Status_Id { get; set; }
        [ForeignKey("TM_PES_NMN_Status_Id")]
        public virtual TM_PES_NMN_Status TM_PES_NMN_Status { get; set; }
        public int? TM_PES_NMN_Type_Id { get; set; }
        [ForeignKey("TM_PES_NMN_Type_Id")]
        public virtual TM_PES_NMN_Type TM_PES_NMN_Type { get; set; }
        public virtual ICollection<PES_Nomination_Answer> PES_Nomination_Answer { get; set; }
        public virtual ICollection<PES_Nomination_Competencies> PES_Nomination_Competencies { get; set; }
        public virtual ICollection<PES_Nomination_Files> PES_Nomination_Files { get; set; }
        //public virtual ICollection<PES_Nomination_KPIs> PES_Nomination_KPIs { get; set; }
        public virtual ICollection<PES_Nomination_Signatures> PES_Nomination_Signatures { get; set; }

        //public string sSignature()
        //{
        //    string sReturn = "";
        //    if (PES_Nomination_Signatures != null & PES_Nomination_Signatures.Any(a => a.active_status == "Y"))
        //    {
        //        sReturn = string.Join("<br/>", PES_Nomination_Signatures.Where(w => w.active_status == "Y").Select(s => s.bill_no).ToList());
        //    }
        //    return sReturn;
        //}
    }
}
