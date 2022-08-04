using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Service.EduService;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class EduMasterDataController : Controller
    {
        private TM_FacultyService _TM_FacultyService;
        private TM_UniversitysService _TM_UniversitysService;
        private TM_MajorService _TM_MajorService;
        public EduMasterDataController(TM_FacultyService TM_FacultyService
            , TM_UniversitysService TM_UniversitysService
            , TM_MajorService TM_MajorService)
        {
            _TM_FacultyService = TM_FacultyService;
            _TM_UniversitysService = TM_UniversitysService;
            _TM_MajorService = TM_MajorService;
        }
        // GET: EduMasterData
        public ActionResult CreateddlFaculty(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_FacultyService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.faculty_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.faculty_name_en }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult CreateddlFacultyByUniver(vSelect item)
        {
            vSelect lstData = new vSelect();
            if (!string.IsNullOrEmpty(item.value))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(item.value));
                var lst = _TM_UniversitysService.Find(nId);
                if (lst != null && lst.TM_Universitys_Faculty.Where(w => w.active_status == "Y").Any())
                {
                    lstData.databind = item.databind + "";
                    lstData.id = item.id + "";
                    lstData.value = item.value + "";
                    lstData.disable = item.disable;
                    lstData.lstData = lst.TM_Universitys_Faculty.Where(w => w.active_status == "Y").OrderBy(o => o.universitys_faculty_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.universitys_faculty_name_en + "(" + s.TM_Faculty.faculty_name_en + ")" }).ToList();
                }
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlMajor(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_MajorService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.major_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.major_name_en }).ToList();
            }
            return PartialView("_select", lstData);
        }


    }
}