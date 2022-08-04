using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ReportVM
{
    public class rpvTIF_Report
    {
    }

    public class rpvTIFReport_Session
    {
        public List<TM_Candidate_TIF> lstData { get; set; }
        public List<TM_Candidate_MassTIF> lstDataMass { get; set; }
        public List<TM_Candidate_PIntern> lstDataPIntern { get; set; }
        public List<TM_Candidate_PIntern_Mass> lstDataPInternMass { get; set; }
    }
    public class rpvTIFList_lst
    {
        public int? no { get; set; }
        public int? seq { get; set; }
        public string fistname { get; set; }
        public string lastname { get; set; }
        public string nickname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string id_card { get; set; }
        public DateTime? start_date { get; set; }
        public int? oxford { get; set; }
        public string first_eva { get; set; }
        public string second_eva { get; set; }
        public string hr_acknow { get; set; }
        public string core1 { get; set; }
        public string core2 { get; set; }
        public string core3 { get; set; }
        public string core4 { get; set; }
        public string core5 { get; set; }
        public string core6 { get; set; }
        public string core7 { get; set; }
        public string core8 { get; set; }
        public string core9 { get; set; }
        public int? core_total { get; set; }
        public string audit1 { get; set; }
        public string audit2 { get; set; }
        public string audit3 { get; set; }
        public string audit4 { get; set; }
        public string audit5 { get; set; }
        public int? audit_total { get; set; }
        public string bu_comment { get; set; }
        public string bu_result { get; set; }
        public string hr_comment { get; set; }
        public string add_info1 { get; set; }
        public string add_info2 { get; set; }
        public string add_info3 { get; set; }
        public string add_info4 { get; set; }
        public string add_info5 { get; set; }
        public string add_info6 { get; set; }
        public string add_info7 { get; set; }
        public string add_info8 { get; set; }
        public string add_info9 { get; set; }
        public string full_name { get; set; }

        public DateTime? interview_date { get; set; }
        public DateTime? hr_setinterview { get; set; }
    }
    public class rpvPIAList_lst
    {
        public string bu_comment { get; set; }
        public string bu_result { get; set; }
        public int? no { get; set; }
        public int? seq { get; set; }
        public string fistname { get; set; }
        public string lastname { get; set; }
        public string nickname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string id_card { get; set; }
        public DateTime? start_date { get; set; }
        public int? oxford { get; set; }
        public string first_eva { get; set; }
        public string mailfirst_eva { get; set; }
        public string second_eva { get; set; }
        public string mailsecond_eva { get; set; }
        public string hr_acknow { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Q5 { get; set; }
        public string Q6 { get; set; }
        public string Q7 { get; set; }
        public string Q8 { get; set; }
        public string Q9 { get; set; }
        public string Q10 { get; set; }
        public string Q11 { get; set; }
        public string Q12 { get; set; }
        public string Q13 { get; set; }
        public string Q14 { get; set; }
        public string Q15 { get; set; }
        public int? Q_total { get; set; }
   
        public string full_name { get; set; }

        public DateTime? interview_date { get; set; }
        public DateTime? hr_setinterview { get; set; }
    }
    public class rpvtif_list_question
    {
        public int? no { get; set; }
        public string squestion { get; set; }
        public int? nSeq { get; set; }
        public string sheader { get; set; }
        public string sremark { get; set; }
        public string srating { get; set; }
        public string stopic { get; set; }
        public string first_eva { get; set; }
        public string second_eva { get; set; }


    }
    public class rpvtif_list_rating
    {
        public string sratingname{ get; set; }
        public string sratingdes { get; set; }
        public string snSeq { get; set; }
    }
}