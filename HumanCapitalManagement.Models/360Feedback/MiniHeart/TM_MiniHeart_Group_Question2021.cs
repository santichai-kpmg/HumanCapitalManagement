using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models._360Feedback.MiniHeart2021
{
    public class TM_MiniHeart_Group_Question2021
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Action_date { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(10)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(10)]
        public string Update_User { get; set; }
        public virtual ICollection<TM_MiniHeart_Question2021> TM_MiniHeart_Question { get; set; }
    }
}
