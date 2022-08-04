using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{

    public class vMassTIFForm : CSearchMassTIFForm
    {
        public List<vMassTIFForm_obj> lstData { get; set; }
    }

    public class vMassTIFForm_Return : CResutlWebMethod
    {
        public List<vMassTIFForm_obj> lstData { get; set; }
    }

    public class EvaluateMassTif
    {
        public List<int> Core_answers { get; set; }
        public List<int> Audit_answers { get; set; }
    }
    public class vMassTIFForm_obj
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


    public class vMassTIFForm_obj_save
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
        public List<vMassTIFForm_Question> lstQuestion { get; set; }

        public List<vMassTIFForm_AudQuestion> lstAuditing_Questions { get; set; }
    }
    public class vMassTIFForm_Question
    {
        public string nID { get; set; }
        public string header { get; set; }
        public string aanswer { get; set; }
        public string banswer { get; set; }
        public string canswer { get; set; }

    }

    public class vMassTIFForm_AudQuestion
    {
        public string nID { get; set; }
        public string header { get; set; }
        public string guideline { get; set; }
        public string set_text { get; set; }
        public string set_id { get; set; }
        public int? nSeq { get; set; }

    }
}