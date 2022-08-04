﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.Common
{
    public class GroupListPermission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? action_type_permiss { get; set; }

        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }

        public virtual MenuAction MenuAction { get; set; }
        public virtual GroupPermission GroupPermission { get; set; }
    }
}
