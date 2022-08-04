using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{

    public class vPIntern : CSearchInterview
    {
        public List<vInterview_obj> lstData { get; set; }
    }

    public class vPIntern_Return : CResutlWebMethod
    {
        public List<vInterview_obj> lstData { get; set; }
    }

    public class vPIntern_Approver_Return : CResutlWebMethod
    {
        public List<vPersonnelAp_obj> lstData { get; set; }
    }

    public class vPIntern_obj
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

        public string pr_type { get; set; }
    }


    public class vPIntern_obj_save
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
        public vObject_of_pintern objPIntern { get; set; }
        public vObject_of_Masstif objMasstifform { get; set; }
        public string set_type { get; set; }
        public string comment { get; set; }
        public string tif_status_id { get; set; }
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
    }

    public class vPInternApprover_obj_save
    {
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

    }
    public class vObject_of_pintern
    {
        public string IdEncrypt { get; set; }
        public string TIF_no { get; set; }
        public List<vtif_list_question> lstQuestion { get; set; }
        public List<lstDataSelect> lstRating { get; set; }

       

    }
    public class vPIntern_list_question
    {
        public int? nSeq { get; set; }
        public string id { get; set; }
        public string question { get; set; }
        public string header { get; set; }
        public string remark { get; set; }
        public string rating { get; set; }
        public string point { get; set; }


    }
   

}