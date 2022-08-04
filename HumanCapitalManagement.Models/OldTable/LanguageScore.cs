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
    [Table("TBtran_LanguageScore")]
    public class LanguageScore
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Language Score ID")]
        public long Id { get; set; }

        //public virtual Candidate CandidateID { get; set; }

        public string LanguageDesc { get; set; }

        public string Speaking { get; set; }

        public string Writing { get; set; }

        public string Reading { get; set; }

        //public virtual ICollection<Candidate> Candidates { get; set; }

        public LanguageScore()
        {
            //Candidates = new HashSet<Candidate>();
        }
    }
}