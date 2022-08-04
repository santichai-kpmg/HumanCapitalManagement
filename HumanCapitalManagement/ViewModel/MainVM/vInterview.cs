using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{

    public class vInterview : CSearchInterview
    {
        public List<vInterview_obj> lstData { get; set; }
    }

    public class vInterview_Return : CResutlWebMethod
    {
        public List<vInterview_obj> lstData { get; set; }
    }

    public class vInterview_Approver_Return : CResutlWebMethod
    {
        public List<vPersonnelAp_obj> lstData { get; set; }
    }

    public class vInterview_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string refno { get; set; }
        public string group_name { get; set; }
        public string name_en { get; set; }
        public string phone { get; set; }
        public string rank { get; set; }
        public string position { get; set; }
        public string short_name_en { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string approval { get; set; }
        public string seq_approval { get; set; }
        public string activities { get; set; }
        public string pr_type { get; set; }
    }


    public class vInterview_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string group_id { get; set; }
        public string sub_group_id { get; set; }
        public string employment_type_id { get; set; }
        public string request_type_id { get; set; }
        public string rank_id { get; set; }
        public string position_id { get; set; }
        public string TIF_type { get; set; }
        public string null_tif_contact { get; set; }
        public string pr_no { get; set; }
        public string tif_id { get; set; }
        public vObject_of_tif objtifform { get; set; }
        public vObject_of_Masstif objMasstifform { get; set; }
        public string set_type { get; set; }
        public string comment { get; set; }
        public string tif_status_id { get; set; }
        public string PIA_status_id { get; set; }
        public string candidate_name { get; set; }
        //for Approve
        public string approve_user { get; set; }
        public string user_id { get; set; }
        public string user_no { get; set; }
        public string user_name { get; set; }
        public string user_rank { get; set; }
        public string unit_name { get; set; }
        public string approve_status { get; set; }
        //approver
        public List<vPersonnelAp_obj> lstApprove { get; set; }

        public string confidentiality_agreement { get; set; }

        public List<vSelect_PR> lstrank { get; set; }
        public string recommended_rank_id { get; set; }
        public string rejeck { get; set; }
        public string target_start { get; set; }
        //Rating Mass
        public List<vtif_mass_rating> lstratingnew { get; set; }

        //Question new form mass
        public List<vtif_mass_questionnew> lstquestionnew { get; set; }
        public string activities_Id { get; set; }
    }

    public class vInterviewApprover_obj_save
    {
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

    }
    public class vObject_of_tif
    {
        public string IdEncrypt { get; set; }
        public string TIF_no { get; set; }
        public List<vtif_list_question> lstQuestion { get; set; }
        public List<lstDataSelect> lstRating { get; set; }

        public List<vtif_list_rating> lstR { get; set; }


    }
    public class vtif_list_question
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string question { get; set; }
        public string header { get; set; }
        public string remark { get; set; }
        public string rating { get; set; }
        public string point { get; set; }
        public string topic { get; set; }

    }
    public class vtif_list_rating
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
   
        public string remark { get; set; }
        public string rating { get; set; }
        

    }
    public class vtif_mass_rating
    {
       
        public string text { get; set; }
        public string value { get; set; }

    }
    public class vtif_mass_questionnew
    {
        public string question { get; set; }
        public string header { get; set; }
        public string value { get; set; }

    }
    public class vObject_of_Masstif
    {
        public string IdEncrypt { get; set; }

        public string TIF_no { get; set; }
        public List<vMasstif_list_question> lstQuestion { get; set; }
        public List<vMasstif_list_Auditing> lstAuditing { get; set; }
        public List<lstDataSelectMass> lstScoring { get; set; }
        public List<lstDataSelect> lstAuditing_Qst { get; set; }
        public List<vMassAdInfo_Question> lstAdInfo { get; set; }

        public List<lstDataSelectMassRating> lstRating { get; set; }


        public static implicit operator vObject_of_Masstif(vInterview_obj_save v)
        {
            throw new NotImplementedException();
        }
    }
    public class vMassAdInfo_Question
    {
        public string nID { get; set; }
        public int? seq { get; set; }
        public string question { get; set; }
        public string multi_answer { get; set; }
        public List<vMassAdInfo_Answers> lstAnswers { get; set; }
        public string[] lstAnswersselect { get; set; }
        public string other_answer { get; set; }
        public string is_validate { get; set; }
        public string header { get; set; }

    }
    public class vMassAdInfo_Answers
    {
        public string nID { get; set; }
        public string header { get; set; }
        public string answer { get; set; }
        public string other_answer { get; set; }
        public string other_text { get; set; }
        public string question_text { get; set; }
        public string question_id { get; set; }
        public int? seq { get; set; }
    }
    public class vMasstif_list_question
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string header { get; set; }
        public string a_answer { get; set; }
        public string b_answer { get; set; }
        public string c_answer { get; set; }
        public string evidence { get; set; }
        public string scoring { get; set; }
        public string rating{ get; set; }
        public string value { get; set; }
        public string question { get; set; }
        public string topic { get; set; }
    }
    public class vMasstif_list_Auditing
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string header { get; set; }
        public string Qst { get; set; }
        public string scoring { get; set; }
        public string answer { get; set; }
        public string group { get; set; }
        public List <lstDataSelect> lstQuiz { get; set; }
    }
    public class vMassType_onchange
    {
        public List<lstDataSelect> lstAuditing_Qst { get; set; }
    }
    public class lstDataSelectMass
    {
        public string text { get; set; }
        public string value { get; set; }
        public int? point { get; set; }
        public string code { get; set; }
    }
    public class lstDataSelectMassRating
    {
        public string text { get; set; }
        public string value { get; set; }
    }

}