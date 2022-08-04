using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraineeManagement.App_Start;
using TraineeManagement.ViewModels.CommonVM;

namespace TraineeManagement.ViewModels.MainVM
{
    public class vTimeSheet : CSearchTimeSheet
    {
        public List<vTimeSheet_obj> lstData { get; set; }
    }

    public class vTimeSheet_Return : CResutlWebMethod
    {
        public List<vTimeSheet_obj> lstData { get; set; }
    }
    public class vTimeSheet_obj
    {
        public string Edit { get; set; }
        public string Edit_timesheet { get; set; }
        public string View { get; set; }
        public string Id { get; set; }
        public string seq { get; set; }
        public string hr_acknowledge { get; set; }
        public string acknowledge_date { get; set; }
        public string acknowledge_user { get; set; }
        public string submit_status { get; set; }
        public string active_status { get; set; }
        public string comments { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string TM_TimeSheet_Status_Id { get; set; }

        public string req_Approve_user { get; set; }
        public string Approve_date { get; set; }
        public string Approve_user { get; set; }
        public string Approve_status { get; set; }
        public string Approve_remark { get; set; }
        public string trainee_create_date { get; set; }
        public string trainee_create_user { get; set; }
        public string trainee_update_date { get; set; }
        public string trainee_update_user { get; set; }
        public string TM_PR_Candidate_Mapping_Id { get; set; }


    }
  
    public class vTimeSheet_Detail_obj
    {
        public string Id { get; set; }
        public string seq { get; set; }
        public string date_start { get; set; }
        public string date_end { get; set; }
        public string Engagement_Code { get; set; }
        public string Engagement_Name { get; set; }
        public string Engagement_Manager { get; set; }
        public string remark { get; set; }
        public string active_status { get; set; }
        public string trainee_create_date { get; set; }
        public string trainee_create_user { get; set; }
        public string trainee_update_date { get; set; }
        public string trainee_update_user { get; set; }
        public string TM_Time_Type_Id { get; set; }
        public string TimeSheet_Form_Id { get; set; }


        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string hour { get; set; }
        public string submit_status { get; set; }
        public string approve_status { get; set; }

        //public string title
        //{
        //    get
        //    {
        //        return title;
        //    }
        //    set
        //    {
        //        title = Engagement_Code;
        //    }
        //}
        //public string start
        //{
        //    get
        //    {
        //        return start;
        //    }
        //    set
        //    {
        //        start = Engagement_Code;
        //    }
        //}
        //public string end
        //{
        //    get
        //    {
        //        return end;
        //    }
        //    set
        //    {
        //        end = Engagement_Code;
        //    }
        //}

    }
    public class vTimeSheet_obj_View
    {
        public string Add { get; set; }
        public string Edit { get; set; }
        public string View { get; set; }
        public string IdEncrypt { get; set; }
        public string Id { get; set; }
        public string seq { get; set; }
        public string hr_acknowledge { get; set; }
        public string acknowledge_date { get; set; }
        public string acknowledge_user { get; set; }
        public string submit_status { get; set; }
        public string active_status { get; set; }
        public string comments { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string TM_TimeSheet_Status_Id { get; set; }

        public string req_Approve_user { get; set; }
        public string Approve_date { get; set; }
        public string Approve_user { get; set; }
        public string Approve_status { get; set; }
        public string Approve_remark { get; set; }
        public string trainee_create_date { get; set; }
        public string trainee_create_user { get; set; }
        public string trainee_update_date { get; set; }
        public string trainee_update_user { get; set; }
        public string TM_PR_Candidate_Mapping_Id { get; set; }
        public List<vTimeSheet_obj> lstData { get; set; }
    }
    public class vTimeSheet_obj_Save
    {
        public string IdEncrypt { get; set; }
        public string Id { get; set; }
        public string seq { get; set; }
        public string hr_acknowledge { get; set; }
        public string acknowledge_date { get; set; }
        public string acknowledge_user { get; set; }
        public string submit_status { get; set; }
        public string active_status { get; set; }
        public string comments { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string TM_TimeSheet_Status_Id { get; set; }

        public string req_Approve_user { get; set; }
        public string Approve_date { get; set; }
        public string Approve_user { get; set; }
        public string Approve_status { get; set; }
        public string Approve_remark { get; set; }
        public string trainee_create_date { get; set; }
        public string trainee_create_user { get; set; }
        public string trainee_update_date { get; set; }
        public string trainee_update_user { get; set; }
        public string TM_PR_Candidate_Mapping_Id { get; set; }


        public string mgr_user_id { get; set; }
        public string mgr_user_no { get; set; }
        public string mgr_user_name { get; set; }
        public string mgr_user_rank { get; set; }
        public string mgr_unit_name { get; set; }
        public string status { get; set; }
    }
    public class vTimeSheet_Detail_obj_Save
    {
        public string IdEncrypt { get; set; }
        public string Id { get; set; }
        public string seq { get; set; }
        public string date_start { get; set; }
        public string date_end { get; set; }
        public string Engagement_Code { get; set; }
        public string remark { get; set; }
        public string active_status { get; set; }
        public string trainee_create_date { get; set; }
        public string trainee_create_user { get; set; }
        public string trainee_update_date { get; set; }
        public string trainee_update_user { get; set; }
        public string TM_Time_Type_Id { get; set; }
        public string TimeSheet_Form_Id { get; set; }
        public string TimeSheet_Form_Approve { get; set; }
        public string name { get; set; }
        public string company { get; set; }
        public string cost_center { get; set; }
        public string division_code { get; set; }
        public string account_no { get; set; }

        
        public List<vTimeSheet_Detail_obj> lstData { get; set; }
        public List<Calandar_Even> lstData_cld { get; set; }
        public List<holiday_date> lstData_holiday { get; set; }
    }
    public class objTimeSheet_Return : CResutlWebMethod
    {

    }
    public class vTimeSheet_lstData
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
    public class vTimeSheet_lst_approve
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

    public class Calandar_Even
    {
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        //public string allDay { get; set; }
        //public string url { get; set; }
        public string backgroundColor { get; set; }
        //public string borderColor { get; set; }
        public string status { get; set; }
    }
    
    public class holiday_date
    {
        public string id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string remark { get; set; }
      
    }


}