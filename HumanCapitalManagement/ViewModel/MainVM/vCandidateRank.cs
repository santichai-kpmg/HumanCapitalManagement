using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{

    public class vCandidateRank : CSearchCandidateRank
    {
        public List<vCandidateRank_obj> lstData { get; set; }
    }

    public class vCandidateRank_Return : CResutlWebMethod
    {
        public List<vCandidateRank_obj> lstData { get; set; }
    }
    public class vCandidateRank_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string name_th { get; set; }
        public string name_en { get; set; }
        public string short_name_en { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string replaced_user { get; set; }
    }


    public class vCandidateRank_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string name_en { get; set; }
        public string short_name_en { get; set; }
        public string piority { get; set; }
        public string ceo_approve { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }

    }

    public class vCandidateRankApprover_obj_save
    {
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

    }
}