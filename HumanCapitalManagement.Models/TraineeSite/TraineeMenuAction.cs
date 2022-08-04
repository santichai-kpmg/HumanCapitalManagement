using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.TraineeSite
{
    public class TraineeMenuAction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual TraineeMenu TraineeMenu { get; set; }
        [StringLength(500)]
        public string Controller { get; set; }
        [StringLength(500)]
        public string Action { get; set; }
        [StringLength(10)]
        public string ACTIVE_FLAG { get; set; }
        public DateTime? CREATED_DT { get; set; }
        [StringLength(50)]
        public string CREATED_USER { get; set; }
        public DateTime? UPDATE_DT { get; set; }
        [StringLength(50)]
        public string UPDATE_USER { get; set; }

        public int Action_Type { get; set; }
    }
}
