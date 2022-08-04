using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraineeManagement.App_Start;
using TraineeManagement.ViewModels.CommonVM;

namespace TraineeManagement.ViewModels.MainVM
{
    public class vEvaluation : CSearchEvaluation
    {
        public List<vEvaluation_obj> lstData { get; set; }
    }

    public class vEvaluation_Return : CResutlWebMethod
    {
        public List<vEvaluation_obj> lstData { get; set; }
    }
    public class vEvaluation_obj
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
    }
    public class vEvaluation_obj_View
    {

        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string group_name { get; set; }
        public string status { get; set; }
        public string trainee_name { get; set; }
        public string trainee_id { get; set; }
        public string position { get; set; }
        public string no_eva { get; set; }
        public string target_start { get; set; }
        public string target_end { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string nick_name { get; set; }
        public string trainee_no { get; set; }
        public List<vEvaluation_obj> lstData { get; set; }
        //public vObject_of_tif objtifform { get; set; }
        //public string mgr_user_id { get; set; }
        //public string mgr_user_no { get; set; }
        //public string mgr_user_name { get; set; }
        //public string mgr_user_rank { get; set; }
        //public string mgr_unit_name { get; set; }
        //public string ic_user_id { get; set; }
        //public string ic_user_no { get; set; }
        //public string ic_user_name { get; set; }
        //public string ic_user_rank { get; set; }
        //public string ic_unit_name { get; set; }
    }
    public class vEvaluation_obj_Save
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
        public vObject_of_tif objtifform { get; set; }
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
        public string target_start { get; set; }
        public string target_end { get; set; }
    }
    public class vEva_list_question
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string question { get; set; }
        public string sgroup { get; set; }
        public string remark { get; set; }
        public string rating { get; set; }

    }
    public class vObject_of_tif
    {
        public string IdEncrypt { get; set; }
        public string TIF_no { get; set; }
        public List<vEva_list_question> lstQuestion { get; set; }
        public List<lstDataSelect> lstRating { get; set; }
        public List<lstDataSelect> lstRatingActive { get; set; }

    }
    public class objEvaluation_Return : CResutlWebMethod
    {

    }
    public class vEvaluation_lstData
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
    public class vEvaluation_lst_approve
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
}