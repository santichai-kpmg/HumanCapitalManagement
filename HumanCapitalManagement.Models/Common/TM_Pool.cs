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
    public class TM_Pool
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string Pool_code { get; set; }
        [StringLength(250)]
        public string Pool_name_th { get; set; }
        [StringLength(250)]
        public string Pool_name_en { get; set; }
        [StringLength(250)]
        public string Pool_short_name_th { get; set; }
        [StringLength(250)]
        public string Pool_short_name_en { get; set; }
        [StringLength(500)]
        public string Pool_description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public int? seq { get; set; }
        public virtual ICollection<TM_Divisions> TM_Divisions { get; set; }
        public virtual ICollection<TM_Pool_Approve_Permit> TM_Pool_Approve_Permit { get; set; }
        public virtual ICollection<TM_Pool_Rank> TM_Pool_Rank { get; set; }
      
        public virtual TM_Company TM_Company { get; set; }
    }
}
