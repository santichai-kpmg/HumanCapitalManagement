using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.ViewModel.MainVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ReportVM
{
    public class rpvTraineeEva
    {
    }
    public class rpvEva_list_question
    {
        public int no { get; set; }
        public int? seq { get; set; }
        public string description { get; set; }
        public string trainee_rating { get; set; }
        public string eva_ratine { get; set; }
        public string comment { get; set; }
        public string sgroup { get; set; }
    }

    public class rpvEva_Session
    {
        public List<TM_Trainee_Eva> lstData { get; set; }
        public List<vTraineeEva_obj> lstTraineeTracking { get; set; }
    }
    public class rpvEva_Rating
    {
        public string sratingname { get; set; }
        public string sratingdes { get; set; }

    }

}