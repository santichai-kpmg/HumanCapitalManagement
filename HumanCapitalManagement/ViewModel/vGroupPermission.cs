using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    public class vGroupPermission : CSearchGroup_Permission
    {
        public List<vGroupPermiss_obj> lstData { get; set; }
    }

    public class vGroupPermission_Return : CResutlWebMethod
    {
        public List<vGroupPermiss_obj> lstData { get; set; }
    }
    public class vGroupPermiss_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string name_th { get; set; }
        public string name_en { get; set; }
      
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }

        public List<vGroupListPermission> lstData { get; set; }
        public List<vGroupListPermission> lstDataSave { get; set; }
    }

    public class vGroupListPermission
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

    public class objGroupPermission_Return : CResutlWebMethod
    {

    }
}