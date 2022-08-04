using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Common
{
    public class UserPermission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual GroupPermission GroupPermission { get; set; }
        [StringLength(50)]
        public string user_no { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
        [StringLength(500)]
        public string User_description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual ICollection<UserListPermission> UserListPermission { get; set; }
        public virtual ICollection<UserUnitGroup> UserUnitGroup { get; set; }

    }
}
