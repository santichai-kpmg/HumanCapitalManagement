using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models._360Feedback
{
    public class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(1000)]
        public string Positive { get; set; }

        [StringLength(1000)]
        public string Strength { get; set; }

        [StringLength(1000)]
        public string Need_Improvement { get; set; }

        [StringLength(1000)]
        public string Recommendations { get; set; }

        public int? Rate { get; set; }
        
        [StringLength(1)]
        public string Type { get; set; }
        
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(10)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(10)]
        public string Update_User { get; set; }
        [StringLength(10)]
        public string Status { get; set; }
        [StringLength(50)]
        public string Given_User { get; set; }
        public DateTime? Given_Date { get; set; }

        [StringLength(50)]
        public string Request_User { get; set; }
        public DateTime? Request_Date { get; set; }

        [StringLength(50)]
        public string Approve_User { get; set; }
        public DateTime? Approve_Date { get; set; }
    }
}
