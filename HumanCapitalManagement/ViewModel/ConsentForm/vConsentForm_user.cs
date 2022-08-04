using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ConsentForm
{
    public class vConsentForm_user :   CResutlWebMethod
    {
        public int Id { get; set; }
        public string Employee_no { get; set; }
        public string Description { get; set; }
        public string Active_Status { get; set; }
        public DateTime Create_date { get; set; }
        public DateTime Update_date { get; set; }
        public string Create_user { get; set; }
        public string Update_user { get; set; }
        public List<vConsentAnswer_user> lstAnswer { get; set; }
        public List<lstDataSelect> lstData { get; set; }

    }
    public class lstDataSelect
    {
        public string text { get; set; }
        public string value { get; set; }
    }
    public class vConsentForm_UserDetail
    {
        public int Id { get; set; }
        public string Employee_no { get; set; }
        public string Description { get; set; }
        public string Active_Status { get; set; }
        public DateTime Create_date { get; set; }
        public DateTime Update_date { get; set; }
        public string Create_user { get; set; }
        public string Update_user { get; set; }
    }


        public class vConsentAnswer_user
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public string Description { get; set; }
        public string Active_Status { get; set; }
        public DateTime Create_date { get; set; }
        public DateTime Update_date { get; set; }
        public string Create_user { get; set; }
        public string Update_user { get; set; }
    }
}