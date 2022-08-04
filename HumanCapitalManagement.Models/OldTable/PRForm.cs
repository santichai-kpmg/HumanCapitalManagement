using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data;
using System.Globalization;

namespace HumanCapitalManagement.Models.OldTable
{
    [Table("TBtran_PRForm")]
    public class PRForm
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "PR ID")]
        public long Id { get; set; }

        [Required]
        [Column("RequestTypeID")] 
        public virtual RequestType RequestType { get; set; }

        [Required]
        public string RefNo { get; set; }

        [Required]
        public string CompanyCode { get; set; }

        [Required]
        public string Division { get; set; }

        public string Remark { get; set; }

        //[Required]
        //public DateTime RequireDate { get; set; } 

        //public string RankID { get; set; }

        //public string Gender { get; set; }

        //public int Age { get; set; }

        //public string ReplaceEmployee { get; set; }


        public string JobDescription { get; set; }

        //[Required]
        //[Column("EducationID")] 
        //public virtual Education Education { get; set; }

        public string EducationDesc { get; set; }


        //public string QualificationExperience { get; set; }

        //[Required]
        //public int HeadCount { get; set; }

        //[Required]
        //[Column("TimeNeedID")]
        //public virtual TimeNeed TimeNeed { get; set; }

        [Required]
        [Column("StatusID")]
        public virtual Status Status { get; set; }

        //[Column("LanguageScoreID")]
        //public virtual LanguageScore LanguageScore { get; set; }

        public string Language { get; set; }

        [Required]
        public string RequestBy { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        public string BUApproveBy { get; set; }

        public DateTime? BUApproveDate { get; set; }

        public string HeadApproveBy { get; set; }

        public DateTime? HeadApproveDate { get; set; }

        public string CeoApproveBy { get; set; }

        public DateTime? CeoApproveDate { get; set; }

        public string rejectReason { get; set; }
        
        public virtual ICollection<Candidate> Candidates { get; set; }

        public virtual ICollection<Position> Positions { get; set; }

        public virtual ICollection<AttachFile> AttachFiles { get; set; }

        public PRForm()
        {
            Candidates = new HashSet<Candidate>();
            Positions = new HashSet<Position>();
            AttachFiles = new HashSet<AttachFile>();
        }
    

        public bool isCEOApprove()
        {
            if (!string.IsNullOrEmpty(CeoApproveBy))
            {
                if (CeoApproveDate == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public Position getPosition()
        {
            List<Position> posLst = Positions.ToList();
            if (posLst.Count() > 0)
            {
                return posLst[0];
            }
            else
            {
                return new Position();
            }            
        }

        public string getInterviewName()
        {
            if (Candidates.Count() > 0)
            {
                string temp = "";
                int count = 1;
                List<Candidate> canLst = Candidates.Where(x => x.HiringProcess == null).ToList();
                foreach (Candidate can in canLst)
                {
                    temp += count + ") " + can.CandidateName + "\n";
                    count++;
                }
                return temp;
            }
            else
            {
                return "";
            }
            
        }

    }
}