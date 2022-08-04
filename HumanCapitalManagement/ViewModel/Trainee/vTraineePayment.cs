using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.MainVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static HumanCapitalManagement.Controllers.TraineeController.TraineePerdiemTransportController;
using static HumanCapitalManagement.ViewModel.Trainee.vPerdiemTransport;

namespace HumanCapitalManagement.ViewModel.Trainee
{
    public class vTraineePayment
    {
        public class ResultLine_Save : CResutlWebMethod
        {
            public List<ResultLine> lst_data_save { get; set; }
            public string IdEncrypt { get; set; }
        }

        public class ResultLine
        {
            public ResultLine() { }

            public string name { get; set; }
            public string month { get; set; }
            public string daily_wage { get; set; }
            public string rate1 { get; set; }
            public string rate2 { get; set; }
            public string rate3 { get; set; }
            public string totalhrs { get; set; }
            public string total { get; set; }
            public string from { get; set; }
            public string date { get; set; }
            public string subject { get; set; }
            public string approvedate { get; set; }
            public string type_save { get; set; }


            public List<vTimeSheet_Detail> lstTimeSheet_Detail_All { get; set; }
            public List<vPerdiem_Transport> lstPerdiem_Transport_All { get; set; }

            public List<ResultLine_All> lstData_All { get; set; }
            public List<ResultLine_OT> lstData_OT { get; set; }
            public List<ResultLine_AutoPay> lstData_AutoPay { get; set; }
            public List<ResultLine_Sum> lstData_Sum { get; set; }
            public List<ResultLine_Memo> lstData_Memo { get; set; }
            public List<ResultLine_Sum_Perdiem> lstData_PerdiemTransport { get; set; }
        }
        public class ResultLine_All
        {
            public ResultLine_All() { }

            public string id { get; set; }
            public string company { get; set; }
            public string cost_center { get; set; }
            public string trainee_code { get; set; }
            public string name { get; set; }
            public string hours_standard { get; set; }
            public string hours_ot { get; set; }
            public string hours_total { get; set; }
            public string amount_standard { get; set; }
            public string amount_ot { get; set; }
            public string amount_perdiem { get; set; }
            public string amount_total { get; set; }
            public string bank { get; set; }
            public string id_book_bank { get; set; }
            public string note { get; set; }
            public int? trainee_create_user { get; set; }
        }
        public class ResultLine_AutoPay
        {
            public ResultLine_AutoPay() { }

            public string Vendor_ID { get; set; }
            public string Bank_Code { get; set; }
            public string Account_Number { get; set; }
            public string Vendor_Name { get; set; }
            public string Amount { get; set; }
            public string Bene_Ref { get; set; }
            public string WHT { get; set; }
            public string Advice { get; set; }
            public string SMS { get; set; }
            public string Payment_Detail { get; set; }
            public string Invoice { get; set; }
            public string Branch_Code { get; set; }
            public string Cost_Center { get; set; }
            public string Company { get; set; }
        }
        public class ResultLine_OT
        {
            public ResultLine_OT() { }

            public string name { get; set; }
            public string month { get; set; }
            public string daily_wage { get; set; }
            public string rate1 { get; set; }
            public string rate2 { get; set; }
            public string rate3 { get; set; }
            public string totalhrs { get; set; }
            public string total { get; set; }
        }
        public class ResultLine_Sum
        {
            public ResultLine_Sum() { }

            public string company { get; set; }
            public string cost_center { get; set; }
            public string trainee_code { get; set; }
            public string name { get; set; }
            public string client_name { get; set; }
            public string code { get; set; }
            public string client { get; set; }
            public string office { get; set; }
            public string nomal_hours { get; set; }
            public string nomal_amount { get; set; }
            public string ot_hours { get; set; }
            public string ot_amount { get; set; }
            public string grand_total { get; set; }
            public string nomal_amount_hd { get; set; }
            public string ot_amount_hd { get; set; }
        }
        public class ResultLine_Sum_Perdiem
        {
            public string Edit { get; set; }
            public int Id { get; set; }
            public int? seq { get; set; }
            public string date_start { get; set; }
            public string date_end { get; set; }
            public string daterange { get; set; }
            public string Engagement_Code { get; set; }
            public string Engagement_Name { get; set; }
            public string remark { get; set; }
            public string Type_of_withdrawal { get; set; }
            public string Company { get; set; }
            public string Reimbursable { get; set; }
            public string Business_Purpose { get; set; }
            public string Description { get; set; }
            public decimal? Amount { get; set; }
            public string active_status { get; set; }
            public DateTime? trainee_create_date { get; set; }
            public int? trainee_create_user { get; set; }
            public DateTime? trainee_update_date { get; set; }
            public int? trainee_update_user { get; set; }
            public string submit_status { get; set; }
            public string approve_status { get; set; }
            public string Approve_user { get; set; }
            public string mgr_user_id { get; set; }
            public string mgr_user_no { get; set; }
            public string mgr_user_name { get; set; }
            public string mgr_user_rank { get; set; }
            public string mgr_unit_name { get; set; }
            public string status { get; set; }
            public string Cost_Center { get; set; }
            public string Trainee_Code { get; set; }
            public string Name { get; set; }
            public string Create_Date { get; set; }




        }
        public class ResultLine_Memo
        {
            public ResultLine_Memo() { }

            public string company { get; set; }
            public string standard { get; set; }
            public string cost_center { get; set; }
            public string ot { get; set; }
            public string perdiem { get; set; }
            public string total { get; set; }
        }
        public class Engagement_PR
        {
            public string id { get; set; }
            public string name { get; set; }
            public string text { get; set; }
            public string value { get; set; }
            public string cost_cen { get; set; }
            public string comp { get; set; }
        }

        
    }
}