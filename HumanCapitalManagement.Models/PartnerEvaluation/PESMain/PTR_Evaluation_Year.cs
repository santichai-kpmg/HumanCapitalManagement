using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.PartnerEvaluation.PESMain
{
    public class PTR_Evaluation_Year
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? evaluation_year { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        [StringLength(1500)]
        public string comments { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual ICollection<PTR_Evaluation> PTR_Evaluation { get; set; }
        public byte[] sfile64 { get; set; }
        [StringLength(20)]
        public string sfileType { get; set; }
        [StringLength(250)]
        public string sfile_oldname { get; set; }
        [StringLength(250)]
        public string sfile_newname { get; set; }
        public byte[] actual_sfile64 { get; set; }
        [StringLength(20)]
        public string actual_sfileType { get; set; }
        [StringLength(250)]
        public string actual_sfile_oldname { get; set; }
        [StringLength(250)]
        public string actual_sfile_newname { get; set; }
    }
}
