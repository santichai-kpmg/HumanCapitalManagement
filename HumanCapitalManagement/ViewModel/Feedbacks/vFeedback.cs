using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.Feedbacks
{
 
    public class vFeedback : CSearchFeedback
    {
        public List<vFeedback_obj> lstData { get; set; }
    }

    public class vFeedback_Return : CResutlWebMethod
    {
        public vFeedback_obj_save maindata { get; set; }
        public List<vFeedback_obj_save> lstData { get; set; }
        public List<vFeedback_obj_save> lstDataRe { get; set; }
    }
    public class vFeedback_obj
    {

        public int Id { get; set; }
        public string Positive { get; set; }
        public string Strength { get; set; }  
        public string Need_Improvement { get; set; }
        public string Recommendations { get; set; }
        public int? Rate { get; set; }
        public string Type { get; set; }
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        public int? Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        public int? Update_User { get; set; }
        public string Status { get; set; }
        public string Given_User { get; set; }
        public string Request_User { get; set; }


        public string Fullname { get; set; }
        public string Group { get; set; }


        public string Approve_User { get; set; }

        public DateTime? Given_Date { get; set; }
        public DateTime? Request_Date { get; set; }
        public DateTime? Approve_Date { get; set; }

        public string statusvalue { get; set; }

    }


    public class vFeedback_obj_save : CResutlWebMethod
    {
        public int Id { get; set; }
        public string Edit { get; set; }
        public string Icon { get; set; }
        public string Positive { get; set; }
        public string Strength { get; set; }
        public string Need_Improvement { get; set; }
        public string Recommendations { get; set; }
        public string Rate { get; set; }
        public string Type { get; set; }
        public string Active_Status { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }
        public string Status { get; set; }
        public string Given_User { get; set; }
        public string Request_User { get; set; }
        public string Fullname { get; set; }
        public string Group { get; set; }
        public string Approve_User { get; set; }

        public string Given_Date { get; set; }
        public string Request_Date { get; set; }
        public string Approve_Date { get; set; }
        public string statusvalue { get; set; }


        public string Mode { get; set; }

  
    }

    
}