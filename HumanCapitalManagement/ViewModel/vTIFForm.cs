using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    public class vTIFForm : CSearchTIFForm
    {
        public List<vTIFForm_obj> lstData { get; set; }
        public List<vActivity_obj> lstDataActivity { get; set; }
    }

    public class vTIFForm_Return : CResutlWebMethod
    {
        public List<vTIFForm_obj> lstData { get; set; }
        public List<vActivity_obj> lstDataActivity { get; set; }
    }
    public class vTIFForm_obj
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
    public class vActivity_obj
    {
        public string View { get; set; }
        public string Edit { get; set; }
        public string Activities_name_en { get; set; }
        public string Id { get; set; }
        public string activestatus { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
    }

    public class vTIFForm_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string action_date { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public List<vTIFForm_Question> lstQuestion { get; set; }
        public List<vRatingForm_PreIntern> lstRating { get; set; }
        public List<vActivity_obj_save> lstActivity { get; set; }
    }
    public class vTIFForm_Question
    {
        public string nID { get; set; }
        public string header { get; set; }
        public string topic { get; set; }
        public string question { get; set; }
    }


    public class vRatingForm_PreIntern
    {
        public string nID { get; set; }
        public string rating_des { get; set; }
        public string rating_name { get; set; }
        public string point { get; set; }

    }
    public class vActivity_obj_save
    {
        public int Id { get; set; }
        public string nID { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string Seq { get; set; }
        public string activities_name_en { get; set; }
        public string activities_name_th { get; set; }
        public string activities_description { get; set; }
        public string active_status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string action_date { get; set; }

    }
}