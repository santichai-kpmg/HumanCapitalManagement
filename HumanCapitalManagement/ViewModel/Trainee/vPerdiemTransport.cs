using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.Trainee
{
    public static class vPerdiemTransport
    {
        public class vdata_sorce_Return : CResutlWebMethod
        {
            public List<vPerdiem_Transport> lstData { get; set; }
        }
        public class vPerdiem_Transport
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

        }

       
    }
}