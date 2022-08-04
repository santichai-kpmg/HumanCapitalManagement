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
    [Table("TBtran_Candidate")]
    public class Candidate
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Candidate ID")]
        public long Id { get; set; }

        [Required]
        [Column("PRFormID")]
        public virtual PRForm PRForm { get; set; }

        [Required]
        public string CandidateName { get; set; }

        [Required]
        public string rank { get; set; }


        public string fullRank { get; set; }


        public string ContactNo { get; set; }

        [Column("InterviewID")]
        public virtual Interview InterviewStatus { get; set; }

        //[Column("OfferID")]
        //public virtual Offer OfferStatus { get; set; }

        //new start
        public virtual HiringProcess HiringProcess { get; set; }

        //public virtual CandidateStatus CandidateStatus { get; set; }

        public DateTime? AcceptDate { get; set; }
        //new end

        public DateTime? OfferingDate { get; set; }

        public DateTime? DateByBU { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? RejectedDate { get; set; }

        //public DateTime? EndDate { get; set; }

        //[Column("onboardID")]
        //public virtual Onboard OnboardStatus { get; set; }

        public string Remark { get; set; }

        //public DateTime? JoinDate { get; set; }

        public string HROwner { get; set; }

        //public virtual ICollection<LanguageScore> LanguageScores { get; set; }

        public Candidate()
        {
            //LanguageScores = new HashSet<LanguageScore>();
        }


        public string OfferingDateDesc()
        {
            if (OfferingDate != null)
            {
                return ((DateTime)OfferingDate).ToString("dd-MMM-yyyy");
            }
            else
            {
                return string.Empty;
            }
        }

        public string DateByBUDesc()
        {
            if (DateByBU != null)
            {
                return ((DateTime)DateByBU).ToString("dd-MMM-yyyy");
            }
            else
            {
                return string.Empty;
            }
        }

        public string AcceptDateDesc()
        {
            if (AcceptDate != null)
            {
                return ((DateTime)AcceptDate).ToString("dd-MMM-yyyy");
            }
            else
            {
                return string.Empty;
            }
        }

        public string StartDateDesc()
        {
            if (StartDate != null)
            {
                return ((DateTime)StartDate).ToString("dd-MMM-yyyy");
            }
            else
            {
                return string.Empty;
            }
        }

        //public string EndDateDesc()
        //{
        //    if (EndDate != null)
        //    {
        //        return ((DateTime)EndDate).ToString("dd-MMM-yyyy");
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        //public string JoinDateDesc()
        //{
        //    if (JoinDate != null)
        //    {
        //        return ((DateTime)JoinDate).ToString("dd-MMM-yyyy");
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        public string rejectDateDesc()
        {
            if (RejectedDate != null)
            {
                return ((DateTime)RejectedDate).ToString("dd-MMM-yyyy");
            }
            else
            {
                return string.Empty;
            }
        }

    }
}