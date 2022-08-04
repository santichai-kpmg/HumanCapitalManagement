using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ReportVM
{

    public class vStaffMovement : CResutlWebMethod
    {
        public string session { get; set; }
        public string fy_year { get; set; }
        public string fysh_year { get; set; }
        public string full_last_year { get; set; }
        public string mtd_start { get; set; }
        public List<vStaffMovementReportData> lstData { get; set; }
        public List<vCurrentStaffReportData> lstPlan { get; set; }
    }
    public class vStaffMovementReportData
    {
        public string division { get; set; }
        public string sgroup { get; set; }
        public decimal? plan { get; set; }
        public int? ac_starting { get; set; }
        public int? current_hc { get; set; }
        public int? ac_new_hires { get; set; }
        public int? ac_resign { get; set; }
        public int? new_join { get; set; }
        public int? resigned { get; set; }
        public int? transferred_in { get; set; }
        public int? transferred_out { get; set; }
        public int? ending { get; set; }
        public int? fy_plan { get; set; }
        public int? over_under { get; set; }
        public string country { get; set; }

    }
    public class vStaffMovementReportData_Total
    {
        public string division { get; set; }
        public string sgroup { get; set; }
        public decimal? plan { get; set; }
        public int ac_starting { get; set; }
        public int current_hc { get; set; }
        public int ac_new_hires { get; set; }
        public int ac_resign { get; set; }
        public int? new_join { get; set; }
        public int? resigned { get; set; }
        public int? transferred_in { get; set; }
        public int? transferred_out { get; set; }
        public int? ending { get; set; }
        public int? fy_plan { get; set; }
        public int? over_under { get; set; }
    }
}