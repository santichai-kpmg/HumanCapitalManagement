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
    [Table("TBmst_CandidateStatus")]
    public class CandidateStatus
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "CandidateStatus ID")]
        public long Id { get; set; }

        public string CandidateStatusDesc { get; set; }

        public int Sort { get; set; }

        //public virtual ICollection<Candidate> Candidates { get; set; }

        public CandidateStatus()
        {
            //Candidates = new HashSet<Candidate>();
        }
    }
}