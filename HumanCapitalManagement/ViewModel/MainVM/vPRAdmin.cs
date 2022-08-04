using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{
    public class vPRAdmin: CSearchPRAdmin
    {

        public List<vSelect_PR> lstSubGroup { get; set; }
        public List<vSelect_PR> lstPosition { get; set; }
        public List<vPRAdmin_obj> lstData { get; set; }
    }

    public class vPRAdmin_Return : CResutlWebMethod
    {
        public List<vPRAdmin_obj> lstData { get; set; }
    }
    public class vPRAdmin_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string division { get; set; }
        public string sgroup { get; set; }
        public string request_type { get; set; }
        public string position { get; set; }
        public string rank { get; set; }
        public string hc { get; set; }
        public string pr_status { get; set; }
        public string pr_status_id { get; set; }
        public string bu_interview { get; set; }
        public string select { get; set; }
        public string hiring { get; set; }
        public string strat_date { get; set; }
        public string request_by { get; set; }
        public string request_date { get; set; }
        public string no_select { get; set; }
        public string no_accept { get; set; }
        public string sub_group { get; set; }
        public string refno { get; set; }
        public string approve_date { get; set; }
        public string pr_status_seq { get; set; }
        public int? any_changes { get; set; }
    }
    public class vPRAdmin_obj_view
    {
        public string request_status { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string group_id { get; set; }
        public string sub_group_id { get; set; }
        public string sub_group { get; set; }
        public string sub_group_head { get; set; }
        public string employment_type_id { get; set; }
        public string request_type_id { get; set; }
        public string rank_id { get; set; }
        public string position_id { get; set; }
        public string target_start { get; set; }
        public string target_end { get; set; }
        public string no_of_head { get; set; }
        public string name_en { get; set; }
        public string short_name_en { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        //for replec
        public string replaced_user { get; set; }
        public string user_no { get; set; }
        public string user_name { get; set; }
        public string user_position { get; set; }
        public string unit_name { get; set; }
        public string job_descriptions { get; set; }
        public string qualification_experience { get; set; }
        //approver
        public List<vPRAdmin_lst_approve> lstApprove { get; set; }
        //plan
        public string FY_plan { get; set; }
        public string FY_plan_title { get; set; }
        public string cur_headcount { get; set; }

        //approve status
        public string app_step_name { get; set; }
        public string app_user_name { get; set; }
        public string app_remark { get; set; }
        public string app_mode { get; set; }
        public string status_name { get; set; }
        public string cancel_remark { get; set; }
        public string reject_remark { get; set; }
        public List<vPRAdmin_lstData> lstCandidates { get; set; }

    }

    public class vPRAdmin_lstData
    {
        public string Id { get; set; }
        public string Edit { get; set; }
        public string View { get; set; }
        public string candidate_name { get; set; }
        public string active_status { get; set; }
        public string candidate_status { get; set; }
        public string owner_name { get; set; }
        public string rank { get; set; }
        public string remark { get; set; }
        public string action_date { get; set; }
    }
    public class objPRAdmin_Return : CResutlWebMethod
    {

    }
    public class vPRAdmin_Approve_Permit
    {
        public string Edit { get; set; }
        public string emp_code { get; set; }
        public string emp_id { get; set; }
        public string emp_name { get; set; }
        public string emp_group { get; set; }
        public string emp_position { get; set; }

        public string emp_dec { get; set; }
    }
    public class vPRAdmin_lst_approve
    {
        public int nStep { get; set; }
        public string IdEncrypt { get; set; }
        public string step_name { get; set; }
        public string emp_no { get; set; }
        public string app_code { get; set; }
        public string app_name { get; set; }
        public string description { get; set; }
        public string approve_date { get; set; }
    }
    public class vPRAdmin_popup_detail
    {
        public string IdEncrypt { get; set; }
        public string sMode { get; set; }
        public List<vPRAdmin_lstData> lstCandidates { get; set; }
    }
}