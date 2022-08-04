﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.eGreetings
{
    public class TM_eGreetings_Peroid
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? Start_Peroid { get; set; }
        public DateTime? End_Peroid { get; set; }
       
        [StringLength(10)]
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        [StringLength(10)]
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        [StringLength(10)]
        public string Update_User { get; set; }
        [StringLength(1000)]
        public string Remark { get; set; }

        

    }
}
