using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Common
{
    public class TM_Divisions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string division_code { get; set; }
        [StringLength(250)]
        public string division_name_th { get; set; }
        [StringLength(250)]
        public string division_name_en { get; set; }
        [StringLength(250)]
        public string division_short_name_th { get; set; }
        [StringLength(250)]
        public string division_short_name_en { get; set; }
        [StringLength(500)]
        public string division_description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public int? seq { get; set; }
        public virtual TM_Pool TM_Pool { get; set; }
        public virtual ICollection<TM_UnitGroup_Approve_Permit> TM_UnitGroup_Approve_Permit { get; set; }
        public virtual ICollection<TM_SubGroup> TM_SubGroup { get; set; }
        public virtual ICollection<TM_Position> TM_Position { get; set; }

        [StringLength(50)]
        public string default_grouphead { get; set; }
        [StringLength(50)]
        public string default_practice { get; set; }
        [StringLength(50)]
        public string default_ceo { get; set; }
        [StringLength(1000)]
        public string approve_description { get; set; }
    }
}
