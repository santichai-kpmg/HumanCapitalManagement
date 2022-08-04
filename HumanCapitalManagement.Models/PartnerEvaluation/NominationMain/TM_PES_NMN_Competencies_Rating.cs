﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.NominationMain
{
    public class TM_PES_NMN_Competencies_Rating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string competencies_score_en { get; set; }
        [StringLength(250)]
        public string competencies_score_short_en { get; set; }
        [StringLength(500)]
        public string description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public int seq { get; set; }
    }
}
