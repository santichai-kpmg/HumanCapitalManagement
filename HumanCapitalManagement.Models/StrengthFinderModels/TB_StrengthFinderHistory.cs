using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.StrengthFinderModels
{
    public class TB_StrengthFinderHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmployeeNo { get; set; }
        public virtual TM_StrenghtFinder StrengthName1 { get; set; }
        public virtual TM_StrenghtFinder StrengthName2 { get; set; }
        public virtual TM_StrenghtFinder StrengthName3 { get; set; }
        public virtual TM_StrenghtFinder StrengthName4 { get; set; }
        public virtual TM_StrenghtFinder StrengthName5 { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
