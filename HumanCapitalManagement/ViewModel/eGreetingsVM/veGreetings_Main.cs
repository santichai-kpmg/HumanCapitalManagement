using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.eGreetingsVM
{
 
    public class veGreetings_Main_Main : CSearcheGreetings_Main
    {
        public List<veGreetings_Main_obj> lstData { get; set; }
    }

    public class veGreetings_Main_Return : CResutlWebMethod
    {
        public veGreetings_Main_obj_save maindata { get; set; }
        public List<veGreetings_Main_obj> mainlstdata { get; set; }
        public List<veGreetings_Main_obj> mainlstdatarecive { get; set; }
        public List<veGreetings_Main_obj_save> lstData { get; set; }
        public List<veGreetings_Question_obj> lstDataquestion { get; set; }
        public string Startddl { get; set; }
        public string Endddl { get; set; }

        public string MainUse { get; set; }
        public string DetailUse { get; set; }


    }
    public class veGreetings_Main_obj
    {
        public int Id { get; set; }
        public DateTime? Start_Peroid { get; set; }
        public DateTime? End_Peroid { get; set; }
        public string Emp_No { get; set; }
        public int Remaining_Rights { get; set; }
        public string Status { get; set; }
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Update_User { get; set; }
        public List<veGreetings_Detail_obj> Probation_Details { get; set; }
        public string Remark { get; set; }

        public string TM_eGreetings_Peroid_Id { get; set; }


        public string Quota { get; set; }
        public string Give { get; set; }
        public string Recive { get; set; }

        public string Emp_Name { get; set; }
        public string Group { get; set; }
       
        
    }


    public class veGreetings_Main_obj_save : CResutlWebMethod
    {
        public string Id { get; set; }
        public string Start_Peroid { get; set; }
        public string End_Peroid { get; set; }
        public string Emp_No { get; set; }
        public string Remaining_Rights { get; set; }
        public string Status { get; set; }
        public string Active_Status { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }
        public List<veGreetings_Detail_obj> eGreetings_Detail { get; set; }
        public List<veGreetings_Detail_obj> eGreetings_Reason { get; set; }
        public List<veGreetings_Detail_obj> eGreetings_Certificate { get; set; }
        public string Remark { get; set; }


        public string image_path { get; set; }
        public string param { get; set; }
        public int receive { get; set; }
        public string emp_name { get; set; }
        public string emp_costcenter { get; set; }
    }

    
}