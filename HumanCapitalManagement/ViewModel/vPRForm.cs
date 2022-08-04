using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    
    public class vPRForm : CSearchPRForm
    {
        public List<vPRFormData> lstData { get; set; }
    }

    public class vPRForm_Result : CResutlWebMethod
    {
        public string session { get; set; }
        public List<vPRFormData> lstData { get; set; }

    }
    public class vPRFormData
    {
        public string division { get; set; }
        public string sgroup { get; set; }
        public string request_type { get; set; }
        public string position { get; set; }
        public string rank { get; set; }
        public string hc { get; set; }
        public string pr_status { get; set; }
        public string bu_interview { get; set; }
        public string select { get; set; }
        public string hiring { get; set; }
        public string strat_date { get; set; }
        public string remark { get; set; }
        public string View { get; set; }
        public string Edit { get; set; }
        public string IdEncrypt { get; set; }
        public string request_by { get; set; }
        public string request_date { get; set; }
    }

}