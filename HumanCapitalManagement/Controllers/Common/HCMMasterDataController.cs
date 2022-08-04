using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Service.AddressService;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.Service.EduService;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HumanCapitalManagement.Service.PreInternAssessment;
using HumanCapitalManagement.Service.VisaExpiry;

namespace HumanCapitalManagement.Controllers.Common
{
    public class HCMMasterDataController : Controller
    {
        private GroupPermissionService _GroupPermissionService;
        private PoolService _PoolService;
        private RankService _RankService;
        private DivisionService _DivisionService;
        private RequestTypeService _RequestTypeService;
        private EmploymentTypeService _EmploymentTypeService;
        private TM_Candidate_TypeService _TM_Candidate_TypeService;
        private TM_Recruitment_TeamService _TM_Recruitment_TeamService;
        private TM_Candidate_StatusService _TM_Candidate_StatusService;
        private TM_Candidate_RankService _TM_Candidate_RankService;
        private TM_TIF_StatusService _TM_TIF_StatusService;
        private TM_SourcingChannelService _TM_SourcingChannelService;
        private TM_CountryService _TM_CountryService;
        private TM_CityService _TM_CityService;
        private TM_DistrictService _TM_DistrictService;
        private TM_SubDistrictService _TM_SubDistrictService;
        private TM_UniversitysService _TM_UniversitysService;
        private TM_Universitys_FacultyService _TM_Universitys_FacultyService;
        private TM_Universitys_MajorService _TM_Universitys_MajorService;
        private TM_Education_DegreeServices _TM_Education_DegreeServices;
        private TM_GenderService _TM_GenderService;
        private TM_MaritalStatusService _TM_MaritalStatusService;
        private TM_NationalitiesService _TM_NationalitiesService;
        private TM_TechnicalTestService _TM_TechnicalTestService;
        private TM_PrefixService _TM_PrefixService;
        private TM_PInternAssessment_ActivitiesService _TM_PInternAssessment_ActivitiesService;
        private TM_PIntern_StatusService _TM_PIntern_StatusService;
        private TM_Type_DocumentService _TM_Type_DocumentService;
        private TM_Prefix_VisaService _TM_Prefix_VisaService;
        private TM_Company_VisaService _TM_Company_VisaService;
        private TM_Remark_VisaService _TM_Remark_VisaService;

        public HCMMasterDataController(GroupPermissionService GroupPermissionService
            , PoolService PoolService
            , DivisionService DivisionService
            , RequestTypeService RequestTypeService
            , RankService RankService
            , EmploymentTypeService EmploymentTypeService
            , TM_Candidate_TypeService TM_Candidate_TypeService
            , TM_Recruitment_TeamService TM_Recruitment_TeamService
            , TM_Candidate_StatusService TM_Candidate_StatusService
            , TM_Candidate_RankService TM_Candidate_RankService
            , TM_TIF_StatusService TM_TIF_StatusService
            , TM_SourcingChannelService TM_SourcingChannelService
            , TM_CountryService TM_CountryService
            , TM_CityService TM_CityService
            , TM_DistrictService TM_DistrictService
            , TM_SubDistrictService TM_SubDistrictService
            , TM_UniversitysService TM_UniversitysService
            , TM_Universitys_FacultyService TM_Universitys_FacultyService
            , TM_Universitys_MajorService TM_Universitys_MajorService
            , TM_Education_DegreeServices TM_Education_DegreeServices
            , TM_GenderService TM_GenderService
            , TM_MaritalStatusService TM_MaritalStatusService
            , TM_NationalitiesService TM_NationalitiesService
            , TM_TechnicalTestService TM_TechnicalTestService
            , TM_PrefixService TM_PrefixService
            , TM_PInternAssessment_ActivitiesService TM_PInternAssessment_ActivitiesService
            , TM_PIntern_StatusService TM_PIntern_StatusService
            , TM_Type_DocumentService TM_Type_DocumentService
            , TM_Prefix_VisaService TM_Prefix_VisaService
            , TM_Company_VisaService TM_Company_VisaService
            , TM_Remark_VisaService TM_Remark_VisaService


            )
        {
            _GroupPermissionService = GroupPermissionService;
            _PoolService = PoolService;
            _RankService = RankService;
            _DivisionService = DivisionService;
            _RequestTypeService = RequestTypeService;
            _EmploymentTypeService = EmploymentTypeService;
            _TM_Candidate_TypeService = TM_Candidate_TypeService;
            _TM_Recruitment_TeamService = TM_Recruitment_TeamService;
            _TM_Candidate_StatusService = TM_Candidate_StatusService;
            _TM_Candidate_RankService = TM_Candidate_RankService;
            _TM_TIF_StatusService = TM_TIF_StatusService;
            _TM_SourcingChannelService = TM_SourcingChannelService;
            _TM_CountryService = TM_CountryService;
            _TM_CityService = TM_CityService;
            _TM_DistrictService = TM_DistrictService;
            _TM_SubDistrictService = TM_SubDistrictService;
            _TM_UniversitysService = TM_UniversitysService;
            _TM_Universitys_FacultyService = TM_Universitys_FacultyService;
            _TM_Universitys_MajorService = TM_Universitys_MajorService;
            _TM_Education_DegreeServices = TM_Education_DegreeServices;
            _TM_GenderService = TM_GenderService;
            _TM_MaritalStatusService = TM_MaritalStatusService;
            _TM_NationalitiesService = TM_NationalitiesService;
            _TM_TechnicalTestService = TM_TechnicalTestService;
            _TM_PrefixService = TM_PrefixService;
            _TM_PInternAssessment_ActivitiesService = TM_PInternAssessment_ActivitiesService;
            _TM_PIntern_StatusService = TM_PIntern_StatusService;
            _TM_Type_DocumentService = TM_Type_DocumentService;
            _TM_Prefix_VisaService = TM_Prefix_VisaService;
            _TM_Company_VisaService = TM_Company_VisaService;
            _TM_Remark_VisaService = TM_Remark_VisaService;


        }

        //function for ddl division
        public ActionResult CreateDivision(vSelect item)
        {
            vSelect lstData = new vSelect();
            if (CGlobal.UserInfo != null)
            {
                if (CGlobal.GetDivision().Any())
                {
                    var lst = CGlobal.GetDivision().ToList();
                    if (lst != null && lst.Any())
                    {
                        lstData.databind = item.databind + "";
                        lstData.id = item.id + "";
                        lstData.value = item.value + "";
                        lstData.disable = item.disable;
                        lstData.lstData = lst.OrderBy(o => o.sName).Select(s => new lstDataSelect { value = s.sName + "", text = s.sName }).ToList();
                    }
                }
            }
            return PartialView("_selectDivision", lstData);
        }

        //public ActionResult Create


        //function for ddl division id is Value
        public ActionResult CreateDivisionCode(vSelect item)
        {
            vSelect lstData = new vSelect();
            if (CGlobal.UserInfo != null)
            {
                if (CGlobal.GetDivision().Any())
                {
                    var lst = CGlobal.GetDivision().Where(w => w.sCompany_code == "4100").ToList();
                    if (lst != null && lst.Any())
                    {
                        lstData.databind = item.databind + "";
                        lstData.id = item.id + "";
                        lstData.value = item.value + "";
                        lstData.disable = item.disable;
                        lstData.lstData = lst.OrderBy(o => o.sName).Select(s => new lstDataSelect { value = s.sID + "", text = s.sName }).ToList();
                    }
                }
            }
            return PartialView("_selectDivision", lstData);
        }
        //function for ddl pr statsu
        public ActionResult CreatePrStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            StoreDb db = new StoreDb();
            var lst = db.Status.ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Sort).Select(s => new lstDataSelect { value = s.Id + "", text = s.StatusDesc }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        //function for ddl Group permission
        public ActionResult CreateGroupPermission(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _GroupPermissionService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.group_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.group_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateGroupForSelectBox(vSelectBox item)
        {
            vSelectBox lstData = new vSelectBox();
            lstData.lstData = new List<lstDataSelectBox>();
            var lst = HCMFunc.Get_GroupForSelect().ToList();
            if (lst != null && lst.Any())
            {

                lstData.id = item.id + "";
                lstData.avalue = item.avalue;
                lstData.disable = item.disable;
                lstData.lstData = (from lstItem in lst
                                       // from lstR in RoleDivi.Where(w => w.sID == lstItem.id).DefaultIfEmpty(new lstDivision())
                                   select new lstDataSelectBox
                                   {
                                       value = lstItem.id,
                                       text = lstItem.name,
                                       // nFix = lstR.from_role + "",
                                       Vgroup = lstItem.sgroup,
                                   }
                    ).OrderBy(o => o.text).ToList();
            }
            return PartialView("_selectBox", lstData);
        }
        public ActionResult CreateRequestTypeBox(vSelectBox item)
        {
            vSelectBox lstData = new vSelectBox();
            lstData.lstData = new List<lstDataSelectBox>();
            var lst = _RequestTypeService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {

                lstData.id = item.id + "";
                lstData.avalue = item.avalue;
                lstData.disable = item.disable;
                lstData.lstData = (from lstItem in lst
                                       // from lstR in RoleDivi.Where(w => w.sID == lstItem.id).DefaultIfEmpty(new lstDivision())
                                   select new lstDataSelectBox
                                   {
                                       value = lstItem.Id + "",
                                       text = lstItem.request_type_name_en,
                                   }
                    ).OrderBy(o => o.text).ToList();
            }
            return PartialView("_selectBoxNoGroup", lstData);
        }
        public ActionResult CreateCandidateStatusBox(vCandidateBox item)
        {
            vSelectBox lstData = new vSelectBox();
            lstData.lstData = new List<lstDataSelectBox>();
            if (!string.IsNullOrEmpty(item.status_id))
            {
                int nId = SystemFunction.GetIntNullToZero(item.status_id + "");
                var lst = _TM_Candidate_StatusService.GetDataForBox(nId).ToList();
                if (lst != null && lst.Any())
                {
                    lstData.id = item.id + "";
                    lstData.avalue = item.avalue;
                    lstData.disable = item.disable;
                    lstData.lstData = (from lstItem in lst.OrderBy(o => o.seq)
                                           // from lstR in RoleDivi.Where(w => w.sID == lstItem.id).DefaultIfEmpty(new lstDivision())
                                       select new lstDataSelectBox
                                       {
                                           value = lstItem.Id + "",
                                           text = lstItem.candidate_status_name_en,
                                       }
                        ).ToList();
                }
            }
            return PartialView("_selectBoxNoGroup", lstData);
        }
        public ActionResult CreatePool(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _PoolService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Pool_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.Pool_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateRank(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _RankService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.piority).Select(s => new lstDataSelect { value = s.Id + "", text = s.rank_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateGroupFromHRIS(vSelect item)
        {
            New_HRISEntities dbHr = new New_HRISEntities();
            vSelect lstData = new vSelect();
            int[] aUnitCode = _DivisionService.GetAll().AsEnumerable().Select(s => SystemFunction.GetIntNullToZero(s.division_code)).ToArray();
            var lst = dbHr.tbMaster_UnitGroup.Where(w => w.IsActive == true && !aUnitCode.Contains(w.ID)).OrderBy(o => o.Name).ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Name).Select(s => new lstDataSelect { value = s.ID + "", text = s.ID + " : " + s.Name }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlEmploymentType(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _EmploymentTypeService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.employee_type_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.employee_type_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlIDCard_Type(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Candidate_TypeService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.candidate_type_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.candidate_type_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlRecruitment_Team(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Recruitment_TeamService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                New_HRISEntities dbHr = new New_HRISEntities();
                string[] User = lst.Select(s => s.user_no).ToArray();
                var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = (from l in lst
                                   from lstEmp in _gettUser.Where(w => w.EmpNo == l.user_no).DefaultIfEmpty(new AllInfo_WS())
                                   select new lstDataSelect
                                   {
                                       value = l.user_no + "",
                                       text = lstEmp.EmpFullName
                                   }).OrderBy(o => o.text).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlCandidate_Status(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Candidate_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.candidate_status_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        public ActionResult CreateddlDegreeEdu(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Education_DegreeServices.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.degree_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.degree_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        public ActionResult CreateddlPrefixEN(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_PrefixService.GetDataForSelect().ToList();


            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.PrefixNameEN }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult Createddl_VisaPrefixEN(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Prefix_VisaService.GetDataForSelect().ToList();


            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.PrefixNameEN }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult Createddl_VisaRemark(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Remark_VisaService.GetDataForSelect().ToList();


            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.RemarkEN }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateCompany(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Company_VisaService.GetDataForSelect().ToList();


            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.company_name }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        public ActionResult CreateddlPrefixTH(vSelect item)
        {
            vSelect lstData = new vSelect();

            var lst = _TM_PrefixService.GetDataForSelect().ToList();

            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.PrefixNameTH }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }


        public ActionResult CreateddlCountryOfBirth(vSelect item)
        {
            vSelect lstData = new vSelect();

            var lst = _TM_CountryService.GetDataForSelect().ToList();

            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.country_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.country_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }


        public ActionResult CreateddlNationalities(vSelect item)
        {
            vSelect lstData = new vSelect();

            var lst = _TM_NationalitiesService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.NationalitiesNameEN).Select(s => new lstDataSelect { value = s.Id + "", text = s.NationalitiesNameEN }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }


        public ActionResult CreateddlMaritalStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_MaritalStatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).ThenBy(t => t.MaritalStatusId).Select(s => new lstDataSelect { value = s.Id + "", text = s.MaritalStatusNameEN }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        public ActionResult CreateddlGender(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_GenderService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).ThenBy(t => t.GenderId).Select(s => new lstDataSelect { value = s.Id + "", text = s.GenderNameEN }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        public ActionResult CreateddlTestName(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_TechnicalTestService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).ThenBy(t => t.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.Test_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        public ActionResult CreateddlSourcingChannel(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_SourcingChannelService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).ThenBy(t => t.sourcingchannel_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.sourcingchannel_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlCanStatusForChange(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Candidate_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.candidate_status_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlCanRank(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Candidate_RankService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.piority).Select(s => new lstDataSelect { value = s.Id + "", text = s.crank_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlTIFStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_TIF_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.tif_status_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateCountry(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_CountryService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.country_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlPIAStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_PIntern_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.PIntern_short_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }


        public ActionResult CreateddlENCountry(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_CountryService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.country_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.country_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        public ActionResult CreateActivitiesTrainee(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_PInternAssessment_ActivitiesService.GetDataAllActive().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Id).ThenBy(t => t.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.Activities_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        public ActionResult CreateddlCountry(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_CountryService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.country_name_th).Select(s => new lstDataSelect { value = s.Id + "", text = s.country_name_th }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateTypeDocument(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Type_DocumentService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.type_docname_eng }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlCity(vSelect item)
        {
            vSelect lstData = new ViewModel.CommonVM.vSelect();
            var lst = _TM_CityService.GetDataForSelect().ToList();
            //_TM_CountryService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.city_name_th }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlDistrict(vSelect item)
        {
            vSelect lstData = new ViewModel.CommonVM.vSelect();
            var lst = _TM_DistrictService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.district_name_th }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }
        public ActionResult CreateddlUniversity(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_UniversitysService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.universitys_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.universitys_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }


        public ActionResult CreatelstUniversity(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_UniversitysService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.universitys_name_en).Select(s => new lstDataSelect { value = s.Id + "", text = s.universitys_name_en }).ToList();
            }
            return PartialView("_selectDivision", lstData);
        }

        [HttpPost]
        public JsonResult UserAutoCompleteAll(string SearchItem, string sQueryID)
        {
            New_HRISEntities dbHr = new New_HRISEntities();
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));

            var _getData = sQuery.Where(w =>
            (
            (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
            ).Contains((SearchItem + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
            ).Take(15).ToList();
            if (_getData.Any())
            {
                string[] aNo = _getData.Select(s => s.Employeeno).ToArray();
                var _getUnit = dbHr.JobInfo.Where(w => aNo.Contains(w.Employeeno)).ToList();
                var _getEmp = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();
                result = (from lstGt in _getData
                          from lstUnit in _getEmp.Where(w => w.EmpNo == lstGt.Employeeno).DefaultIfEmpty(new AllInfo_WS())
                          select new C_USERS_RETURN
                          {
                              id = lstGt.Employeeno,
                              user_id = lstGt.UserID,
                              user_name = lstGt.Employeename + " " + lstGt.Employeesurname,
                              unit_name = lstUnit.UnitGroup,
                              user_position = lstUnit.Rank,
                              user_rank = lstUnit.Rank,

                          }
                    ).ToList();
                //_getData.Select(s => new C_USERS_RETURN
                //    {
                //        id = s.Employeeno,
                //        user_id = s.UserID,
                //        user_name = s.Employeename,
                //        user_last_name = s.Employeesurname
                //    }).ToList();
            }
            #endregion
            //return result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UserAutoCompleteAllForPR(string SearchItem, string sQueryID)
        {
            New_HRISEntities dbHr = new New_HRISEntities();
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);

            var _getData = sQuery.Where(w =>
            (
            (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
            ).Contains((SearchItem + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
            ).Take(15).ToList();
            if (_getData.Any())
            {
                string[] aNo = _getData.Select(s => s.Employeeno).ToArray();
                var _getUnit = dbHr.JobInfo.Where(w => aNo.Contains(w.Employeeno)).ToList();
                var _getEmp = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();
                result = (from lstGt in _getData
                          from lstUnit in _getEmp.Where(w => w.EmpNo == lstGt.Employeeno).DefaultIfEmpty(new AllInfo_WS())
                          select new C_USERS_RETURN
                          {
                              id = lstGt.Employeeno,
                              user_id = lstGt.UserID,
                              user_name = lstGt.Employeename + " " + lstGt.Employeesurname,
                              unit_name = lstUnit.UnitGroup,
                              user_position = lstUnit.Rank,

                          }
                    ).ToList();
                //_getData.Select(s => new C_USERS_RETURN
                //    {
                //        id = s.Employeeno,
                //        user_id = s.UserID,
                //        user_name = s.Employeename,
                //        user_last_name = s.Employeesurname
                //    }).ToList();
            }
            #endregion
            //return result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UserAutoCompleteAllStaffAndOutsource(string SearchItem, string sQueryID)
        {
            New_HRISEntities dbHr = new New_HRISEntities();
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            var sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3")).ToList();
            var getos = dbHr.TB_OutSource.Where(w => (w.Active == 1 || w.Active == 3)).ToList();
           var sQueryos = getos.Select((item, index) => new Employee()
            {
                UserID = item.userid+"",
                Employeeno = item.employeeno,
                Employeename = item.employeename + "",
                Employeesurname = item.employeesurname + "",
                EmpThaiName = item.employeename + "",
                EmpThaiSurName = item.employeesurname + ""
            }).ToList();
            sQuery.AddRange(sQueryos);

            var _getData = sQuery.Where(w =>
            (
            (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
            ).Contains((SearchItem + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
            ).Take(15).ToList();
            if (_getData.Any())
            {
                string[] aNo = _getData.Select(s => s.Employeeno).ToArray();
                var _getUnit = dbHr.JobInfo.Where(w => aNo.Contains(w.Employeeno)).ToList();
                var _getEmp = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();
                result = (from lstGt in _getData
                          from lstUnit in _getEmp.Where(w => w.EmpNo == lstGt.Employeeno).DefaultIfEmpty(new AllInfo_WS())
                          select new C_USERS_RETURN
                          {
                              id = lstGt.Employeeno,
                              user_id = lstGt.UserID,
                              user_name = lstGt.Employeename + " " + lstGt.Employeesurname,
                              unit_name = lstGt.Employeeno.Contains("OS") ? "IT Support" : lstUnit.UnitGroup,
                              user_position = lstGt.Employeeno.Contains("OS") ? "IT Support" : lstUnit.Rank,
                              user_rank = lstGt.Employeeno.Contains("OS") ? "OutSource" : lstUnit.Rank,

                          }
                    ).ToList();
                //_getData.Select(s => new C_USERS_RETURN
                //    {
                //        id = s.Employeeno,
                //        user_id = s.UserID,
                //        user_name = s.Employeename,
                //        user_last_name = s.Employeesurname
                //    }).ToList();
            }
            #endregion
            //return result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}