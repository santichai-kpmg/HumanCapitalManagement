using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{

    public class vPRCandidates : CSearchPRCandidates
    {
        public List<vSelect_PR> lstSubGroup { get; set; }
        public List<vSelect_PR> lstPosition { get; set; }
        public List<vSelect_PR> lstrank { get; set; }
        public List<vPRCandidates_obj> lstData { get; set; }
    }

    public class vPRCandidates_Return : CResutlWebMethod
    {
        public List<vPRCandidates_obj> lstData { get; set; }
        public List<vPRCandidates_lstData> lstCandidates { get; set; }
    }
    public class vPRCandidates_obj
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
        public string pr_status_id { get; set; }
        public string bu_interview { get; set; }
        public string select { get; set; }
        public string hiring { get; set; }
        public string strat_date { get; set; }
        public string request_by { get; set; }
        public string request_date { get; set; }
        public string sub_group { get; set; }
        public string refno { get; set; }
        public string pr_status_seq { get; set; }
        public string approve_date { get; set; }
        public string no_select { get; set; }
        public string no_accept { get; set; }
    }
    public class vPRCandidates_obj_Save
    {
        public string Session { get; set; }
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
        public string job_descriptions { get; set; }
        public string qualification_experience { get; set; }
        //for replec
        public string replaced_user { get; set; }
        public string user_no { get; set; }
        public string user_name { get; set; }
        public string user_position { get; set; }
        public string unit_name { get; set; }
        //approver
        public List<vPRCandidates_lst_approve> lstApprove { get; set; }
        //plan
        public string FY_plan { get; set; }
        public string FY_plan_title { get; set; }
        public string cur_headcount { get; set; }
        //approve status
        public string status_name { get; set; }
        public List<vPRCandidates_lstData> lstCandidates { get; set; }
        //for update from add candidate page
        public string hr_remark { get; set; }
        public string TIF_type { get; set; }
        public string is_trainee { get; set; }
        public string no_of_eva { get; set; }
        public string pr_status_id { get; set; }
    }
    public class objPRCandidates_Return : CResutlWebMethod
    {

    }
    public class vPRCandidates_lstData
    {
        public string Id { get; set; }
        public string Edit { get; set; }
        public string View { get; set; }
        public string candidate_name { get; set; }
        public string active_status { get; set; }
        public string active_status_id { get; set; }
        public string candidate_status { get; set; }
        public string owner_name { get; set; }
        public string rank { get; set; }
        public string remark { get; set; }
        public string action_date { get; set; }
        public string ref_no { get; set; }
        public string activity { get; set; }
        public List<vPRCandidates_Detail> lstDetail { get; set; }
    }
    public class vPRCandidates_Detail
    {
        public string Id { get; set; }
        public string action_date { get; set; }
        public string action_name { get; set; }
        public string desc { get; set; }
    }

    public class vPRCandidates_Detail_Return : CResutlWebMethod
    {
        public List<vPRCandidates_Detail> lstData { get; set; }
    }
    public class vPRCandidates_lst_approve
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

    public class vCandidateTemp
    {
        public string gender { get; set; }
        public string prenameeng { get; set; }
        public string nameeng { get; set; }
        public string surnameeng { get; set; }
        public string nicknameeng { get; set; }
        public string prenamethai { get; set; }
        public string namethai { get; set; }
        public string surnamethai { get; set; }
        public string nicknamethai { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string birthdate { get; set; }
        public string nationalityname { get; set; }
    }
    #region Upload File
    public class vPRCanReturn_UploadFile : CResutlWebMethod
    {
        public List<vPRCan_obj> lstNewData { get; set; }
        public string IdEncrypt { get; set; }
    }
    public class vPRCan_obj
    {
        public string Edit { get; set; }
        public string sID { get; set; }
        public string sName { get; set; }
        public string sLName { get; set; }
        public string sDuplicate { get; set; }
        public string sIsOld { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string institutename { get; set; }
        public string PresentAddressEng { get; set; }
        public string snickname { get; set; }
        public string phone { get; set; }
        public string activity { get; set; }
    }
    public class vPRCan_FileTemp
    {
        public string IdEncrypt { get; set; }
        public string sName { get; set; }
        public string sLName { get; set; }
        public string snickname { get; set; }
        public int nCandidate_ID { get; set; }
        public string id_card { get; set; }
        public string email { get; set; }
        public string PresentAddressEng { get; set; }
        public string phone { get; set; }
        public string activity { get; set; }

    }

    public class File_Upload_PRCan
    {
        public byte[] sfile64 { get; set; }
        public string sfileType { get; set; }
        public string sfile_name { get; set; }
        public List<vPRCan_FileTemp> lstTempCandidate { get; set; }
    }

    public class vPRCan_file
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string empNo { get; set; }
        public string Session { get; set; }
        public string activity { get; set; }
    }
    #endregion
    #region update many
    public class vPRCan_many
    {
        public string IdEncrypt { get; set; }
        public string candidate_rank_id { get; set; }
        public string action_date { get; set; }
        public string action_remark { get; set; }
        public string recruitment_id { get; set; }
        public string active_status { get; set; }
        public string status_id { get; set; }
        public List<vSelect_PR> lstStatus { get; set; }
        public List<vPRmany_lstData> lstCandidates { get; set; }
    }

    public class SearchManyPR
    {
        [DefaultValue("")]
        public string status_id { get; set; }
        [DefaultValue("")]
        public string IdEncrypt { get; set; }
    }
    public class vPRmany_Return : CResutlWebMethod
    {
        public List<vPRmany_lstData> lstCandidates { get; set; }

        public List<vSelect_PR> lstStatus { get; set; }
    }
    public class vPRmany_lstData
    {
        public string Id { get; set; }
        public string Edit { get; set; }
        public string View { get; set; }
        public string candidate_name { get; set; }
        public string active_status { get; set; }
        public string active_status_id { get; set; }
        public string candidate_status { get; set; }
        public string owner_name { get; set; }
        public string rank { get; set; }
        public string remark { get; set; }
        public string action_date { get; set; }

    }
    #endregion
}