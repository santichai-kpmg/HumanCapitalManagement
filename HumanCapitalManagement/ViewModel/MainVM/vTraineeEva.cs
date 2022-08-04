using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{
    public class vTraineeEva : CSearchTraineeEva
    {
        public List<vTraineeEva_obj> lstData { get; set; }
    }
    public class vTraineeEva_Return : CResutlWebMethod
    {
        public List<vTraineeEva_obj> lstData { get; set; }
        public List<vTraineeEva_lst> lstError { get; set; }
    }
    public class vTraineeEva_lst
    {
        public string IdEncrypt { get; set; }
        public string refno { get; set; }
        public string name_en { get; set; }
        public string status { get; set; }

        public string msg { get; set; }
    }
    public class vTraineeEva_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string division { get; set; }
        public string sgroup { get; set; }
        public string position { get; set; }
        public string rank { get; set; }
        public string target_start { get; set; }
        public string target_end { get; set; }
        public string pm_name { get; set; }
        public string incharge_name { get; set; }
        public string key_type { get; set; }
        public string eva_status { get; set; }
        public string hr_ack { get; set; }
        public string bu_approve { get; set; }
        public string trainee_name { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string nickname { get; set; }
        public string pm_no { get; set; }
        public string ic_no { get; set; }
        public string hiring_status { get; set; }
        public string evaluator_name { get; set; }
        public string evaluator_mail { get; set; }
        public string hr_name { get; set; }
    }
    public class vTraineeEva_obj_View
    {

        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string group_name { get; set; }
        public string status { get; set; }
        public string trainee_name { get; set; }
        public string trainee_id { get; set; }
        public string position { get; set; }
        public string no_eva { get; set; }
        public string period_start { get; set; }
        public string period_end { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string nick_name { get; set; }
        public string trainee_no { get; set; }
        public List<vTraineeEva_obj> lstData { get; set; }
    }
    public class vTraineeEva_obj_Save
    {

        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string IdEncryptBack { get; set; }
        public string group_name { get; set; }
        public string status { get; set; }
        public string trainee_name { get; set; }
        public string trainee_id { get; set; }
        public string position { get; set; }
        public string no_eva { get; set; }
        public string period_start { get; set; }
        public string period_end { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public vObject_of_TrainEva objtifform { get; set; }
        public string mgr_user_id { get; set; }
        public string mgr_user_no { get; set; }
        public string mgr_user_name { get; set; }
        public string mgr_user_rank { get; set; }
        public string mgr_unit_name { get; set; }
        public string ic_user_id { get; set; }
        public string ic_user_no { get; set; }
        public string ic_user_name { get; set; }
        public string ic_user_rank { get; set; }
        public string ic_unit_name { get; set; }
        public string comment { get; set; }
        public string comment2 { get; set; }
        public string comment3 { get; set; }
        public string nick_name { get; set; }
        public string trainee_no { get; set; }
        public string type_approve { get; set; }
        public string can_create_eve { get; set; }
        public string incharge_comments { get; set; }
        public string hiring_status { get; set; }
        public string confidentiality_agreement { get; set; }
        public string scomplete { get; set; }
        public string target_start { get; set; }
        public string target_end { get; set; }
        public string sreject { get; set; }
        public string hiringratingname { get; set; }

    }
    public class vEva_list_question
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string question { get; set; }
        public string sgroup { get; set; }
        public string remark { get; set; }
        public string rating { get; set; }
        public string approve_rating { get; set; }
        public string sDisable { get; set; }
        public string hiringratingname { get; set; }
    }
    public class vObject_of_TrainEva
    {
        public string IdEncrypt { get; set; }
        public string TIF_no { get; set; }
        public List<vEva_list_question> lstQuestion { get; set; }
        public List<lstDataSelect> lstRating { get; set; }

    }
    public class objTraineeEva_Return : CResutlWebMethod
    {

    }
    public class vTraineeEva_lstData
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
    public class vTraineeEva_lst_approve
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


}