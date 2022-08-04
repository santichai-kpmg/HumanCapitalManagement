using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ProbationVM
{
    public class vProbation_With_Out_obj
    {
        public string View { get; set; }
        public string Edit { get; set; }
        public int Id { get; set; }
       
        public string Remark { get; set; }

        public string Staff_No { get; set; }
        public string Staff_Name { get; set; }
       
        public string Active_Status { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }
        public string Company { get; set; }
        public string Cost_Center { get; set; }
        public string Join_Date { get; set; }


    }
    public class vProbation_With_Out : CSearchProbation
    {
        public List<vProbation_With_Out> lstData { get; set; }
    }

    public class vProbation_With_Out_Return : CResutlWebMethod
    {
        public string idEncype { get; set; }
        public vProbation_With_Out_obj maindata { get; set; }
        public List<vProbation_With_Out_obj> lstData { get; set; }
     
    }

}