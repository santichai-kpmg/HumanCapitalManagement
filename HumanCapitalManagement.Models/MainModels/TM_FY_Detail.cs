using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_FY_Detail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_Divisions TM_Divisions { get; set; }
        public virtual TM_FY_Plan TM_FY_Plan { get; set; }
        public decimal? para    { get; set; }
        public decimal? aa  { get; set; }
        public decimal? sr  { get; set; }
        public decimal? am  { get; set; }
        public decimal? mgr  { get; set; }
        public decimal? ad  { get; set; }
        public decimal? dir  { get; set; }
        public decimal? ptr     { get; set; }
    }
}
