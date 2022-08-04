using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ReportVM
{
    public class vLeadTime
    {
        public string no { get; set; }
        public string spool { get; set; }
        public string group_name { get; set; }
        public string request_type { get; set; }
        public string position_name { get; set; }
        public int? target { get; set; }
        public int? nfilled { get; set; }
        public string approval_status { get; set; }
        public DateTime? request_date { get; set; }
        public DateTime? approve_date { get; set; }
        public string verbal_date { get; set; }
        public DateTime? bu_pass { get; set; }
        public DateTime? send_mgr { get; set; }
        public DateTime? hiring_a { get; set; }
        public DateTime? date_offer { get; set; }
        public DateTime? accepte_date { get; set; }
        public string candidate_name { get; set; }
        public string rank_grade { get; set; }
        public DateTime? onboard { get; set; }
        public decimal? lead_date { get; set; }
        public decimal? lead_offer { get; set; }
        public string remark { get; set; }
        public string owner_name { get; set; }
        public string owner_no { get; set; }
        public string sourcing { get; set; }
        public string n_group { get; set; }
        public string refno { get; set; }
    }
}