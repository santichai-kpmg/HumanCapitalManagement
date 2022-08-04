using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    public class vHeadcountReport
    {
    }
    #region for report  Classification

    public class vHCPlanAndSummaryReport_Result : CResutlWebMethod
    {
        public string session { get; set; }
        public string fy_year { get; set; }
        public string fysh_year { get; set; }
        public string full_last_year { get; set; }
        public List<vHCPlanAndSummaryReportData> lstData { get; set; }
        public List<vHCPlanAndSummaryReportData_Total> lstDataTotal { get; set; }
    }
    public class vHCPlanAndSummaryReportData
    {
        public string division { get; set; }
        public string sgroup { get; set; }
        public decimal? plan { get; set; }
        public int ac_starting { get; set; }
        public int current_hc { get; set; }
        public int ac_new_hires { get; set; }
        public int ac_resign { get; set; }


    }
    public class vHCPlanAndSummaryReportData_Total
    {
        public string division { get; set; }
        public string sgroup { get; set; }
        public decimal? plan { get; set; }
        public int ac_starting { get; set; }
        public int current_hc { get; set; }
        public int ac_new_hires { get; set; }
        public int ac_resign { get; set; }
    }
    #endregion
}