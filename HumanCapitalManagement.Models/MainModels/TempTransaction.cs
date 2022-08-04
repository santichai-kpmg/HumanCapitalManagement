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
    public class TempTransaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(20)]
        public string id_card { get; set; }
        [StringLength(250)]
        public string first_name_en { get; set; }
        [StringLength(250)]
        public string last_name_en { get; set; }

        public string candidate_NickName { get; set; }

        public string candidate_prefix_TH { get; set; }

        public string candidate_FNameTH { get; set; }

        public string candiate_LNameTH { get; set; }

        [StringLength(50)]
        public string candidate_phone { get; set; }

        public string candidate_Email { get; set; }

        public string candidate_ProfessionalQualification { get; set; }

    
    }
}
