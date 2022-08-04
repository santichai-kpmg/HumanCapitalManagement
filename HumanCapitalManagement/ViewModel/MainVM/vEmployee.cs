using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{

    public class vCompany : CSearchCompany
    {
        public List<vCompany_obj> lstDataCompany { get; set; }
    }
    public class vEmployee
    {
        public List<vEmployee_obj> lstDataEmployee { get; set; }
    }
    public class vEmployee_Save
    {
        public List<vEmployee_obj> lstDataEmployee { get; set; }
    }
    public class vDocument_EMP : CSearchDocument
    {
        public List<vDocumentEmployee_obj> lstDocEmployee { get; set; }
    }
    public class vCompany_Return : CResutlWebMethod
    {

        public List<vCompany_obj> lstDataCompany { get; set; }
        public List<vEmployee_obj> lstDataEmployee { get; set; }
        public List<vMember_obj> lstDataMember { get; set; }
        public List<vCompany_obj_save> lstDataTSIC { get; set; }
        public List<vDocumentEmployee_obj> lstDocEmployee { get; set; }
        //public List<vActivity_obj> lstDataActivity { get; set; }
    }

    public class vCompany_obj_save
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string company_eng { get; set; }
        public string company_th { get; set; }
        public string kpmg_code { get; set; }
        public string registration_number { get; set; }
        public string TSIC_Id { get; set; }
        public string TSIC_Name { get; set; }
        public string Staff_id { get; set; }
        public string Staff_fullname { get; set; }
        public string Staff_rank { get; set; }
        public string Staff_group { get; set; }
        public string active_status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public List<vEmployee_obj> lstEmployee { get; set; }
        public string Status { get; set; }
        public string BackPage { get; set; }
        //for Approve
        public string approve_user { get; set; }
        public string user_id { get; set; }
        public string user_no { get; set; }
        public string user_name { get; set; }
        public string user_rank { get; set; }
        public string unit_name { get; set; }
        public string approve_status { get; set; }
        public string User { get; set; }

    }
    public class vCompany_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string company_eng { get; set; }
        public string company_th { get; set; }
        public string kpmg_code { get; set; }
        public string registration_number { get; set; }
        public string Approve_Status { get; set; }
        public string TSIC_Id { get; set; }
        public string Staff_id { get; set; }
        public string active_status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
    }



    public class vEmployee_obj
    {
        public int Id { get; set; }
        public string Edit { get; set; }
        public string View { get; set; }
        public string IdEncrypt { get; set; }
        public string TM_Prefix_Id { get; set; }
        public string TM_Prefix_name { get; set; }
        public string code { get; set; }
        public string name_eng { get; set; }
        public string name_th { get; set; }
        public string middle { get; set; }
        public string full_name { get; set; }
        public string lastname_eng { get; set; }
        public string lastname_th { get; set; }
        public string telephone { get; set; }
        public string mail { get; set; }
        public string staff { get; set; }
        public string active_status { get; set; }
        public string CompanyDetail_Id { get; set; }
        public string Company_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
    }

    public class vEmployee_obj_save
    {
        public const string pattern = config.regexpattern;
        //public static string regex = "";
        [RegularExpression(pattern)]
        public int Id { get; set; }
        [RegularExpression(pattern)]
        public string IdEMP { get; set; }
        [RegularExpression(pattern)]
        public string Edit { get; set; }
        [RegularExpression(pattern)]
        public string View { get; set; }
        [RegularExpression(pattern)]
        public string IdEncrypt { get; set; }
        [RegularExpression(pattern)]
        public string Company_name { get; set; }
        [RegularExpression(pattern)]
        public string TM_Prefix_Id { get; set; }
        [RegularExpression(pattern)]
        public string membercode { get; set; }
        [RegularExpression(pattern)]
        public string code { get; set; }
        [RegularExpression(pattern)]
        public string name_eng { get; set; }
        [RegularExpression(pattern)]
        public string name_th { get; set; }
        [RegularExpression(pattern)]
        public string middle { get; set; }
        [RegularExpression(pattern)]
        public string full_name { get; set; }
        [RegularExpression(pattern)]
        public string lastname_eng { get; set; }
        [RegularExpression(pattern)]
        public string lastname_th { get; set; }
        [RegularExpression(pattern)]
        public string telephone { get; set; }
        [RegularExpression(pattern)]
        public string mail { get; set; }
        [RegularExpression(pattern)]
        public string active_status { get; set; }
        [RegularExpression(pattern)]
        public string TM_CompanyDetail_Id { get; set; }
        [RegularExpression(pattern)]
        public string create_date { get; set; }
        [RegularExpression(pattern)]
        public string create_user { get; set; }
        [RegularExpression(pattern)]
        public string update_date { get; set; }
        [RegularExpression(pattern)]
        public string update_user { get; set; }
        [RegularExpression(pattern)]
        public string BackPage { get; set; }
        [RegularExpression(pattern)]
        public string BackPage_Head { get; set; }
        [RegularExpression(pattern)]
        public string Member { get; set; }
        [RegularExpression(pattern)]
        public string staff { get; set; }
        [RegularExpression(pattern)]
        public string staff_no { get; set; }
        [RegularExpression(pattern)]
        public string staff_name { get; set; }
        [RegularExpression(pattern)]
        public string staff_group { get; set; }
        [RegularExpression(pattern)]
        public string staff_rank { get; set; }
        [RegularExpression(pattern)]
        public string remark_delete { get; set; }
        [RegularExpression(pattern)]
        public string remark { get; set; }
        public List<vDocumentEmployee_obj> lstDocEmployee { get; set; }
        public List<vMember_obj> lstMember { get; set; }

    }

    public class vDocumentEmployee_obj
    {
        public const string pattern = config.regexpattern;
        [RegularExpression(pattern)]
        public int Id { get; set; }
        [RegularExpression(pattern)]
        public int Id_Doc { get; set; }
        [RegularExpression(pattern)]
        public string IdEncrypt { get; set; }
        [RegularExpression(pattern)]
        public string Edit { get; set; }
        [RegularExpression(pattern)]
        public string View { get; set; }
        [RegularExpression(pattern)]
        public string document_number { get; set; }
        [RegularExpression(pattern)]
        public string date_of_issued { get; set; }
        [RegularExpression(pattern)]
        public string Country_id { get; set; }
        [RegularExpression(pattern)]
        public string valid_date { get; set; }
        [RegularExpression(pattern)]
        public string active_status { get; set; }
        [RegularExpression(pattern)]
        public string active_status_edit { get; set; }
        [RegularExpression(pattern)]
        public string create_date { get; set; }
        public DateTime? date_of_issued_edit { get; set; }
        [RegularExpression(pattern)]
        public string create_user { get; set; }
        [RegularExpression(pattern)]
        public string update_date { get; set; }
        public DateTime? valid_date_edit { get; set; }
        [RegularExpression(pattern)]
        public string update_user { get; set; }
        [RegularExpression(pattern)]
        public string Employee_Id { get; set; }
        [RegularExpression(pattern)]
        public string Type_doc_id { get; set; }
        [RegularExpression(pattern)]
        public string Alert_Type_Create { get; set; }

    }
    public class vMember_obj
    {
        public const string pattern = config.regexpattern;
        [RegularExpression(pattern)]
        public int Id { get; set; }
        [RegularExpression(pattern)]
        public string EditMember { get; set; }
        [RegularExpression(pattern)]
        public string View { get; set; }
        [RegularExpression(pattern)]
        public string IdEncrypt { get; set; }
        [RegularExpression(pattern)]
        public string TM_Prefix_Id { get; set; }
        [RegularExpression(pattern)]
        public string TM_Prefix_name { get; set; }
        [RegularExpression(pattern)]
        public string code { get; set; }
        [RegularExpression(pattern)]
        public string name_eng { get; set; }
        [RegularExpression(pattern)]
        public string name_th { get; set; }
        [RegularExpression(pattern)]
        public string middle { get; set; }
        [RegularExpression(pattern)]
        public string full_name { get; set; }
        [RegularExpression(pattern)]
        public string lastname_eng { get; set; }
        [RegularExpression(pattern)]
        public string lastname_th { get; set; }
        [RegularExpression(pattern)]
        public string telephone { get; set; }
        [RegularExpression(pattern)]
        public string mail { get; set; }
        [RegularExpression(pattern)]
        public string active_status { get; set; }
        [RegularExpression(pattern)]
        public string CompanyDetail_Id { get; set; }
        [RegularExpression(pattern)]
        public string Company_name { get; set; }
        [RegularExpression(pattern)]
        public string create_date { get; set; }
        [RegularExpression(pattern)]
        public string create_user { get; set; }
        [RegularExpression(pattern)]
        public string update_date { get; set; }
        [RegularExpression(pattern)]
        public string update_user { get; set; }
        [RegularExpression(pattern)]
        public string Backpage_Customer { get; set; }
        [RegularExpression(pattern)]
        public string staff { get; set; }

    }
    public class vDocumentEmployee_obj_search
    {
        public string IdEncrypt { get; set; }
        public string status_id { get; set; }
        public string description { get; set; }
    }
}