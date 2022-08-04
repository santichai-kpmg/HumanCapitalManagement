using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.PESVM
{

    public class vPTRMailBox : CSearchPTRMailBox
    {
        public List<vPTRMailBox_obj> lstData { get; set; }
    }

    public class vPTRMailBox_Return : CResutlWebMethod
    {
        public List<vPTRMailBox_obj> lstData { get; set; }
        public string IdEncrypt { get; set; }
        public string Session { get; set; }
    }
    public class vPTRMailBox_obj
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

    public class vAddEvaluationPlan_Return : CResutlWebMethod
    {
        public List<vPartnerKPIs_obj> lstNewData { get; set; }
        public List<vPartner_obj> lstOldData { get; set; }
        public string IdEncrypt { get; set; }
    }
    public class vPTRMailBox_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }
        public string action_date { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string Session { get; set; }
        public string file_name { get; set; }
        public string actual_file_name { get; set; }
        public List<vPartnerKPIs_obj> lstNewData { get; set; }
        public List<vPartnerKPIs_obj> lstActualData { get; set; }
        public List<vPartner_obj> lstOldData { get; set; }
    }
    public class File_Upload_PTRMailBox
    {
        public byte[] sfile64 { get; set; }
        public string sfileType { get; set; }
        public string sfile_name { get; set; }

        public byte[] Actualsfile64 { get; set; }
        public string ActualsfileType { get; set; }
        public string Actualsfile_name { get; set; }
        public List<vPartnerKPIs_FileTemp> lstPartnerKPIs { get; set; }
        public List<vPartnerKPIs_FileTemp> lstActualPTR { get; set; }
    }
    public class vPTRMailBoReturn_UploadFile : CResutlWebMethod
    {
        public List<vPartnerKPIs_obj> lstNewData { get; set; }
        public string IdEncrypt { get; set; }
    }
    public class vPartnerKPIs_obj
    {
        public string Edit { get; set; }
        public string codeEncrypt { get; set; }
        public string emp_code { get; set; }
        public string emp_id { get; set; }
        public string emp_name { get; set; }
        public string emp_comp { get; set; }
        public string emp_group { get; set; }
        public string emp_rank { get; set; }
        public string emp_dec { get; set; }
        public string Fee_Managed { get; set; }
        public string Contribution_margin { get; set; }
        public string Recovery_rate { get; set; }
        public string Lock_up { get; set; }
        public string Chargeability { get; set; }
        public string Billing { get; set; }
        public string Collections { get; set; }
        public string Fee_Managed_group { get; set; }
        public string Contribution_margin_group { get; set; }
        public string Recovery_rate_group { get; set; }
        public string Lock_up_group { get; set; }
        public string Chargeability_group { get; set; }
        public string Billing_group { get; set; }
        public string Collections_group { get; set; }
        public string Fee_Managed_sbu { get; set; }
        public string Contribution_margin_sbu { get; set; }
        public string Recovery_rate_sbu { get; set; }
        public string Lock_up_sbu { get; set; }
        public string Chargeability_sbu { get; set; }
        public string Billing_sbu { get; set; }
        public string Collections_sbu { get; set; }
        public string Fee_Managed_su { get; set; }
        public string Contribution_margin_su { get; set; }
        public string Recovery_rate_su { get; set; }
        public string Lock_up_su { get; set; }
        public string Chargeability_su { get; set; }
        public string Billing_su { get; set; }
        public string Collections_su { get; set; }
        public string lstApproval { get; set; }
    }
    public class vPartnerKPIs_FileTemp
    {

        public string emp_code { get; set; }
        public string Fee_Managed { get; set; }
        public string Contribution_margin { get; set; }
        public string Recovery_rate { get; set; }
        public string Lock_up { get; set; }
        public string Chargeability { get; set; }
        public string Billing { get; set; }
        public string Collections { get; set; }
        public string Fee_Managed_Group { get; set; }
        public string Contribution_margin_Group { get; set; }
        public string Recovery_rate_Group { get; set; }
        public string Lock_up_Group { get; set; }
        public string Chargeability_Group { get; set; }
        public string Billing_Group { get; set; }
        public string Collections_Group { get; set; }
        public string Fee_Managed_sbu { get; set; }
        public string Contribution_margin_sbu { get; set; }
        public string Recovery_rate_sbu { get; set; }
        public string Lock_up_sbu { get; set; }
        public string Chargeability_sbu { get; set; }
        public string Billing_sbu { get; set; }
        public string Collections_sbu { get; set; }
        public string Fee_Managed_su { get; set; }
        public string Contribution_margin_su { get; set; }
        public string Recovery_rate_su { get; set; }
        public string Lock_up_su { get; set; }
        public string Chargeability_su { get; set; }
        public string Billing_su { get; set; }
        public string Collections_su { get; set; }

    }
    public class vPartner_obj
    {
        public string Edit { get; set; }
        public string codeEncrypt { get; set; }
        public string emp_code { get; set; }
        public string emp_id { get; set; }
        public string emp_name { get; set; }
        public string emp_comp { get; set; }
        public string emp_group { get; set; }
        public string emp_rank { get; set; }
        public string emp_dec { get; set; }
    }


}