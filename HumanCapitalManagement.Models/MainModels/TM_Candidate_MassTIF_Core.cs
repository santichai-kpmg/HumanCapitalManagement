﻿using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_Candidate_MassTIF_Core
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? seq { get; set; }
        [StringLength(2000)]
        public string evidence { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Candidate_MassTIF TM_Candidate_MassTIF { get; set; }
        public virtual TM_Mass_Scoring TM_Mass_Scoring { get; set; }
        public virtual TM_MassTIF_Form_Question TM_MassTIF_Form_Question { get; set; }
        public int? TM_TIF_Rating_Id { get; set; }
        [ForeignKey("TM_TIF_Rating_Id")]
        public virtual TM_TIF_Rating TM_TIF_Rating { get; set; }
    }
}
