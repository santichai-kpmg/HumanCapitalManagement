using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ConsentForm
{
    public class vConsentForm_Question_main
    {
        
        public string Topic { get; set; }
        public vConsentForm_user objGetYes { get; set; }
        public List<vConsentForm_Question_sub> lstSub = new List<vConsentForm_Question_sub>();
        

    }
    public class vConsentForm_Question_sub
    {
        public string Name { get; set; }
        public string IdEncrypt { get; set; }
        public int Id { get; set; }
        public string Seq { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Active_status { get; set; }
        public string Create_date { get; set; }
        public string Create_user { get; set; }
        public string Update_date { get; set; }
        public string Update_user { get; set; }
    }

    public class vConsentForm_Question_obj
    {
        public string Name { get; set; }
        public string IdEncrypt { get; set; }
        public int Id { get; set; }
        public string Seq { get; set; }
        public string Type { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Active_status { get; set; }
        public string Create_date { get; set; }
        public string Create_user { get; set; }
        public string Update_date { get; set; }
        public string Update_user { get; set; }
        
    }
    public class vConsentForm_Answer
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Active_status { get; set; }
        public string Answer { get; set; }
        public int Seq { get; set; }

    }
    public class vConsentForm_Question 
    {
        
        public string EmployeeNo { get; set; }
        public string UserName { get; set; }
        public string IdEncrypt { get; set; }
        public int Id { get; set; }
        public string Seq { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Active_status { get; set; }
        public string Create_date { get; set; }
        public string Create_user { get; set; }
        public string Update_date { get; set; }
        public string Update_user { get; set; }
        public string Consent_date { get; set; }
        public int? TM_comsent_form_Id { get; set; }
        public List<vConsentForm_Answer> lstAnswer { get; set; }
        public List<vConsentForm_Question_obj> lstData { get; set; }
        public vConsentForm_user objGet { get; set; }
        public List<vConsentForm_Question_main> lstData_display { get; set; }

        public vConsentForm_Question_obj editData { get; set; }
        /*public List<_Page_Type> editData { get; set; }*/
    }

    public class vConsent_Group_Question_Return : CResutlWebMethod
    {

        public vConsentForm_Question_obj maindata { get; set; }
        public List<vConsentForm_Question_obj> lstData { get; set; }
    }

    public class _Page_Type
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
    }

}