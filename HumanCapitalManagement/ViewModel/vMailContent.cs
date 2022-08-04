using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    public class vMailContent : CSearchMailContent
    {
        public List<vMailContent_obj> lstData { get; set; }
    }

    public class vMailContent_Return : CResutlWebMethod
    {
        public List<vMailContent_obj> lstData { get; set; }
    }
    public class vMailContent_obj
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


    public class vMailContent_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string name_en { get; set; }
        public string mail_header { get; set; }
        public string mail_to { get; set; }
        public string sender_name { get; set; }
        public string content { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public List<vmail_cc> lstData { get; set; }
        public List<vmail_cc_bymail> lstData_mail { get; set; }
        public List<vMailContent_Parameter> lstData_param { get; set; }
    }

    public class vMailContent_Parameter
    {
        public string name { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

    }

    public class vMailContentApprover_obj_save
    {
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

    }
    public class vmail_cc
    {
        public string Edit { get; set; }
        public string emp_code { get; set; }
        public string emp_id { get; set; }
        public string emp_name { get; set; }
        public string emp_group { get; set; }
        public string emp_position { get; set; }
        public string emp_dec { get; set; }
    }

    public class objCC_Return : CResutlWebMethod
    {
        public List<vmail_cc> lstData { get; set; }

    }


    public class vmail_cc_bymail
    {
        public string Edit { get; set; }
        public string mail_cc { get; set; }
        public string mail_id { get; set; }
        public string mail_update { get; set; }
        public string mail_update_by { get; set; }
        public string mail_dec { get; set; }
    }
    public class objCCmail_Return : CResutlWebMethod
    {
        public List<vmail_cc_bymail> lstData_mail { get; set; }

    }

}