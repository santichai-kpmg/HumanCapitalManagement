using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    public class vCurrentStaffReport
    {
    }


    public class vCurrentStaffReport_Result : CResutlWebMethod
    {
        public string session { get; set; }
        public List<vCurrentStaffReportData> lstData { get; set; }

    }
    public class vCurrentStaffReportData
    {
        public string country { get; set; }
        public string type_name { get; set; }
        public string division { get; set; }
        public string sgroup { get; set; }
        public int para { get; set; }
        public int aa { get; set; }
        public int sr { get; set; }
        public int am { get; set; }
        public int mgr { get; set; }
        public int ad { get; set; }
        public int dir { get; set; }
        public int ptr { get; set; }
        public int total { get; set; }
    }
}