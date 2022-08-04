using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.TraineeSite
{
    public class TraineeMenu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(10)]
        public string MENU_ID { get; set; }
        [StringLength(10)]
        public string MENU_PARENT { get; set; }

        public int MENU_LEVEL { get; set; }
        [StringLength(500)]
        public string MENU_NAME_TH { get; set; }
        [StringLength(500)]
        public string MENU_NAME_EN { get; set; }

        public int MENU_SEQ { get; set; }
        [StringLength(500)]
        public string Controller { get; set; }
        [StringLength(500)]
        public string Action { get; set; }
        [StringLength(250)]
        public string LINK { get; set; }
        public DateTime? CREATED_DT { get; set; }
        [StringLength(50)]
        public string CREATED_USER { get; set; }
        public DateTime? UPDATE_DT { get; set; }
        [StringLength(50)]
        public string UPDATE_USER { get; set; }
        [StringLength(10)]
        public string ACTIVE_FLAG { get; set; }
        [StringLength(10)]
        public string MENU_SUB { get; set; }
        [StringLength(50)]
        public string MENU_ICON { get; set; }
        [StringLength(50)]
        public string MENU_DUMMY { get; set; }
        [StringLength(50)]
        public string active_menu { get; set; }
        [StringLength(10)]
        public string MENU_Permission { get; set; }

        public virtual ICollection<TraineeMenuAction> TraineeMenuAction { get; set; }
    }
}
