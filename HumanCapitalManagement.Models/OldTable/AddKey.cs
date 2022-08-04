using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data;
using System.Globalization;

namespace HumanCapitalManagement.Models.OldTable
{   
    [Table("TBmst_AddKey")]
    public class AddKey
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "AddKey ID")]
        public long Id { get; set; }

        public string NameValues { get; set; }

        public string ValueData { get; set; }

    }    
}