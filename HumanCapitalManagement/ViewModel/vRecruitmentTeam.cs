using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    public class vRecruitmentTeam : CSearchRecruitmentTeam
    {
        public List<vRecruitmentTeam_obj> lstData { get; set; }
    }

    public class vRecruitmentTeam_Return : CResutlWebMethod
    {
        public List<vRecruitmentTeam_obj> lstData { get; set; }
    }
    public class vRecruitmentTeam_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string name_en { get; set; }
        public string group_name { get; set; }
        public string rank { get; set; }
        public string short_name_en { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
    }


    public class vRecruitmentTeam_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string group_code { get; set; }
        public string name_en { get; set; }
        public string short_name_en { get; set; }
        public string user_id { get; set; }
        public string user_no { get; set; }
        public string user_name { get; set; }
        public string user_position { get; set; }
        public string unit_name { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }

    }

    public class vRecruitmentTeamApprover_obj_save
    {
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

    }
}