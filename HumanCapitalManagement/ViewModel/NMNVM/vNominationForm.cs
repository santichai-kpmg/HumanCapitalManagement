using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.NMNVM
{


    public class vNominationForm : CSearchNMNForm
    {
        public List<vNominationForm_obj> lstData { get; set; }
    }

    public class vNominationForm_Return : CResutlWebMethod
    {
        public List<vNominationForm_obj> lstData { get; set; }
        public string IdEncrypt { get; set; }
        public string Session { get; set; }
        public List<vPartnerKPIs_obj> lstNewData { get; set; }
    }
    public class vNominationForm_UploadFile : CResutlWebMethod
    {
        public List<vNominationForm_lst_File> lstNewData { get; set; }
        public string IdEncrypt { get; set; }
    }
    public class vNominationForm_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string nmn_type { get; set; }
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
        public string approval_step { get; set; }
        public string bu_eva { get; set; }
        public string practice_eva { get; set; }
        public string p_d { get; set; }
        public string ceoe_eva { get; set; }
        public string ceoe { get; set; }
        public string ceoe_comment { get; set; }
    }
    public class rpvNominationForm_Session
    {
        public List<vNominationForm_report> lstData { get; set; }
    }
    public class vNominationForm_report
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
    }

    public class vNominationForm_obj_save
    {

        // editor mode 
        public string is_self { get; set; }
        public string view_type { get; set; }
        public string agree_status { get; set; }
        public string sMode { get; set; }
        public bool admin_mode { get; set; }
        public int Id { get; set; }
        public string type_id { get; set; }
        public string type_name { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string sname { get; set; }
        public string sgroup { get; set; }
        public string srank { get; set; }
        public string other_role { get; set; }
        public string step_id { get; set; }
        //detail
        public string Nomination_type { get; set; }
        public string Year_joined_KPMG { get; set; }
        public string working_with_KPMG { get; set; }
        public string working_outside_KPMG { get; set; }
        public string being_AD_Director { get; set; }
        public string being_ADDirector { get; set; }
        public string Professional_Qualifications { get; set; }
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


        //approver


        public List<vNominationForm_lst_approve> lstApprove { get; set; }
        public List<vNominationForm_lst_File> lstFile { get; set; }
        public List<vNominationForm_KPIs> lstKPIs { get; set; }
        public List<vNominationForm_RATING_SUMMARY> lstRating_Summary { get; set; }
        public List<vNominationForm_RATING_Annual> lstAnnul_Rate { get; set; }
        public List<lstDataSelect> lstRate { get; set; }
        public string Client_Q1 { get; set; }
        public string Client_Q2 { get; set; }
        public string Client_Q3 { get; set; }
        public string Client_Q4 { get; set; }
        public string Business_Skills_Q1 { get; set; }
        public string Business_Skills_Q2 { get; set; }
        public string Business_Skills_Q3 { get; set; }
        public string Management_Leadership_Q1 { get; set; }
        public string Management_Leadership_Q2 { get; set; }
        public string Management_Leadership_Q3 { get; set; }
        public string Social_Skills_Q1 { get; set; }
        public string Social_Skills_Q2 { get; set; }
        public string Thinking_Skills_Q1 { get; set; }
        public string Thinking_Skills_Q2 { get; set; }
        public string Thinking_Skills_Q3 { get; set; }
        public string Technical_Competence_Q1 { get; set; }
        public string Technical_Competence_Q2 { get; set; }
        public string Technical_Competence_Q3 { get; set; }
        public string RISK_MANAGEMENT_Q1 { get; set; }
        public string RISK_MANAGEMENT_Q2 { get; set; }
        public string DEVELOPMENT_AREAS { get; set; }
        public string yearcurrent_rate { get; set; }
        public string yearcurrent_selfrate { get; set; }
        public string yearone_rate { get; set; }
        public string yeartwo_rate { get; set; }
        public string yearthree_rate { get; set; }
        // [DefaultValue("disabled")]
        public string disble_mode { get; set; }

    }
    public class vNominationForm_lst_approve
    {
        public int nStep { get; set; }
        public int? id { get; set; }
        public string IdEncrypt { get; set; }
        public string step_name { get; set; }
        public string short_step_name { get; set; }
        public string emp_no { get; set; }
        public string app_code { get; set; }
        public string app_name { get; set; }
        public string description { get; set; }
        public string approve_date { get; set; }
        public string agree_status { get; set; }
        public string annual_rating { get; set; }
    }
    public class vNominationForm_lst_File
    {
        public string IdEncrypt { get; set; }
        public string Edit { get; set; }
        public string file_name { get; set; }
        public string description { get; set; }
        public string View { get; set; }
    }

    public class objNominationForm_lst_Authorized_Return : CResutlWebMethod
    {
        public List<vNominationForm_lst_Authorized> lstData { get; set; }
    }
    public class vNominationForm_lst_Authorized
    {
        public string IdEncrypt { get; set; }
        public string Edit { get; set; }
        public string authorized_name { get; set; }
        public string authorized_user { get; set; }
        public string authorized_rank { get; set; }
        public string description { get; set; }
        public string View { get; set; }
    }
    public class vNominationForm_KPIs
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
        public string yearoneTar { get; set; }
        public string yeartwoTar { get; set; }
        public string yearthreeTar { get; set; }
        public string sdisable { get; set; }
        public string stype { get; set; }
    }
    
    public class vImport_NominationForm_KPIs : CResutlWebMethod
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
        public string yearoneTar { get; set; }
        public string yeartwoTar { get; set; }
        public string yearthreeTar { get; set; }
        public string sdisable { get; set; }
        public string stype { get; set; }
    }
    public class vNominationForm_Answer
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string question { get; set; }
        public string header { get; set; }
        public string remark { get; set; }
        public string rating { get; set; }
        public string sgroup { get; set; }
        public string sdisable { get; set; }
    }
    public class vNominationForm_RATING_SUMMARY
    {
        public int? nSeq { get; set; }
        public int? nstep { get; set; }
        public string id { get; set; }
        public string question { get; set; }
        public string header { get; set; }
        public string sgroup { get; set; }
        public string sExcellent { get; set; }
        public string sHigh { get; set; }
        public string sLow { get; set; }
        public string sNI { get; set; }
        public string sdisable { get; set; }
        public string nscore { get; set; }
        public int? nrate { get; set; }
        public string isCurrent { get; set; }
        public string user { get; set; }

    }

    public class vNominationForm_RATING_Annual
    {
        public int? nSeq { get; set; }
        public int? nstep { get; set; }
        public string id { get; set; }
        public string sYear { get; set; }
        public string srate { get; set; }
        public string rate_id { get; set; }
        public string user { get; set; }
        public string IdEncrypt { get; set; }
        public string Edit { get; set; }

    }
    public class vNominationForm_Feedback
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
    public class vNominationForm_Final
    {
        public int? nSeq { get; set; }
        public string position { get; set; }
        public string status { get; set; }
        public string remark { get; set; }

    }

    public class File_Upload_NominationForm
    {
        public byte[] sfile64 { get; set; }
        public string sfileType { get; set; }
        public string sfile_name { get; set; }
        public List<vPartnerKPIs_FileTemp> lstPartnerKPIs { get; set; }
    }
    public class vNominationForm_File
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string File_IdEncrypt { get; set; }

    }

}