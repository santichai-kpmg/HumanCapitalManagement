using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.Feedbacks.vMiniHeart_Detail2021;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.Feedbacks.MiniHeart_Main2021
{
 
    public class vMiniHeart_Main_Main2021 : CSearchMiniHeart_Main
    {
        public List<vMiniHeart_Main2021_obj> lstData { get; set; }
    }

    public class vMiniHeart_Main2021_Return : CResutlWebMethod
    {
        public vMiniHeart_Main2021_obj_save maindata { get; set; }
        public List<vMiniHeart_Main2021_obj> mainlstdata { get; set; }
        public List<vMiniHeart_Main2021_obj> mainlstdatarecive { get; set; }
        public List<vMiniHeart_Main2021_obj_save> lstData { get; set; }
        public List<vMiniHeart_Question2021_obj> lstDataquestion { get; set; }
        public string Startddl { get; set; }
        public string Endddl { get; set; }

        public string MainUse { get; set; }
        public string DetailUse { get; set; }


    }
    public class vMiniHeart_Main2021_obj
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
        public List<vMiniHeart_Detail2021_obj> Probation_Details { get; set; }
        public string Remark { get; set; }

        public string TM_MiniHeart_Peroid_Id { get; set; }


        public string Quota { get; set; }
        public string Give { get; set; }
        public string Recive { get; set; }

        public string Emp_Name { get; set; }
        public string Group { get; set; }
       
        
    }


    public class vMiniHeart_Main2021_obj_save : CResutlWebMethod
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
        public List<vMiniHeart_Detail2021_obj> MiniHeart_Detail { get; set; }
        public List<vMiniHeart_Detail2021_obj> MiniHeart_Reason { get; set; }
        public List<vMiniHeart_Detail2021_obj> MiniHeart_Certificate { get; set; }
        public string Remark { get; set; }


        public string image_path { get; set; }
        public string param { get; set; }
        public int receive { get; set; }
        public string emp_name { get; set; }
        public string emp_costcenter { get; set; }
    }

    
}