using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ConsentForm
{
    public class vConsentForm_View_obj
    {
        public int Id { get; set; }
        public string View { get; set; }
        public string Edit { get; set; }
        public string Name { get; set; }
        public string Create_Date { get; set; }
        public string Active_status { get; set; }
        public string Employee_no { get; set; }
        public string Name_user { get; set; }
        public string Date { get; set; }
    }
    public class vConsentForm_View : CResutlWebMethod
    {
        public List<vConsentForm_View_obj> lstData { get; set; }
        public vConsentForm_View_obj objData { get; set; }
    }
}