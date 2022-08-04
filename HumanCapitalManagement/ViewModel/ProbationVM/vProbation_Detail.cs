using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ProbationVM
{
    public class vProbation_Detail_obj
    {
        public int Id { get; set; }
        public int Seq { get; set; }
        public int? TM_Probation_Question_Id { get; set; }
        public string Assessment { get; set; }
        public string Remark { get; set; }

        public string Ans_1 { get; set; }
        public string Ans_2 { get; set; }

        public string Ans_3 { get; set; }

        public string Active_Status { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }

        public string Topic { get; set; }
        public string Content { get; set; }
        public string proposition { get; set; }
        //public string proposition
        //{
        //    get
        //    {


        //        if (!String.IsNullOrEmpty(valueset))
        //        {
        //            return valueset;
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //    set
        //    {
        //        if (!String.IsNullOrEmpty(value))
        //        {
        //            proposition = value;
        //        }
        //        else
        //        {
        //            proposition = "";
        //        }
        //    }

        //}
    }
    public class vProbation_Detail : CSearchProbation
    {
        public List<vProbation_Detail> lstData { get; set; }
    }

    public class vProbation_Detail_Return : CResutlWebMethod
    {

        public vProbation_Detail_obj maindata { get; set; }
        public List<vProbation_Detail_obj> lstData { get; set; }
    }
}