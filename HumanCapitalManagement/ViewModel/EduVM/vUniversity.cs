using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.EduVM
{

    public class vUniversity : CSearchUniversity
    {
        public List<vUniversity_obj> lstData { get; set; }
    }
    public class vUniversity_Return : CResutlWebMethod
    {
        public List<vUniversity_obj> lstData { get; set; }
    }
    public class vUniversity_obj
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


    public class vUniversity_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string name_en { get; set; }
        public string name_th { get; set; }
        public string name_aol { get; set; }
        public string short_name_en { get; set; }
        public string replaced_user { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public List<vUniversity_Faculty> lstFaculty { get; set; }
    }
    public class objUniversity_Faculty_Return : CResutlWebMethod
    {
        public List<vUniversity_Faculty> lstData { get; set; }

        public vUniversity_Faculty_obj_save objData { get; set; }
    }

    public class vUniversity_Faculty
    {

        public string Edit { get; set; }
        public string fa_name { get; set; }
        public string fa_display_name { get; set; }
        public string decs { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
    }

    public class vUniversity_Faculty_obj_save
    {
        public string IdEncrypt { get; set; }
        public string fa_name_id { get; set; }
        public string fa_display_name { get; set; }
        public string decs { get; set; }
        public string active_status { get; set; }
        public string id { get; set; }
    }
    public class vUniversity_Faculty_edit
    {
        public string IdEncrypt { get; set; }
        public string IdEncrypt_Uni_Id { get; set; }
    }
}