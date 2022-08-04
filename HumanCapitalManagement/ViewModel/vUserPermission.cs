using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{

    public class vUserPermission : CSearchUser_Permission
    {
        public List<vUserPermiss_obj> lstData { get; set; }
    }

    public class vUserPermission_Return : CResutlWebMethod
    {
        public List<vUserPermiss_obj> lstData { get; set; }
    }
    public class vUserPermiss_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string user_id { get; set; }
        public string user_no { get; set; }
        public string user_name { get; set; }
        public string user_last_name { get; set; }
        public string unit_name { get; set; }
        public string rank { get; set; }
        public string unit_group { get; set; }
        public string unit_permis { get; set; }
        public List<vListGroup_lstData> lstGroup_Per { get; set; }
        public string permis_group { get; set; }
        public string user_group { get; set; }
        public string user_group_code { get; set; }
        public string group_permiss_id { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public List<vUserListPermission> lstData { get; set; }
        public List<vUserListPermission> lstDataSave { get; set; }
        public string[] aUnitCode { get; set; }
        public List<C_USERS_RETURN> lstUser{ get; set; }


    }

    public class vUserListPermission
    {
        public string menu_id { get; set; }
        public int n_seq { get; set; }
        public int menu_level { get; set; }
        public string menu_name { get; set; }
        public string menu_permission { get; set; }
        public string view_action { get; set; }
        public string add_action { get; set; }
        public string edit_action { get; set; }
        public string approve_action { get; set; }
        public string view_value { get; set; }
        public string add_value { get; set; }
        public string edit_value { get; set; }
        public string approve_value { get; set; }
        //detail
        public string detail_value { get; set; }
        public string detail_action { get; set; }
    }

    public class objUserPermission_Return : CResutlWebMethod
    {

    }

    public class vLoadPermission_Return : CResutlWebMethod
    {
        public List<vListPermission> lstData { get; set; }
    }
    public class vListPermission
    {
        public string menu_id { get; set; }
        public string Action_Type { get; set; }
    }
    public class vListGroup_lstData
    {
        public string Id { get; set; }
        public string group_name { get; set; }
    }
    public class vListGroup_popup_detail
    {
        public string IdEncrypt { get; set; }
        public string sMode { get; set; }
        public List<vListGroup_lstData> lstGroup { get; set; }
    }

    public class vSearchPermisMuntl
    {
        public string group_id { get; set; }
        public string rank_id { get; set; }
    }
    public class vUserPermisMuntl_Return : CResutlWebMethod
    {
        public List<C_USERS_RETURN> lstData { get; set; }
    }
}