using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.eGreetings
{
    public class TM_eGreetings_Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Seq { get; set; }
        public int Group { get; set; }
        [StringLength(1000)]
        public string Topic { get; set; }
        [MaxLengthAttribute()]
        public string Content { get; set; }
        [StringLength(10)]
        public string Use { get; set; }
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(10)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(10)]
        public string Update_User { get; set; }

        public string Icon { get; set; }

        public virtual TM_eGreetings_Group_Question TM_eGreetings_Group_Question { get; set; }
    }
}
