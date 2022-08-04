using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{

    public class vFYPlan : CSearchFYPlan
    {
        public List<vFYPlan_obj> lstData { get; set; }
    }

    public class vFYPlan_Return : CResutlWebMethod
    {
        public List<vFYPlan_obj> lstData { get; set; }
        public string IdEncrypt { get; set; }
        public string Session { get; set; }
    }

    public class vFYPlanDetail_Return : CResutlWebMethod
    {
        public List<vFYPlan_obj> lstData { get; set; }
        public string IdEncrypt { get; set; }
        public string Session { get; set; }
    }



    public class vFYPlan_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }

        public string fy_year { get; set; }

        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        
    }

    //        public string fy_year { get; set; } ,  
   

}