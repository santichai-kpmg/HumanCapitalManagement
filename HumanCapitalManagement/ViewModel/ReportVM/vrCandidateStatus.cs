using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ReportVM
{
    public class vrCandidateStatus : CSearchTIFReport
    {
        public List<vSelect_PR> lstSubGroup { get; set; }
        public List<vSelect_PR> lstPosition { get; set; }
        public List<vrCandidateStatus_obj> lstData { get; set; }

    }

    public class vrCandidateStatus_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string group_name { get; set; }
        public string sub_group { get; set; }
        public string name_en { get; set; }
        public string rank { get; set; }
        public string pr_rank { get; set; }
        public string position { get; set; }
        public string status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string tif_result { get; set; }
        public string tif_status { get; set; }
        public string candidate_status { get; set; }
        public string candidate_status_date { get; set; }
        public string refno { get; set; }
        public string pr_type { get; set; }
        public string pr_type_id { get; set; }
        public string first_eva { get; set; }
        public string first_eva_date { get; set; }
        public string second_eva { get; set; }
        public string second_eva_date { get; set; }
        public string hr_owner { get; set; }
        public string hr_ac { get; set; }
        public string hr_acdate { get; set; }
        public string pr_status { get; set; }
        public string pr_status_id { get; set; }
    }
    public class vrCandidateStatus_Return : CResutlWebMethod
    {
        public List<vrCandidateStatus_obj> lstData { get; set; }
    }
    public class vrCandidateStatus_Session
    {
        public List<vrCandidateStatus_obj> lstData { get; set; }
    }
}