using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ProbationVM
{
    public class vProbation_Form_obj
    {
        public string View { get; set; }
        public string Edit { get; set; }
        public string Update { get; set; }
        public int Id { get; set; }
        public string Assessment { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string Probation_Active { get; set; }
        public string Provident_Fund { get; set; }
        public string Mail_Send { get; set; }
        public string Start_Pro { get; set; }
        public string End_Pro { get; set; }
        public string Count_Date_Pro { get; set; }
        public string HR_No { get; set; }
        public string HR_Submit_Date { get; set; }
        public string Staff_No { get; set; }
        public string Staff_Acknowledge_Date { get; set; }
        public string PM_No { get; set; }
        public string PM_Submit_Date { get; set; }
        public string GroupHead_No { get; set; }
        public string GroupHead_Submit_Date { get; set; }
        public string HOP_No { get; set; }
        public string HOP_Submit_Date { get; set; }
        public string Active_Status { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }



        public string Staff_Name { get; set; }
        public string PM_Name { get; set; }
        public string GroupHead_Name { get; set; }
        public string HOP_Name { get; set; }
        public string HR_Name { get; set; }


        public string Position { get; set; }
        public string Cost_Center { get; set; }
        public string Date_Employed { get; set; }
        public string Probationary_Period_End { get; set; }
        public string Extend_Form { get; set; }
        public string Extend_Status { get; set; }
        public string Extend_Period { get; set; }

        public string You_Are { get; set; }
        public string Absent { get; set; }
        public string Save_Type { get; set; }
        public string File_Upload { get; set; }
        public string Remark_Revise { get; set; }


        public string Staff_Action { get; set; }
        public string PM_Action { get; set; }
        public string GroupHead_Action { get; set; }
        public string HOP_Action { get; set; }
        public string PM_Cost { get; set; }

        public string Company { get; set; }

    }
    public class vProbation_Form : CSearchProbation
    {
        public List<vProbation_Form> lstData { get; set; }
    }

    public class vProbation_Form_Return : CResutlWebMethod
    {
        public string idEncype { get; set; }
        public vProbation_Form_obj maindata { get; set; }
        public List<vProbation_Form_obj> lstData { get; set; }
        public List<vProbation_Form_obj> lstDataTask { get; set; }
        public List<vProbation_Form_obj> lstDataManage { get; set; }
        public List<vProbation_Detail_obj> lstData_Detail { get; set; }
        public List<vProbation_Question_obj> lstDataQuestion { get; set; }
        public List<vAction_Plans_obj> lstDataActionPlans { get; set; }
    }

}