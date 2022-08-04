using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ReportVM
{
    public class rpvTIFMassReport
    {
    }

    public class vdsCore
    {
        public int? nSeq { get; set; }
        public string sheader { get; set; }
        public string sevidence { get; set; }
        public string sscoring { get; set; }
        public string srating { get; set; }
        public string question { get; set; }
    }
    public class vdsAdInfo_Question
    {
        public string nID { get; set; }
        public int? seq { get; set; }
        public string question { get; set; }
        public string multi_answer { get; set; }
        public string other_answer { get; set; }

    }
}