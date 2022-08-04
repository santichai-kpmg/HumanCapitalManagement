using HumanCapitalManagement.ViewModel.MainVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.CommonVM
{
    public class File_Upload
    {
        public byte[] sfile64 { get; set; }
        public string sfileType { get; set; }
        public string sfile_name { get; set; }
        public List<vCandidateTemp> vCandidateTemp { get; set; }
        public List<vTempTransaction> vTempTransaction { get; set; }

    }
}