using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.Probation;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.ProbationVM
{
    public class vAction_Plans_obj
    {
        public int Id { get; set; }
        public int? Seq { get; set; }
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Update_User { get; set; }
        public byte[] sfile64 { get; set; }
        public string sfileType { get; set; }
        public string sfile_oldname { get; set; }
        public string sfile_newname { get; set; }
        public string Description { get; set; }

        public virtual Probation_Form Probation_Form { get; set; }
    }
    public class vAction_Plans : CSearchProbation
    {
        public List<vAction_Plans> lstData { get; set; }
    }

    public class vAction_Plans_Return : CResutlWebMethod
    {

        public vAction_Plans_obj maindata { get; set; }
        public List<vAction_Plans_obj> lstData { get; set; }
    }
}