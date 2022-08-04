using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.PESVM
{

    public class vPTREvaluation : CSearchPTREvaluation
    {
        public List<vPTREvaluation_obj> lstData { get; set; }
    }

    public class vPTREvaluation_Return : CResutlWebMethod
    {
        public List<vPTREvaluation_obj> lstData { get; set; }
        public string IdEncrypt { get; set; }
        public string Session { get; set; }
    }
    public class vPTREvaluation_UploadFile : CResutlWebMethod
    {
        public List<vPTREvaluation_lst_File> lstNewData { get; set; }
        public string IdEncrypt { get; set; }
    }
    public class vPTREvaluation_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string name_th { get; set; }
        public string name_en { get; set; }
        public string name { get; set; }
        public string emp_no { get; set; }
        public string fy_year { get; set; }
        public string short_name_en { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string eva_status { get; set; }
        public string self_eva { get; set; }
        public string final_eva { get; set; }
        public string sgroup { get; set; }
        public string sbu { get; set; }
        public string spractice { get; set; }
        public string sceo { get; set; }
        public string srank { get; set; }
        public string approval { get; set; }
        public string bu_eva { get; set; }
        public string practice_eva { get; set; }
        public string p_d { get; set; }
        public string ceoe_eva { get; set; }
        public string ceoe { get; set; }
        public string ceoe_comment { get; set; }
        public string approve_status { get; set; }
        public string status_id { get; set; }
    }
    public class rpvPTREvaluation_Session
    {
        public List<vPTREvaluation_report> lstData { get; set; }
    }
    public class vPTREvaluation_report
    {

        public string name { get; set; }
        public string emp_no { get; set; }
        public string fy_year { get; set; }
        public string eva_status { get; set; }
        public string self_eva { get; set; }
        public string final_eva { get; set; }
        public string sgroup { get; set; }
        public string p_d { get; set; }
        public string last_update { get; set; }
        public string sbu { get; set; }
        public string spractice { get; set; }
        public string sceo { get; set; }
        public string srank { get; set; }
        public string bu_eva { get; set; }
        public string practice_eva { get; set; }
        public string final_comment { get; set; }
        public string bu_comment { get; set; }
        public string practice_comment { get; set; }

        public string ceoe { get; set; }
        public string ceoe_comment { get; set; }
        public string ceoe_eva { get; set; }

    }

    public class vPTREvaluation_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string sname { get; set; }
        public string sgroup { get; set; }
        public string srank { get; set; }
        public string other_role { get; set; }
        public string action_date { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string Session { get; set; }
        public string file_name { get; set; }
        public string status_name { get; set; }
        public string status_id { get; set; }
        public string eva_mode { get; set; }
        public string yearcurrent { get; set; }
        public string yearone { get; set; }
        public string yeartwo { get; set; }
        public string yearthree { get; set; }
        public string self_rating_id { get; set; }
        public string approve_rating_id { get; set; }
        public string approve_remark { get; set; }
        public string revise_remark { get; set; }
        public string is_ceo { get; set; }
        public string is_update_by_approval { get; set; }
        public string revise_comment { get; set; }

        public List<vPartnerKPIs_obj> lstNewData { get; set; }
        public List<vPartner_obj> lstOldData { get; set; }
        //approver
        public List<vPTREvaluation_lst_approve> lstApprove { get; set; }
        public List<vPTREvaluation_lst_File> lstFile { get; set; }
        public List<vPTREvaluation_KPIs> lstKPIs { get; set; }
        public List<vPTREvaluation_Answer> lstAnswer { get; set; }
        public List<vPTREvaluation_Answer> lstIncidents { get; set; }
        public List<vPTREvaluation_Incidents_Score> lstIncidents_score { get; set; }
        public List<vPTREvaluation_Feedback> lstFeedback { get; set; }
        public List<vPTREvaluation_Final> lstFinal { get; set; }
        public List<vPTREvaluation_lst_Authorized> lstAuthorized { get; set; }
    }
    public class vPTREvaluation_lst_approve
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
    public class vPTREvaluation_lst_File
    {
        public string IdEncrypt { get; set; }
        public string Edit { get; set; }
        public string file_name { get; set; }
        public string description { get; set; }
        public string View { get; set; }
    }

    public class objPTREvaluation_lst_Authorized_Return : CResutlWebMethod
    {
        public List<vPTREvaluation_lst_Authorized> lstData { get; set; }
    }
    public class vPTREvaluation_lst_Authorized
    {
        public string IdEncrypt { get; set; }
        public string Edit { get; set; }
        public string authorized_name { get; set; }
        public string authorized_user { get; set; }
        public string authorized_rank { get; set; }
        public string description { get; set; }
        public string View { get; set; }
    }
    public class vPTREvaluation_KPIs
    {
        public string IdEncrypt { get; set; }
        public string sname { get; set; }
        public string target_data { get; set; }
        public string target_group { get; set; }
        public string howto { get; set; }
        public string remark { get; set; }
        public string actual { get; set; }
        public string group_actual { get; set; }
        public string yearone { get; set; }
        public string yeartwo { get; set; }
        public string yearthree { get; set; }
        public string group_yearone { get; set; }
        public string group_yeartwo { get; set; }
        public string group_yearthree { get; set; }
        public string sdisable { get; set; }
        public string stype { get; set; }
        public string sbu { get; set; }
        public string su { get; set; }
    }
    public class vPTREvaluation_Answer
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string question { get; set; }
        public string header { get; set; }
        public string remark { get; set; }
        public string rating { get; set; }
        public string sgroup { get; set; }
        public string sdisable { get; set; }
        public int? nCID { get; set; }
    }
    public class vPTREvaluation_Incidents_Score
    {
        public int? nSeq { get; set; }
        public int? nstep { get; set; }
        public string id { get; set; }
        public string question { get; set; }
        public string header { get; set; }
        public string sgroup { get; set; }
        public string sExcellent { get; set; }
        public string sMeet { get; set; }
        public string sHigh { get; set; }
        public string sLow { get; set; }
        public string sNI { get; set; }
        public string sdisable { get; set; }
        public string nscore { get; set; }
        public int? nrate { get; set; }
        public string isCurrent { get; set; }
        public string user { get; set; }

    }
    public class vPTREvaluation_Feedback
    {
        public string Edit { get; set; }
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string sname { get; set; }
        public string sgroup { get; set; }
        public string rating { get; set; }
        public string appreciations { get; set; }
        public string recommendations { get; set; }
    }
    public class vPTREvaluation_Final
    {
        public int? nSeq { get; set; }
        public string position { get; set; }
        public string status { get; set; }
        public string remark { get; set; }

    }

    public class File_Upload_PTREvaluation
    {
        public byte[] sfile64 { get; set; }
        public string sfileType { get; set; }
        public string sfile_name { get; set; }
        public List<vPartnerKPIs_FileTemp> lstPartnerKPIs { get; set; }
    }
    public class vPTREvaluation_File
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string File_IdEncrypt { get; set; }

    }
    public class vPTREvaluation_Feedback_obj_save
    {
        public string id { get; set; }
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string unit_id { get; set; }
        public string rating_id { get; set; }
        public string appreciations { get; set; }
        public string recommendations { get; set; }

    }
    public class objPTREvaluation_Feedback_Return : CResutlWebMethod
    {
        public List<vPTREvaluation_Feedback> lstData { get; set; }
    }
    public class vPTRApproval_Sumary
    {
        public int eva_id { get; set; }
        public string emp_no { get; set; }
        public string emp_name { get; set; }
        public int eva_status_id { get; set; }
        public int rating_id { get; set; }
        public string rating_name { get; set; }
        public DateTime? approva_date { get; set; }
    }

    public class vrpEvaluation
    {
        public int? nOrder { get; set; }
        public string sName { get; set; }
        public string sStepName { get; set; }
        public string sStatus { get; set; }
        public string sRating { get; set; }
        public DateTime? dApproved { get; set; }
    }
    public class vrpEvaluation_Answer
    {
        public string id { get; set; }
        public int? nOrder { get; set; }
        public string question { get; set; }
        public string header { get; set; }
        public string remark { get; set; }
        public string rating { get; set; }
        public string sgroup { get; set; }
    }
    public class rpPTREvaluation_Answer
    {
        public int? nSeq { get; set; }
        public string question { get; set; }
        public string header { get; set; }
        public string remark { get; set; }
    }
}