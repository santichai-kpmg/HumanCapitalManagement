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
    [Table("TBmst_RequestType")]
    public class RequestType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Request Type ID")]
        public long Id { get; set; }

        public string RequestTypeDesc { get; set; }

        public int Sort { get; set; }

        public virtual ICollection<PRForm> PRForms { get; set; }

        public RequestType()
        {
            PRForms = new HashSet<PRForm>();
        }
    }
}