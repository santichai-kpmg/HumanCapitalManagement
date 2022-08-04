using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{
    public class vPersonnelRequest : CSearchPersonnelRequest
    {
        public List<vPersonnelRequest_obj> lstData { get; set; }
    }

    public class vPersonnelRequest_Return : CResutlWebMethod
    {
        public List<vPersonnelRequest_obj> lstData { get; set; }
    }
    public class vPersonnelRequest_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
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
        public string bu_interview { get; set; }
        public string select { get; set; }
        public string hiring { get; set; }
        public string strat_date { get; set; }
        public string request_by { get; set; }
        public string request_date { get; set; }
        public string pr_status_id { get; set; }
        public string pr_status_seq { get; set; }
        public string sub_group { get; set; }
        public string refno { get; set; }
    }
    public class vPersonnelRequest_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string group_id { get; set; }
        public string sub_group_id { get; set; }
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
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public List<vSelect_PR> lstSubGroup { get; set; }
        public List<vSelect_PR> lstTypeReq { get; set; }
        public List<vSelect_PR> lstrank { get; set; }
        public List<vSelect_PR> lstPosition { get; set; }

        //for replec
        public string replaced_user { get; set; }
        public string user_id { get; set; }
        public string user_no { get; set; }
        public string user_name { get; set; }
        public string user_position { get; set; }
        public string unit_name { get; set; }

        public string job_descriptions { get; set; }
        public string qualification_experience { get; set; }
        //approver
        public List<vPersonnelApprover_obj> lstApprove { get; set; }
        public List<vPersonnelApprover_obj> lstApprove_save { get; set; }
        public string remark_group_approve { get; set; }
        //plan
        public string FY_plan { get; set; }
        public string FY_plan_title { get; set; }
        public string cur_headcount { get; set; }

        public string TIF_type { get; set; }
    }
    public class objPersonnelRequest_Return : CResutlWebMethod
    {

    }
    public class vPersonnelRequest_Approve_Permit
    {
        public string Edit { get; set; }
        public string emp_code { get; set; }
        public string emp_id { get; set; }
        public string emp_name { get; set; }
        public string emp_group { get; set; }
        public string emp_position { get; set; }

        public string emp_dec { get; set; }
    }
    public class vPersonnelApprover_obj
    {
        public int nStep { get; set; }
        public string IdEncrypt { get; set; }
        public string step_name { get; set; }
        public string create_ddl { get; set; }
        public string emp_no { get; set; }
        public string app_code { get; set; }
        public string app_name { get; set; }
        public string description { get; set; }
        public string approve_date { get; set; }
        public List<lstDataSelect> lstselect { get; set; }
        public string default_value { get; set; }

    }

    #region for select on change
    public class vSelect_PR
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class vSelect_Activity
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class vGroup_onchange
    {
        public string sub_group_id { get; set; }
        public string rank_id { get; set; }
        public string position_id { get; set; }
        public List<vSelect_PR> lstSubGroup { get; set; }
        public List<vSelect_PR> lstrank { get; set; }
        public List<vSelect_PR> lstPosition { get; set; }
        //approver
        public List<vPersonnelApprover_obj> lstApprove { get; set; }
        public string FY_plan { get; set; }
        public string cur_headcount { get; set; }
        public string remark_group_approve { get; set; }
    }
    public class vEmployment_onchange
    {
        public string request_type_id { get; set; }
        public List<vSelect_PR> lstTypeReq { get; set; }
    }
    public class vPosition_onchange
    {
        public string job_descriptions { get; set; }
        public string qualification_experience { get; set; }
    }
    public class vRequestType_onchange
    {
        public string replaced_user { get; set; }
    }
    #endregion

}