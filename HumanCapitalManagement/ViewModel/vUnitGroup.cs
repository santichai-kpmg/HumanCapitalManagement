using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{

    public class vUnitGroup : CSearchUnit_Group
    {
        public List<vUnitGroup_obj> lstData { get; set; }
    }

    public class vUnitGroup_Return : CResutlWebMethod
    {
        public List<vUnitGroup_obj> lstData { get; set; }
    }
    public class vUnitGroup_obj
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
    }


    public class vUnitGroup_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string pool { get; set; }
        public string pool_name { get; set; }
        public string name_en_hris { get; set; }
        public string short_name_en_hris { get; set; }
        public string name_en { get; set; }
        public string short_name_en { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public List<vUnitGroup_Approve_Permit> lstData { get; set; }
        public string group_head_id { get; set; }
        public string practice_id { get; set; }
        public string ceo_id { get; set; }
        public List<vSelect_PR> lstgrouphead { get; set; }
        public List<vSelect_PR> lstpractice { get; set; }
        public List<vSelect_PR> lstceo { get; set; }
        public string remark_group_approve { get; set; }
    }

    public class objUnitGroup_Return : CResutlWebMethod
    {
        public List<vUnitGroup_Approve_Permit> lstData { get; set; }

    }
    public class UnitGroup_Api
    {
        public string code { get; set; }
        public string pool_id { get; set; }
        public string pool_name { get; set; }
        public string group_name_hr { get; set; }
        public string group_sh_name_hr { get; set; }

    }
    public class vUnitGroup_Approve_Permit
    {
        public string Edit { get; set; }
        public string emp_code { get; set; }
        public string emp_id { get; set; }
        public string emp_name { get; set; }
        public string emp_group { get; set; }
        public string emp_position { get; set; }
        public string emp_dec { get; set; }
    }

    public class vUnitApprover_obj_save
    {
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

    }
}