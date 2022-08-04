using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{
    public class vAcKnowledge : CSearchAcKnowledge
    {
        public List<vAcKnowledge_obj> lstData { get; set; }
    }

    public class vAcKnowledge_Return : CResutlWebMethod
    {
        public List<vAcKnowledge_obj> lstData { get; set; }
        public List<vAcKnowledge_lst> lstError { get; set; }
    }
    public class vAcKnowledge_lst
    {
        public string IdEncrypt { get; set; }
        public string refno { get; set; }
        public string name_en { get; set; }
        public string status { get; set; }

        public string msg { get; set; }
    }
    public class vAcKnowledge_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string group_name { get; set; }
        public string name_en { get; set; }
        public string rank { get; set; }
        public string position { get; set; }
        public string status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string tif_result { get; set; }
        public string refno { get; set; }
        public string pr_type { get; set; }
        public string activities { get; set; }
    }
    public class vAcKnowledge_obj_save
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
        public List<vRatingforHistory> lstRating { get; set; }
        public string activities_Id { get; set; }
        //public string r1Header { get; set; }
        //public string r1Deatail { get; set; }
        //public string r2Header { get; set; }
        //public string r2Deatail { get; set; }
        //public string r3Header { get; set; }
        //public string r3Deatail { get; set; }
    }

    public class vAcKnowledgeApprover_obj_save
    {
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

    }

    public class vRatingforHistory
    {
        public string text { get; set; }
        public string detail { get; set; }
        public string value { get; set; }
        public string nSeq { get; set; }
    }
    #region 
    public class vTIFReport : CSearchTIFReport
    {
        public List<vSelect_PR> lstSubGroup { get; set; }
        public List<vSelect_PR> lstPosition { get; set; }
        public List<vTIFReport_obj> lstData { get; set; }
        public List<vSelect_Activity> lstActivity { get; set; }
    }
    public class vTIFReport_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string group_name { get; set; }
        public string name_en { get; set; }
        public string rank { get; set; }
        public string position { get; set; }
        public string status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string tif_result { get; set; }
        public string tif_status { get; set; }

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
        public string activities { get; set; }
    }
    public class vTIFReport_Return : CResutlWebMethod
    {
        public List<vTIFReport_obj> lstData { get; set; }
    }
    #endregion

}