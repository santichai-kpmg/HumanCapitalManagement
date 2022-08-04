using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    public class vCandidate_tif_approve : CSearchCandidate_tif_approve
    {
        public List<vCandidate_tif_approve_obj> lstData { get; set; }
    }

    public class vCandidate_tif_approve_Return : CResutlWebMethod
    {
        public List<vCandidate_tif_approve_obj> lstData { get; set; }
    }
    public class vCandidate_tif_approve_obj
    {
        public string View { get; set; }
        public string Edit { get; set; }
        public string Id { get; set; }
        public string seq { get; set; }
        public string Req_Approve_user { get; set; }
        public string Approve_date { get; set; }
        public string Approve_user { get; set; }
        public string Approve_status { get; set; }
        public string active_status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
    }


    public class vCandidate_tif_approve_obj_save
    {
        public int Id { get; set; }
        public int? seq { get; set; }
        public string Req_Approve_user { get; set; }
        public DateTime? Approve_date { get; set; }
        public string Approve_user { get; set; }
        public string Approve_status { get; set; }
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        public string update_user { get; set; }

    }
   
}