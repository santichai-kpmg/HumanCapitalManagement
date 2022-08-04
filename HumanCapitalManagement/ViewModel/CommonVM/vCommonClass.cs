using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.CommonVM
{
    public class vCommonClass
    {
    }

    public class vSelectBoxGroup
    {
        public string id { get; set; }
        public string name { get; set; }
        public string sgroup { get; set; }
        public string fix { get; set; }

    }
    public class vErrorObject
    {
        public string page { get; set; }
        public string msg { get; set; }

    }
    public class vObjectMail
    {
        public string id { get; set; }
        public string type { get; set; }
        public string emp_no { get; set; }
    }
    public class vObjectMail_Send
    {
        public string mail_from { get; set; }
        public string title_mail_from { get; set; }
        public string mail_to { get; set; }
        public string mail_cc { get; set; }
        public string mail_hide_cc { get; set; }
        public string mail_subject { get; set; }
        public string mail_content { get; set; }
    }

    public class objDatatable_Serversite
    {
        [DefaultValue(0)]
        public int length { get; set; }
        [DefaultValue(0)]
        public int recordsDisplay { get; set; }
        [DefaultValue(0)]
        public int recordsTotal { get; set; }
        [DefaultValue(0)]
        public int start { get; set; }
    }

    public class vObjectMail_Feedback
    {
        public string id { get; set; }
        public string type { get; set; }
        public string emp_no { get; set; }
        public string emp_to { get; set; }
        
    }
}