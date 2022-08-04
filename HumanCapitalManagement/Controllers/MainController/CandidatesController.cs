using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.AddressService;
using HumanCapitalManagement.Service.PreInternAssessment;
using HumanCapitalManagement.Service.EduService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.HCMClass;
using HumanCapitalManagement.ViewModel;
using System.IO;
using OfficeOpenXml;
//using static HumanCapitalManagement.ViewModel.vTempTransaction;
using System.Globalization;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class CandidatesController : BaseController
    {
        private TM_CandidatesService _TM_CandidatesService;
        private PersonnelRequestService _PersonnelRequestService;
        private TM_Candidate_TypeService _TM_Candidate_TypeService;
        private TM_Candidate_StatusService _TM_Candidate_StatusService;
        private TM_Recruitment_TeamService _TM_Recruitment_TeamService;
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TM_Candidate_RankService _TM_Candidate_RankService;
        private TM_Candidate_Status_CycleService _TM_Candidate_Status_CycleService;
        private TM_SourcingChannelService _TM_SourcingChannelService;
        private TM_CountryService _TM_CountryService;
        private TM_CityService _TM_CityService;
        private TM_DistrictService _TM_DistrictService;
        private TM_SubDistrictService _TM_SubDistrictService;
        private TM_UniversitysService _TM_UniversitysService;
        private TM_Universitys_FacultyService _TM_Universitys_FacultyService;
        private TM_Universitys_MajorService _TM_Universitys_MajorService;
        private TM_Education_HistoryServices _TM_Education_HistoryServices;
        private TM_Education_DegreeServices _TM_Education_DegreeServices;
        private TM_GenderService _TM_GenderService;
        private TM_MaritalStatusService _TM_MaritalStatusService;
        private TM_TechnicalTestService _TM_TechnicalTestService;
        private TM_TechnicalTestTransactionService _TM_TechnicalTestTransactionService;
        private TM_NationalitiesService _TM_NationalitiesService;
        private TM_PrefixService _TM_PrefixService;
        //private TM_WorkExperienceService _TM_WorkExperienceService;
        private TM_WorkExperienceService _TM_WorkExperienceService;
        private TM_PInternAssessment_ActivitiesService _TM_PInternAssessment_ActivitiesService;

        private New_HRISEntities dbHr = new New_HRISEntities();
        public CandidatesController(TM_CandidatesService TM_CandidatesService
            , PersonnelRequestService PersonnelRequestService
            , TM_Candidate_TypeService TM_Candidate_TypeService
            , TM_Candidate_StatusService TM_Candidate_StatusService
            , TM_Recruitment_TeamService TM_Recruitment_TeamService
            , TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TM_Candidate_RankService TM_Candidate_RankService
            , TM_Candidate_Status_CycleService TM_Candidate_Status_CycleService
            , TM_SourcingChannelService TM_SourcingChannelService
            , TM_CountryService TM_CountryService
            , TM_CityService TM_CityService
            , TM_DistrictService TM_DistrictService
            , TM_SubDistrictService TM_SubDistrictService
            , TM_UniversitysService TM_UniversitysService
            , TM_Universitys_FacultyService TM_Universitys_FacultyService
            , TM_Universitys_MajorService TM_Universitys_MajorService
            , TM_Education_HistoryServices TM_Education_HistoryServices
            , TM_Education_DegreeServices TM_Education_DegreeServices
            , TM_GenderService TM_GenderService
            , TM_MaritalStatusService TM_MaritalStatusService
            , TM_TechnicalTestService TM_TechnicalTestService
            , TM_TechnicalTestTransactionService TM_TechnicalTestTransactionService
            , TM_NationalitiesService TM_NationalitiesService
            , TM_PrefixService TM_PrefixService
            , TM_WorkExperienceService TM_WorkExperienceService
            , TM_PInternAssessment_ActivitiesService TM_PInternAssessment_ActivitiesService
            )

        {
            _TM_CandidatesService = TM_CandidatesService;
            _PersonnelRequestService = PersonnelRequestService;
            _TM_Candidate_TypeService = TM_Candidate_TypeService;
            _TM_Candidate_StatusService = TM_Candidate_StatusService;
            _TM_Recruitment_TeamService = TM_Recruitment_TeamService;
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_Candidate_RankService = TM_Candidate_RankService;
            _TM_Candidate_Status_CycleService = TM_Candidate_Status_CycleService;
            _TM_SourcingChannelService = TM_SourcingChannelService;
            _TM_CountryService = TM_CountryService;
            _TM_CityService = TM_CityService;
            _TM_DistrictService = TM_DistrictService;
            _TM_SubDistrictService = TM_SubDistrictService;
            _TM_UniversitysService = TM_UniversitysService;
            _TM_Universitys_FacultyService = TM_Universitys_FacultyService;
            _TM_Universitys_MajorService = TM_Universitys_MajorService;
            _TM_Education_HistoryServices = TM_Education_HistoryServices;
            _TM_Education_DegreeServices = TM_Education_DegreeServices;
            _TM_GenderService = TM_GenderService;
            _TM_MaritalStatusService = TM_MaritalStatusService;
            _TM_TechnicalTestService = TM_TechnicalTestService;
            _TM_TechnicalTestTransactionService = TM_TechnicalTestTransactionService;
            _TM_NationalitiesService = TM_NationalitiesService;
            _TM_PrefixService = TM_PrefixService;
            _TM_WorkExperienceService = TM_WorkExperienceService;
            _TM_PInternAssessment_ActivitiesService = TM_PInternAssessment_ActivitiesService;
        }

        // GET: Candidates
        public ActionResult CandidateList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            vCandidates result = new vCandidates();
            result.active_status = "Y";
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchCandidates SearchItem = (CSearchCandidates)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchCandidates)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                var lstData = _TM_CandidatesService.GetCandidate(
               SearchItem.name,
               SearchItem.active_status);
                string BackUrl = Uri.EscapeDataString(qryStr);
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vCandidates_obj
                                      {
                                          name_en = lstAD.first_name_en + " " + lstAD.last_name_en,
                                          //name_en = lstAD.sub_group_name_en.StringRemark(70),
                                          //group_name = (lstAD.TM_Divisions.division_name_en + "").StringRemark(70),
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          //description = lstAD.sub_group_description.StringRemark(),
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          //update_user = "test user",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();

                }
            }

            return View(result);
        }
        public ActionResult CandidateEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            #region main code

            vCandidates_obj_Save result = new vCandidates_obj_Save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    result.IdEncrypt = id;
                    var _getData = _TM_CandidatesService.Find(nId);
                    if (_getData != null)
                    {

                        result.PA_EN_country_id = null;
                        result.PAcountry_id = null;
                        result.CA_EN_country_id = null;
                        result.CAcountry_id = null;

                        result.lstPACity = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstPADistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstPASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

                        result.lstCACity = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstCADistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstCASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };


                        result.lstPA_EN_City = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstPA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstPA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

                        result.lstCA_EN_City = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstCA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstCA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

                        result.prefixEN_Id = _getData.prefixEN != null ? _getData.prefixEN.Id + "" : "";
                        result.prefixTH_Id = _getData.prefixTH != null ? _getData.prefixTH.Id + "" : "";


                        result.candidate_name = _getData.first_name_en;
                        result.candidate_id = _getData.id_card;
                        result.candidate_id_type = _getData.TM_Candidate_Type != null ? _getData.TM_Candidate_Type.Id + "" : "";
                        result.candidate_lastname = _getData.last_name_en;

                        result.candidate_FNameTH = _getData.candidate_FNameTH;
                        result.candidate_LNameTH = _getData.candiate_LNameTH;
                        result.candidate_phone = _getData.candidate_phone;
                        result.candidate_NickName = _getData.candidate_NickName;
                        result.candidate_OxfordScore = _getData.candidate_oxfordscore.HasValue ? _getData.candidate_oxfordscore.Value.ToString() : "";
                        result.candidate_BaseSalary = _getData.candidate_BaseSalary.MoneyFormat();
                        result.candidate_TotalYearsOfWorkRelatedToThisPosition = _getData.candidate_TotalYearsOfWorkRelatedToThisPosition.ToString();
                        result.candidate_TotalYearsOfWorkNotRelatedToThisPosition = _getData.candidate_TotalYearsOfWorkNotRelatedToThisPosition.ToString();

                        result.candidate_NMGTestScore = _getData.candidate_NMGTestScore.ToString();
                        result.candidate_NMGTestDate = _getData.candidate_NMGTestDate.HasValue ? _getData.candidate_NMGTestDate.Value.DateTimebyCulture() : "-- / -- / ----";

                        result.candidate_MobileAllowance = _getData.candidate_MobileAllowance.MoneyFormat();
                        result.candidate_TransportationAllowance = _getData.candidate_TransportationAllowance.MoneyFormat();
                        result.candidate_OtherAllowance = _getData.candidate_OtherAllowance.MoneyFormat();
                        result.candidate_AnnualLeave = _getData.candidate_AnnualLeave.ToString();
                        result.candidate_VariableBonus = _getData.candidate_VariableBonus.MoneyFormat();
                        result.candidate_FixedBonus = _getData.candidate_FixedBonus.MoneyFormat();
                        result.candidate_ExpectedSalary = _getData.candidate_ExpectedSalary.MoneyFormat();

                        result.candidate_Company = _getData.candidate_Company;
                        result.candidate_OrgUnit = _getData.candidate_OrgUnit;
                        result.candidate_AlternativeNameTH = _getData.candidate_AlternativeNameTH;
                        result.candidate_BirthPlace = _getData.candidate_BirthPlace;
                        result.candidate_CountryOfBirth = _getData.candidate_CountryOfBirth;



                        result.CurrentOrLatestIndustry = _getData.CurrentOrLatestIndustry;
                        result.CurrentOrLatestCompanyName = _getData.CurrentOrLatestCompanyName;
                        result.CurrentOrLatestPositionName = _getData.CurrentOrLatestPositionName;




                        result.candidate_DeathContribution = _getData.candidate_DeathContribution;
                        result.candidate_CompleteInfoForOnBoard = _getData.candidate_CompleteInfoForOnBoard;

                        result.candidate_EduCountry = _getData.candidate_EduCountry;
                        result.candidate_EduCurrentGPATranscript = _getData.candidate_EduCurrentGPATranscript;
                        result.candidate_EduCurrentOrLatestDegree = _getData.candidate_EduCurrentOrLatestDegree;
                        result.candidate_EduInstituteOrLocationOfTraining = _getData.candidate_EduInstituteOrLocationOfTraining;
                        result.candidate_EduMajorStudy = _getData.candidate_EduMajorStudy;
                        result.candidate_Email = _getData.candidate_Email;

                        result.candidate_ProfessionalQualification = _getData.candidate_ProfessionalQualification;

                        result.CPAPassedStatus = _getData.CPAPassedStatus;
                        result.CPAPassedYear = _getData.CPAPassedYear;
                        result.CPALicenseNo = _getData.CPALicenseNo;


                        result.candidate_IndustryPrerences1 = _getData.candidate_IndustryPrerences1;
                        result.candidate_IndustryPrerences2 = _getData.candidate_IndustryPrerences2;
                        result.candidate_IndustryPrerences3 = _getData.candidate_IndustryPrerences3;
                        result.candidate_IndustryPrerences4 = _getData.candidate_IndustryPrerences4;
                        result.candidate_IndustryPrerences5 = _getData.candidate_IndustryPrerences5;

                        result.candidate_InternalNoteForHRTeam = _getData.candidate_InternalNoteForHRTeam;
                        result.candidate_ProvidentFundTH = _getData.candidate_ProvidentFundTH;
                        result.candidate_SocialSecurityTH = _getData.candidate_SocialSecurityTH;
                        //result.candidate_TraineeNumber = _getData.candidate_TraineeNumber;
                        result.candidate_TypeOfEmployment = _getData.candidate_TypeOfEmployment;
                        result.candidate_EnglishTestScoreOrOxford = _getData.candidate_EnglishTestScoreOrOxford.HasValue ? _getData.candidate_EnglishTestScoreOrOxford.Value.ToString() : null;


                        result.candidate_BankAccountName = _getData.candidate_BankAccountName;
                        result.candidate_BankAccountNumber = _getData.candidate_BankAccountNumber;
                        result.candidate_EnglishTestName = _getData.candidate_EnglishTestName;
                        result.candidate_EnglishTestDate = _getData.candidate_EnglishTestDate.HasValue ? _getData.candidate_EnglishTestDate.Value.DateTimebyCulture() : "-- / -- / ----";
                        result.candidate_OfficialNote = _getData.candidate_OfficialNote;
                        //result.candidate_Program = _getData.candidate_Program;
                        result.candidate_Email = _getData.candidate_Email;
                        //result.candidate_SourcingChannel_id = _getData.TM_SourcingChannel != null ? _getData.TM_SourcingChannel.Id + "" : null;
                        result.candidate_MaritalStatus = _getData.MaritalStatusName != null ? _getData.MaritalStatusName.Id + "" : null;
                        result.candidate_Nationality = _getData.Nationalities != null ? _getData.Nationalities.Id + "" : null;



                        result.candidate_Gender = _getData.Gender != null ? _getData.Gender.Id + "" : null;
                        result.candidate_CountryOfBirth = _getData.CountryOfBirth != null ? _getData.CountryOfBirth.Id + "" : null;


                        result.candidate_AuditingTestDate = _getData.candidate_AuditingTestDate.HasValue ? _getData.candidate_AuditingTestDate.Value.DateTimeWithTimebyCulture() : "-- / -- / ----";
                        result.candidate_AuditingScore = _getData.candidate_AuditingScore.ToString();
                        result.candidate_DateOfBirth = _getData.candidate_DateOfBirth.HasValue ? _getData.candidate_DateOfBirth.Value.DateTimebyCulture() : "-- / -- / ----";

                        result.candidate_MilitaryServicesDoc = _getData.candidate_MilitaryServicesDoc;
                        result.candidate_DeathContribution = _getData.candidate_DeathContribution;
                        result.candidate_SocialSecurityTH = _getData.candidate_SocialSecurityTH;
                        result.candidate_ProvidentFundTH = _getData.candidate_ProvidentFundTH;
                        result.candidate_CompleteInfoForOnBoard = _getData.candidate_CompleteInfoForOnBoard;
                        result.candidate_IBMP = _getData.candidate_IBMP;

                        result.candidate_CAHouseNo = _getData.candidate_CAHouseNo;
                        result.candidate_CAMobileNumber = _getData.candidate_CAMobileNumber;
                        result.candidate_CAMooAndSoi = _getData.candidate_CAMooAndSoi;
                        result.candidate_CAPostalCode = _getData.candidate_CAPostalCode;
                        result.candidate_CATelephoneNumber = _getData.candidate_CATelephoneNumber;
                        result.candidate_CAStreet = _getData.candidate_CAStreet;


                        result.candidate_PAHouseNo = _getData.candidate_PAHouseNo;
                        result.candidate_PAMooAndSoi = _getData.candidate_PAMooAndSoi;
                        result.candidate_PAPostalCode = _getData.candidate_PAPostalCode;
                        result.candidate_PAStreet = _getData.candidate_PAStreet;

                        result.candidate_PAHouseNo_EN = _getData.candidate_PAHouseNo_EN;
                        result.candidate_PAVillageNoAndAlley_EN = _getData.candidate_PAVillageNoAndAlley_EN;
                        result.candidate_PAStreet_EN = _getData.candidate_PAStreet_EN;
                        result.candidate_PAPostalCode_EN = _getData.candidate_PAPostalCode_EN;
                        result.candidate_PATelephoneNumber_EN = _getData.candidate_PATelephoneNumber_EN;
                        result.candidate_PAMobileNumber_EN = _getData.candidate_PAMobileNumber_EN;

                        result.candidate_CAHouseNo_EN = _getData.candidate_CAHouseNo_EN;
                        result.candidate_CAVillageNoAndAlley_EN = _getData.candidate_CAVillageNoAndAlley_EN;
                        result.candidate_CAStreet_EN = _getData.candidate_CAStreet_EN;
                        result.candidate_CAPostalCode_EN = _getData.candidate_CAPostalCode_EN;
                        result.candidate_CATelephoneNumber_EN = _getData.candidate_CATelephoneNumber_EN;
                        result.candidate_CAMobileNumber_EN = _getData.candidate_CAMobileNumber_EN;




                        if (_getData.PA_EN_TM_SubDistrict != null)
                        {


                            result.PA_EN_subdistrict_id = _getData.PA_EN_TM_SubDistrict != null ? _getData.PA_EN_TM_SubDistrict.Id + "" : null;
                            result.PA_EN_district_id = _getData.PA_EN_TM_SubDistrict.TM_District != null ? _getData.PA_EN_TM_SubDistrict.TM_District.Id + "" : null;
                            result.PA_EN_city_id = _getData.PA_EN_TM_SubDistrict.TM_District.TM_City != null ? _getData.PA_EN_TM_SubDistrict.TM_District.TM_City.Id + "" : null;


                            result.lstPA_EN_SubDistrict = (from lst in _TM_SubDistrictService.FindListBySubDistrictID(_getData.PA_EN_TM_SubDistrict.Id)
                                                           select new vSelect_PR
                                                           {
                                                               name = lst.subdistrict_name_en,
                                                               id = lst.Id + "",
                                                           }).ToList();

                            result.lstPA_EN_District = (from lst in _TM_DistrictService.GetDataById(_getData.PA_EN_TM_SubDistrict.TM_District.Id)
                                                        select new vSelect_PR
                                                        {
                                                            name = lst.district_name_en,
                                                            id = lst.Id + "",
                                                        }).ToList();

                            result.lstPA_EN_City = (from lst in _TM_CityService.GetDataById(_getData.PA_EN_TM_SubDistrict.TM_District.TM_City.Id)
                                                    select new vSelect_PR
                                                    {
                                                        name = lst.city_name_en,
                                                        id = lst.Id + "",
                                                    }).ToList();

                            result.PA_EN_country_id = _getData.PA_EN_TM_SubDistrict.TM_District.TM_City.TM_Country.Id + "";


                        }
                        else
                        {
                            result.lstPA_EN_City = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.lstPA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.lstPA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

                        }
                        /////

                        if (_getData.CA_EN_TM_SubDistrict != null)
                        {


                            result.CA_EN_subdistrict_id = _getData.CA_EN_TM_SubDistrict != null ? _getData.CA_EN_TM_SubDistrict.Id + "" : null;
                            result.CA_EN_district_id = _getData.CA_EN_TM_SubDistrict.TM_District != null ? _getData.CA_EN_TM_SubDistrict.TM_District.Id + "" : null;
                            result.CA_EN_city_id = _getData.CA_EN_TM_SubDistrict.TM_District.TM_City != null ? _getData.CA_EN_TM_SubDistrict.TM_District.TM_City.Id + "" : null;


                            result.lstCA_EN_SubDistrict = (from lst in _TM_SubDistrictService.FindListBySubDistrictID(_getData.CA_EN_TM_SubDistrict.Id)
                                                           select new vSelect_PR
                                                           {
                                                               name = lst.subdistrict_name_en,
                                                               id = lst.Id + "",
                                                           }).ToList();

                            result.lstCA_EN_District = (from lst in _TM_DistrictService.GetDataById(_getData.CA_EN_TM_SubDistrict.TM_District.Id)
                                                        select new vSelect_PR
                                                        {
                                                            name = lst.district_name_en,
                                                            id = lst.Id + "",
                                                        }).ToList();

                            result.lstCA_EN_City = (from lst in _TM_CityService.GetDataById(_getData.CA_EN_TM_SubDistrict.TM_District.TM_City.Id)
                                                    select new vSelect_PR
                                                    {
                                                        name = lst.city_name_en,
                                                        id = lst.Id + "",
                                                    }).ToList();
                            result.CA_EN_country_id = _getData.CA_EN_TM_SubDistrict.TM_District.TM_City.TM_Country.Id + "";
                        }

                        else
                        {
                            result.lstCA_EN_City = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.lstCA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.lstCA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        }


                        //////

                        if (_getData.PA_TM_SubDistrict != null)
                        {


                            result.PAsubdistrict_id = _getData.PA_TM_SubDistrict != null ? _getData.PA_TM_SubDistrict.Id + "" : null;
                            result.PAdistrict_id = _getData.PA_TM_SubDistrict.TM_District != null ? _getData.PA_TM_SubDistrict.TM_District.Id + "" : null;
                            result.PAcity_id = _getData.PA_TM_SubDistrict.TM_District.TM_City != null ? _getData.PA_TM_SubDistrict.TM_District.TM_City.Id + "" : null;


                            result.lstPASubDistrict = (from lst in _TM_SubDistrictService.FindListBySubDistrictID(_getData.PA_TM_SubDistrict.Id)
                                                       select new vSelect_PR
                                                       {
                                                           name = lst.subdistrict_name_th,
                                                           id = lst.Id + "",
                                                       }).ToList();

                            result.lstPADistrict = (from lst in _TM_DistrictService.GetDataById(_getData.PA_TM_SubDistrict.TM_District.Id)
                                                    select new vSelect_PR
                                                    {
                                                        name = lst.district_name_th,
                                                        id = lst.Id + "",
                                                    }).ToList();

                            result.lstPACity = (from lst in _TM_CityService.GetDataById(_getData.PA_TM_SubDistrict.TM_District.TM_City.Id)
                                                select new vSelect_PR
                                                {
                                                    name = lst.city_name_th,
                                                    id = lst.Id + "",
                                                }).ToList();

                            result.PAcountry_id = _getData.PA_TM_SubDistrict.TM_District.TM_City.TM_Country.Id + "";


                        }
                        else
                        {
                            result.lstPACity = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.lstPADistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.lstPASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

                        }


                        if (_getData.CA_TM_SubDistrict != null)
                        {


                            result.CAsubdistrict_id = _getData.CA_TM_SubDistrict != null ? _getData.CA_TM_SubDistrict.Id + "" : null;
                            result.CAdistrict_id = _getData.CA_TM_SubDistrict.TM_District != null ? _getData.CA_TM_SubDistrict.TM_District.Id + "" : null;
                            result.CAcity_id = _getData.CA_TM_SubDistrict.TM_District.TM_City != null ? _getData.CA_TM_SubDistrict.TM_District.TM_City.Id + "" : null;


                            result.lstCASubDistrict = (from lst in _TM_SubDistrictService.FindListBySubDistrictID(_getData.CA_TM_SubDistrict.Id)
                                                       select new vSelect_PR
                                                       {
                                                           name = lst.subdistrict_name_th,
                                                           id = lst.Id + "",
                                                       }).ToList();

                            result.lstCADistrict = (from lst in _TM_DistrictService.GetDataById(_getData.CA_TM_SubDistrict.TM_District.Id)
                                                    select new vSelect_PR
                                                    {
                                                        name = lst.district_name_th,
                                                        id = lst.Id + "",
                                                    }).ToList();

                            result.lstCACity = (from lst in _TM_CityService.GetDataById(_getData.CA_TM_SubDistrict.TM_District.TM_City.Id)
                                                select new vSelect_PR
                                                {
                                                    name = lst.city_name_th,
                                                    id = lst.Id + "",
                                                }).ToList();
                            result.CAcountry_id = _getData.CA_TM_SubDistrict.TM_District.TM_City.TM_Country.Id + "";
                        }
                        else
                        {
                            result.lstCACity = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.lstCADistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.lstCASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        }

                        /*
                        result.lstPA_EN_City = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstPA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstPA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        */

                        //if (_getData.PA_EN_CountryAbroad != null && _getData.PA_EN_TM_SubDistrict  == null)


                        if (_getData.PA_EN_CountryAbroad != null)
                        {
                            result.PA_EN_country_id = _getData.PA_EN_CountryAbroad != null ? _getData.PA_EN_CountryAbroad.Id + "" : null;
                        }


                        //if (_getData.PA_TH_CountryAbroad != null && _getData.PA_TM_SubDistrict == null)
                        if (_getData.PA_TH_CountryAbroad != null)
                        {
                            result.PAcountry_id = _getData.PA_TH_CountryAbroad != null ? _getData.PA_TH_CountryAbroad.Id + "" : null;
                        }


                        //if (_getData.CA_EN_CountryAbroad != null && _getData.CA_EN_TM_SubDistrict == null)
                        if (_getData.CA_EN_CountryAbroad != null)
                        {

                            result.CA_EN_country_id = _getData.CA_EN_CountryAbroad != null ? _getData.CA_EN_CountryAbroad.Id + "" : null;
                        }


                        //if (_getData.CA_TH_CountryAbroad != null && _getData.CA_TM_SubDistrict == null)
                        if (_getData.CA_TH_CountryAbroad_Id != null)
                        {
                            result.CAcountry_id = _getData.CA_TH_CountryAbroad != null ? _getData.CA_TH_CountryAbroad.Id + "" : null;
                        }


                        result.candidate_EducationStartDate = _getData.candidate_EducationStartDate.HasValue ? _getData.candidate_EducationStartDate.Value.DateTimebyCulture() : "-- / -- / ----";
                        result.candidate_EducationEndDate = _getData.candidate_EducationEndDate.HasValue ? _getData.candidate_EducationEndDate.Value.DateTimebyCulture() : "-- / -- / ----";

                        result.candidate_PAMobileNumber = _getData.candidate_PAMobileNumber;

                        result.candidate_EnglishTestStatus = _getData.candidate_EnglishTestStatus;
                        result.candidate_PATelephoneNumber = _getData.candidate_PATelephoneNumber;

                        result.lstTechnicalTestTransaction = (from lstTechTest in _getData.TM_TechnicalTestTransaction.Where(w => w.active_status == "Y")
                                                              select new vcandidatesTechnicalTestTransaction_obj_Save
                                                              {
                                                                  technicaltest_id = lstTechTest.TM_TechnicalTest.Id + "",
                                                                  Test_name_en = lstTechTest.TM_TechnicalTest.Test_name_en,
                                                                  technicaltest_score = lstTechTest.Test_Score.HasValue ? lstTechTest.Test_Score.Value.ToString() : null,
                                                                  technicaltest_date = lstTechTest.Test_Date.HasValue ? lstTechTest.Test_Date.Value.DateTimebyCulture() : "-- / -- / ----",
                                                                  Test_Status = lstTechTest.TM_TechnicalTest.Test_Status,
                                                                  active_status = lstTechTest.active_status,
                                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddTechnicalTest('" + HCMFunc.Encrypt(lstTechTest.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                  Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteTechnicalTest('" + HCMFunc.Encrypt(lstTechTest.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                              }).ToList();



                        result.lstEduHistory = (from lstAD in _getData.TM_Education_History
                                                select new vEduHistory_obj
                                                {
                                                    university_name = lstAD.Id.ToString(),
                                                    major_name = lstAD.TM_Universitys_Major.universitys_major_name_en,
                                                    degree = lstAD.TM_Education_Degree.degree_name_en,
                                                    faculty_name = lstAD.TM_Universitys_Major.TM_Universitys_Faculty.universitys_faculty_name_en,
                                                    education_history_description = lstAD.education_history_description,
                                                    active_status = lstAD.active_status,
                                                    grade = lstAD.grade,
                                                    start_date = lstAD.start_date.HasValue ? lstAD.start_date.Value.DateTimebyCulture() : "",
                                                    end_date = lstAD.end_date.HasValue ? lstAD.end_date.Value.DateTimebyCulture() : "",
                                                    Ref_Cert_ID = lstAD.Ref_Cert_ID + "",
                                                    Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddEducation('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                    //Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteEduHis('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                    Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteEduHis('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                }).ToList();

                        result.lstWorkExpHistory = (from lstAD in _getData.TM_WorkExperience
                                                    select new vWorkExpHistory_obj
                                                    {

                                                        Id = lstAD.Id + "",
                                                        CompanyName = lstAD.CompanyName,
                                                        JobPosition = lstAD.JobPosition,
                                                        StartDate = lstAD.StartDate.HasValue ? lstAD.StartDate.Value.DateTimebyCulture() : "",
                                                        EndDate = lstAD.EndDate.HasValue ? lstAD.EndDate.Value.DateTimebyCulture() : "",
                                                        TypeOfRelatedToJob = lstAD.TypeOfRelatedToJob,
                                                        active_status = lstAD.active_status,
                                                        //base_salary = lstAD.base_salary.ToString(),
                                                        //transportation = lstAD.transportation.ToString(),
                                                        //mobile_allowance = lstAD.mobile_allowance.ToString(),
                                                        //position_allowance = lstAD.position_allowance.ToString(),
                                                        //other_allowance = lstAD.other_allowance.ToString(),
                                                        //annual_leave = lstAD.annual_leave.ToString(),
                                                        //variable_bonus = lstAD.variable_bonus.ToString(),
                                                        //expected_salary = lstAD.expected_salary.ToString(),
                                                        Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddWorkExperience('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                        Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteWorkExperience('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                    }).ToList();


                    }
                }
                else if (id == "0")
                {
                }
            }
            return View(result);

            #endregion

        }

        #region Trainee UserID and Password
        public ActionResult TraineeUserID(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            vCandidates result = new vCandidates();
            result.active_status = "Y";
            //if (!string.IsNullOrEmpty(qryStr))
            //{
            //    CSearchCandidates SearchItem = (CSearchCandidates)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchCandidates)));
            //    string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            //    var lstData = _TM_CandidatesService.GetCandidate(
            //   SearchItem.name,
            //   SearchItem.active_status);
            //    string BackUrl = Uri.EscapeDataString(qryStr);
            //    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
            //    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

            //    if (lstData.Any())
            //    {
            //        result.lstData = (from lstAD in lstData
            //                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user+"").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
            //                          select new vCandidates_obj
            //                          {
            //                              name_en = lstAD.first_name_en + " " + lstAD.last_name_en,
            //                              //name_en = lstAD.sub_group_name_en.StringRemark(70),
            //                              //group_name = (lstAD.TM_Divisions.division_name_en + "").StringRemark(70),
            //                              //active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
            //                              //description = lstAD.sub_group_description.StringRemark(),
            //                              create_user = lstAD.create_user,
            //                              create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
            //                              update_user = lstAD.update_user,
            //                              //update_user = "test user",
            //                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
            //                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
            //                          }).ToList();

            //    }
            //}

            return View(result);

        }
        [HttpPost]
        public ActionResult LoadTraineeList(CSearchCandidates SearchItem)
        {
            VCandidates_Return result = new VCandidates_Return();
            List<vCandidates_obj> lstData_result = new List<vCandidates_obj>();
            var lstData = _TM_CandidatesService.GetTrainee(
            SearchItem.name,
            SearchItem.active_status
            , SearchItem.group_code
            , SearchItem.ref_no
            );
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });

            string BackUrl = Uri.EscapeDataString(qryStr);
            string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
            var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;


            if (lstData.Any())
            {

                lstData_result = (from lstAD in lstData
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vCandidates_obj
                                  {
                                      Id = lstAD.Id,
                                      IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                      name_en = lstAD.candidate_full_name(),
                                      sgroup = lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y").OrderByDescending(o => o.Id).Select(s => s.PersonnelRequest.TM_Divisions.division_name_en).FirstOrDefault() + "",
                                      ref_no = lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y").OrderByDescending(o => o.Id).Select(s => s.PersonnelRequest.RefNo).FirstOrDefault() + "",
                                      user_name = lstAD.candidate_user_id + "",
                                      id_card = lstAD.id_card + "",
                                      spassword = GetPassword(lstAD.first_name_en + "", lstAD.id_card + ""),
                                      //update_user = lstAD.update_user,
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",

                                  }).ToList();
                result.lstData = lstData_result.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        private string GetPassword(string sname, string id_card)
        {
            string sReturn = "";
            if (!string.IsNullOrEmpty(sname) && !string.IsNullOrEmpty(id_card))
            {
                string fPass = (sname + "").Length >= 3 ? (sname.Substring(0, 3)).ToLower() : (sname).ToLower();
                string sPass = (id_card + "").Length >= 3 ? (id_card + "").Substring(0, 3) : (id_card + "");
                int maxleang = (id_card + "").Length;
                string tPass = (id_card + "").Length >= 3 ? (id_card + "").Substring(maxleang - 3) : (id_card + "");

                sReturn = fPass + (sPass.ToLower()) + (tPass.ToLower());
            }
            return sReturn;
        }
        [HttpPost]
        public ActionResult TraineeEditData(VCandidates_Return ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            VCandidates_Return result = new VCandidates_Return();
            List<vAcKnowledge_lst> lstError = new List<vAcKnowledge_lst>();
            if (ItemData != null && ItemData.lstData != null && ItemData.lstData.Any())
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;
                foreach (var item in ItemData.lstData)
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(item.IdEncrypt + ""));
                    if (nId != 0)
                    {
                        var _getCandidate = _TM_CandidatesService.FindAddUploadFile(nId);
                        if (_getCandidate != null)
                        {
                            if (!string.IsNullOrEmpty(_getCandidate.id_card))
                            {
                                _getCandidate.candidate_user_id = ((_getCandidate.first_name_en).Replace(" ", "")).ToLower() + "." + ((_getCandidate.last_name_en).Replace(" ", "")).ToLower();
                                _getCandidate.update_date = dNow;
                                _getCandidate.update_user = CGlobal.UserInfo.UserId;
                                string fPass = (_getCandidate.first_name_en + "").Length >= 3 ? (_getCandidate.first_name_en.Substring(0, 3)).ToLower() : (_getCandidate.first_name_en).ToLower();
                                string sPass = (_getCandidate.id_card + "").Length >= 3 ? (_getCandidate.id_card + "").Substring(0, 3) : (_getCandidate.id_card + "");
                                int maxleang = (_getCandidate.id_card + "").Length;
                                string tPass = (_getCandidate.id_card + "").Length >= 3 ? (_getCandidate.id_card + "").Substring(maxleang - 3) : (_getCandidate.id_card + "");

                                _getCandidate.candidate_password = SHA.GenerateSHA256String(fPass + (sPass.ToLower()) + (tPass.ToLower()));

                                var sComplect = _TM_CandidatesService.Update(_getCandidate);
                            }

                        }
                    }
                }
                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Trainee.";
            }

            return Json(new
            {
                result
            });
        }
        #endregion

        public ActionResult ImportCandidateFile()
        {
            return PartialView("_ImportCandidateFile");
        }

        #region Action Add Button
        public ActionResult AddCandidateTechnicalTest(VCandidate_TechnicalTestTransaction ItemData)
        {
            /* Old
            vcandidatesTechnicalTestTransaction_obj_Save result = new vcandidatesTechnicalTestTransaction_obj_Save();

            if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                int nID = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nID != 0)
                {

                }


            }

            return PartialView("_CandidateTechnicalTest", result);
            */

            vcandidatesTechnicalTestTransaction_obj_Save result = new vcandidatesTechnicalTestTransaction_obj_Save();

            if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                int nID = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nID != 0)
                {
                    var _getData = _TM_TechnicalTestTransactionService.Find(nID);
                    if (_getData != null)
                    {
                        if (_getData.TM_TechnicalTest != null)
                        {
                            result.technicaltest_id = _getData.TM_TechnicalTest.Id + "";
                            result.technicaltest_score = _getData.Test_Score + "";
                            result.technicaltest_date = _getData.Test_Date.HasValue ? _getData.Test_Date.Value.DateTimebyCulture() : "";
                            result.IdEncrypt = ItemData.IdEncrypt;
                        }
                    }
                }
                else
                {

                }

            }

            return PartialView("_CandidateTechnicalTest", result);


        }

        public ActionResult AddEducationHistoryCandidate(VCandidates_EduHistory ItemData)
        {
            vCandidate_university_onchange result = new vCandidate_university_onchange();
            if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                int nID = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nID != 0)
                {
                    result.lstFaculty = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                    result.lstMajor = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

                    var _getData = _TM_Education_HistoryServices.Find(nID);
                    if (_getData != null)
                    {
                        result.IdEncrypt = HCMFunc.Encrypt(_getData.Id + "");
                        if (_getData.TM_Universitys_Major != null)
                        {
                            result.major_id = _getData.TM_Universitys_Major.Id + "";

                            if (_getData.TM_Universitys_Major.TM_Universitys_Faculty != null
                             && _getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys_Major != null
                            && _getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys_Major.Any())
                            {
                                result.lstMajor.AddRange(_getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys_Major.OrderBy(o => o.universitys_major_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.universitys_major_name_en }).ToList());
                            }
                        }
                        if (_getData.TM_Universitys_Major != null
                            && _getData.TM_Universitys_Major.TM_Universitys_Faculty != null
                           )
                        {
                            result.faculty_id = _getData.TM_Universitys_Major.TM_Universitys_Faculty.Id + "";
                            if (_getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys != null
                            && _getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys.TM_Universitys_Faculty != null
                            && _getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys.TM_Universitys_Faculty.Any())
                            {
                                result.lstFaculty.AddRange(_getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys.TM_Universitys_Faculty.OrderBy(o => o.universitys_faculty_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.universitys_faculty_name_en }).ToList());
                            }
                            if (_getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys != null)
                            {
                                result.university_id = _getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys.Id + "";
                            }
                        }
                    }

                    result.university_id = _getData.TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys.universitys_name_en + "";
                    result.grade = _getData.grade.ToString();
                    result.degree = _getData.TM_Education_Degree.Id.ToString();
                    result.education_history_description = _getData.education_history_description;
                    result.Ref_Cert_ID = _getData.Ref_Cert_ID + "";
                    result.start_date = _getData.start_date.HasValue ? _getData.start_date.Value.DateTimebyCulture() : "";
                    result.end_date = _getData.end_date.HasValue ? _getData.end_date.Value.DateTimebyCulture() : "";




                }
                else
                {
                    result.lstFaculty = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                    result.lstMajor = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                }
            }
            else
            {
                result.lstFaculty = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                result.lstMajor = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            }
            return PartialView("_CandidateEducationHistory", result);
        }

        public ActionResult AddWorkExperienceHistoryCandidate(VCandidates_WorkExperienceHistory ItemData)
        {

            vCandidate_WorkExp_onchange result = new vCandidate_WorkExp_onchange();
            if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                result.CompanyName = "IdEncrypt ไม่ Null";
                int nID = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nID != 0)
                {
                    var _getData = _TM_WorkExperienceService.Find(nID);
                    var xxx = _getData.base_salary.HasValue ? _getData.base_salary.Value + "" : "";

                    if (_getData.Id != null)
                    {
                        result.IdEncrypt = HCMFunc.Encrypt(_getData.Id + "");

                        result.Id = _getData.Id + "";
                        result.CompanyName = _getData.CompanyName;
                        result.JobPosition = _getData.JobPosition;
                        result.StartDate = _getData.StartDate.HasValue ? _getData.StartDate.Value.DateTimebyCulture() : "";
                        result.EndDate = _getData.EndDate.HasValue ? _getData.EndDate.Value.DateTimebyCulture() : "";
                        result.TypeOfRelatedToJob = _getData.TypeOfRelatedToJob;
                        //result.active_status = _getData.active_status;
                        result.base_salary = _getData.base_salary.HasValue ? _getData.base_salary.Value + "" : "";
                        result.transportation = _getData.transportation.HasValue ? _getData.transportation.Value + "" : "";
                        result.mobile_allowance = _getData.mobile_allowance.HasValue ? _getData.mobile_allowance.Value + "" : "";
                        result.position_allowance = _getData.position_allowance.HasValue ? _getData.position_allowance.Value + "" : "";
                        result.other_allowance = _getData.other_allowance.HasValue ? _getData.other_allowance.Value + "" : "";
                        result.annual_leave = _getData.annual_leave.HasValue ? _getData.annual_leave.Value + "" : "";
                        result.variable_bonus = _getData.variable_bonus.HasValue ? _getData.variable_bonus.Value + "" : "";
                        result.expected_salary = _getData.expected_salary.HasValue ? _getData.expected_salary.Value + "" : "";
                    }

                }
                else
                {

                }
            }
            else
            {

            }

            return PartialView("_CandidateWorkExp", result);
        }

        #endregion  Action Add Button

        #region Ajax Function

        #region GET Group Data
        [HttpPost]
        public JsonResult GetGroupUniversity(string university_id)
        {
            vCandidates_obj_Save result = new vCandidates_obj_Save();
            result.lstUniversity = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.university_id = "";

            //int nID = SystemFunction.GetIntNullToZero(university_id);
            //if (nID != 0)
            //{
            try
            {
                var _getfaculty = _TM_UniversitysService.GetDataForSelect();

                if (_getfaculty != null && _getfaculty != null && _getfaculty.Any())
                {
                    result.lstUniversity.AddRange(_getfaculty.Where(w => w.active_status == "Y").OrderBy(o => o.universitys_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.universitys_name_en }
                        ).ToList());
                }
            }
            catch (Exception ex)
            {

            }
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetGroupMajor(string faculty_id)
        {
            vCandidates_obj_Save result = new vCandidates_obj_Save();
            result.lstMajor = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.major_id = "";

            int nID = SystemFunction.GetIntNullToZero(faculty_id);
            if (nID != 0)
            {
                var _getfaculty = _TM_Universitys_FacultyService.Find(nID);
                result.faculty_id = _getfaculty.Id.ToString();
                if (_getfaculty != null && _getfaculty.TM_Universitys_Major != null && _getfaculty.TM_Universitys_Major.Any())
                {
                    result.lstMajor.AddRange(_getfaculty.TM_Universitys_Major.Where(w => w.active_status == "Y").OrderBy(o => o.universitys_major_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.universitys_major_name_en }
                        ).ToList());
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGroupFaculty(string university_id)
        {
            vCandidate_university_onchange result = new vCandidate_university_onchange();
            result.lstFaculty = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstMajor = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.faculty_id = "";
            result.major_id = "";

            int nID = SystemFunction.GetIntNullToZero(university_id);
            if (nID != 0)
            {
                var _getUniversitys = _TM_UniversitysService.Find(nID);

                if (_getUniversitys != null && _getUniversitys.TM_Universitys_Faculty != null && _getUniversitys.TM_Universitys_Faculty.Any())
                {

                    result.lstFaculty.AddRange(_getUniversitys.TM_Universitys_Faculty.Where(w => w.active_status == "Y").OrderBy(o => o.universitys_faculty_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.universitys_faculty_name_en }
                        ).ToList());

                }
                //else if (_getUniversitys.type == "H")
                //{
                //    result.lstFaculty.AddRange(_getUniversitys.TM_Universitys_Faculty.Where(w => w.Id == 11).OrderBy(o => o.universitys_faculty_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.universitys_faculty_name_en }
                //        ).ToList());
                //}
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGroupFaculty_By_txt(string university_name)
        {
            vCandidate_university_onchange result = new vCandidate_university_onchange();


            result.faculty_id = "";
            result.major_id = "";

            string nID = university_name;
            if (!string.IsNullOrEmpty(nID))
            {
                result.lstFaculty = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                result.lstMajor = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                var _getUniversitys = _TM_UniversitysService.GetUniversity(nID, "Y").FirstOrDefault();
                //var _getUniversitys = _TM_UniversitysService.Find(_getUniversity.Id);
                result.university_id = _getUniversitys.Id.ToString();
                //if (_getUniversitys.type == "H")
                //{
                //    result.lstFaculty.Add(new vSelect_PR { id = "11", name = "High School" });
                //    result.lstMajor.Add(new vSelect_PR { id = "11", name = "High School" });

                //}
                //else
                //{

                if (_getUniversitys != null && _getUniversitys.TM_Universitys_Faculty != null && _getUniversitys.TM_Universitys_Faculty.Any())
                {
                    result.lstFaculty.AddRange(_getUniversitys.TM_Universitys_Faculty.Where(w => w.active_status == "Y").OrderBy(o => o.universitys_faculty_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.universitys_faculty_name_en }
                        ).ToList());
                }
                //}

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGroupSubDistrict(string districtId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstSubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };


            result.subdistrict_id = " - Select - ";

            int nID = SystemFunction.GetIntNullToZero(districtId);
            if (nID != 0)
            {
                var _getDistrict = _TM_DistrictService.Find(nID);

                if (_getDistrict != null && _getDistrict.TM_SubDistrict != null && _getDistrict.TM_SubDistrict.Any())
                {
                    result.lstSubDistrict.AddRange(_getDistrict.TM_SubDistrict.Where(w => w.active_status == "Y").OrderBy(o => o.subdistrict_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.subdistrict_name_th }
                        ).ToList());
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetGroupPASubDistrict(string districtId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstPASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = " - Select - ", name = " - Select - ", } };


            result.PAsubdistrict_id = " - Select - ";

            int nID = SystemFunction.GetIntNullToZero(districtId);
            if (nID != 0)
            {
                var _getDistrict = _TM_DistrictService.Find(nID);

                if (_getDistrict != null && _getDistrict.TM_SubDistrict != null && _getDistrict.TM_SubDistrict.Any())
                {
                    result.lstPASubDistrict.AddRange(_getDistrict.TM_SubDistrict.Where(w => w.active_status == "Y").OrderBy(o => o.subdistrict_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.subdistrict_name_th }
                        ).ToList());
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetGroupPA_EN_SubDistrict(string districtENId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstPA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = " - Select - ", name = " - Select - ", } };

            result.PA_EN_subdistrict_id = " - Select - ";

            int nID = SystemFunction.GetIntNullToZero(districtENId);
            if (nID != 0)
            {
                var _getDistrictEN = _TM_DistrictService.Find(nID);

                if (_getDistrictEN != null && _getDistrictEN.TM_SubDistrict != null && _getDistrictEN.TM_SubDistrict.Any())
                {
                    result.lstPA_EN_SubDistrict.AddRange(_getDistrictEN.TM_SubDistrict.Where(w => w.active_status == "Y").OrderBy(o => o.subdistrict_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.subdistrict_name_en }
                        ).ToList());
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetGroupCASubDistrict(string districtId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstCASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.CAsubdistrict_id = "";

            int nID = SystemFunction.GetIntNullToZero(districtId);
            if (nID != 0)
            {
                var _getDistrict = _TM_DistrictService.Find(nID);

                if (_getDistrict != null && _getDistrict.TM_SubDistrict != null && _getDistrict.TM_SubDistrict.Any())
                {
                    result.lstCASubDistrict.AddRange(_getDistrict.TM_SubDistrict.Where(w => w.active_status == "Y").OrderBy(o => o.subdistrict_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.subdistrict_name_th }
                        ).ToList());
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetGroupCA_EN_SubDistrict(string districtENId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstCA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.CA_EN_subdistrict_id = "";

            int nID = SystemFunction.GetIntNullToZero(districtENId);
            if (nID != 0)
            {
                var _getDistrictEN = _TM_DistrictService.Find(nID);

                if (_getDistrictEN != null && _getDistrictEN.TM_SubDistrict != null && _getDistrictEN.TM_SubDistrict.Any())
                {
                    result.lstCA_EN_SubDistrict.AddRange(_getDistrictEN.TM_SubDistrict.Where(w => w.active_status == "Y").OrderBy(o => o.subdistrict_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.subdistrict_name_en }
                        ).ToList());
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetGroupDistrict(string cityId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstSubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.district_id = "";
            result.subdistrict_id = "";

            int nID = SystemFunction.GetIntNullToZero(cityId);
            if (nID != 0)
            {
                var _getCity = _TM_CityService.Find(nID);

                if (_getCity != null && _getCity.TM_District != null && _getCity.TM_District.Any())
                {
                    result.lstDistrict.AddRange(_getCity.TM_District.Where(w => w.active_status == "Y").OrderBy(o => o.district_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.district_name_th }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGroupPADistrict(string cityId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstPADistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.PAdistrict_id = "";
            result.PAsubdistrict_id = " - Select - ";

            int nID = SystemFunction.GetIntNullToZero(cityId);
            if (nID != 0)
            {
                var _getCity = _TM_CityService.Find(nID);

                if (_getCity != null && _getCity.TM_District != null && _getCity.TM_District.Any())
                {
                    result.lstPADistrict.AddRange(_getCity.TM_District.Where(w => w.active_status == "Y").OrderBy(o => o.district_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.district_name_th }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGroupPA_EN_District(string cityENId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstPA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.PA_EN_district_id = "";
            result.PA_EN_subdistrict_id = " - Select - ";

            int nID = SystemFunction.GetIntNullToZero(cityENId);
            if (nID != 0)
            {
                var _getCityEN = _TM_CityService.Find(nID);

                if (_getCityEN != null && _getCityEN.TM_District != null && _getCityEN.TM_District.Any())
                {
                    result.lstPA_EN_District.AddRange(_getCityEN.TM_District.Where(w => w.active_status == "Y").OrderBy(o => o.district_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.district_name_en }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGroupCADistrict(string cityId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstCADistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstCASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.CAdistrict_id = "";
            result.CAsubdistrict_id = " - Select - ";

            int nID = SystemFunction.GetIntNullToZero(cityId);
            if (nID != 0)
            {
                var _getCity = _TM_CityService.Find(nID);

                if (_getCity != null && _getCity.TM_District != null && _getCity.TM_District.Any())
                {
                    result.lstCADistrict.AddRange(_getCity.TM_District.Where(w => w.active_status == "Y").OrderBy(o => o.district_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.district_name_th }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGroupCA_EN_District(string cityENId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstCA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstCA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.CA_EN_district_id = "";
            result.CA_EN_subdistrict_id = " - Select - ";

            int nID = SystemFunction.GetIntNullToZero(cityENId);
            if (nID != 0)
            {
                var _getCityEN = _TM_CityService.Find(nID);

                if (_getCityEN != null && _getCityEN.TM_District != null && _getCityEN.TM_District.Any())
                {
                    result.lstCA_EN_District.AddRange(_getCityEN.TM_District.Where(w => w.active_status == "Y").OrderBy(o => o.district_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.district_name_en }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGroupPACity(string countryId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstPADistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPACity = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.PAcity_id = "";
            result.PAdistrict_id = "";
            result.PAsubdistrict_id = "";

            int nID = SystemFunction.GetIntNullToZero(countryId);
            if (nID != 0)
            {
                var _getCountry = _TM_CountryService.Find(nID);

                if (_getCountry != null && _getCountry.TM_City != null && _getCountry.TM_City.Any())
                {
                    result.lstPACity.AddRange(_getCountry.TM_City.Where(w => w.active_status == "Y").OrderBy(o => o.city_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.city_name_th }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGroupPA_EN_City(string countryENId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstPA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPA_EN_City = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.PA_EN_city_id = "";
            result.PA_EN_district_id = "";
            result.PA_EN_subdistrict_id = "";

            int nID = SystemFunction.GetIntNullToZero(countryENId);
            if (nID != 0)
            {
                var _getCountryEN = _TM_CountryService.Find(nID);

                if (_getCountryEN != null && _getCountryEN.TM_City != null && _getCountryEN.TM_City.Any())
                {
                    result.lstPA_EN_City.AddRange(_getCountryEN.TM_City.Where(w => w.active_status == "Y").OrderBy(o => o.city_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.city_name_en }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGroupCACity(string countryId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstCADistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstCASubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstCACity = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.CAcity_id = "";
            result.CAdistrict_id = "";
            result.CAsubdistrict_id = "";

            int nID = SystemFunction.GetIntNullToZero(countryId);
            if (nID != 0)
            {
                var _getCountry = _TM_CountryService.Find(nID);

                if (_getCountry != null && _getCountry.TM_City != null && _getCountry.TM_City.Any())
                {
                    result.lstCACity.AddRange(_getCountry.TM_City.Where(w => w.active_status == "Y").OrderBy(o => o.city_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.city_name_th }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGroupCA_EN_City(string countryENId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();


            result.lstCA_EN_City = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstCA_EN_District = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstCA_EN_SubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };



            result.CA_EN_city_id = "";
            result.CA_EN_district_id = "";
            result.CA_EN_subdistrict_id = "";

            int nID = SystemFunction.GetIntNullToZero(countryENId);


            if (nID != 0)
            {
                var _getCountryEN = _TM_CountryService.Find(nID);

                if (_getCountryEN != null && _getCountryEN.TM_City != null && _getCountryEN.TM_City.Any())
                {
                    result.lstCA_EN_City.AddRange(_getCountryEN.TM_City.Where(w => w.active_status == "Y").OrderBy(o => o.city_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.city_name_en }
                        ).ToList());

                }
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGroupCity(string countryId)
        {
            vCandidate_country_onchage result = new vCandidate_country_onchage();

            result.lstDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstSubDistrict = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstCity = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            result.city_id = "";
            result.district_id = "";
            result.subdistrict_id = "";

            int nID = SystemFunction.GetIntNullToZero(countryId);
            if (nID != 0)
            {
                var _getCountry = _TM_CountryService.Find(nID);

                if (_getCountry != null && _getCountry.TM_City != null && _getCountry.TM_City.Any())
                {
                    result.lstCity.AddRange(_getCountry.TM_City.Where(w => w.active_status == "Y").OrderBy(o => o.city_name_th).Select(s => new vSelect_PR { id = s.Id + "", name = s.city_name_th }
                        ).ToList());
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion  GET Group Data

        #region For Candidate

        [HttpPost]
        public ActionResult LoadCandidateList(CSearchCandidates SearchItem)
        {
            VCandidates_Return result = new VCandidates_Return();
            List<vCandidates_obj> lstData_result = new List<vCandidates_obj>();

            //var lstDatas = _TM_CandidatesService.GetCandidate(
            //SearchItem.name,
            //SearchItem.active_status);
            //var lstDatass = _TM_CandidatesService.GetCandidate_Search(new List<string> {
            //SearchItem.name,
            //SearchItem.lastname,
            //SearchItem.idcard,
            //SearchItem.sex,
            //SearchItem.active_status});

            var lstData = _TM_CandidatesService.GetDataForSelect();
            if (!String.IsNullOrEmpty(SearchItem.name))
            {
                //update to search all name 
                lstData = lstData.Where(w => ((w.first_name_en + " " + w.last_name_en).Trim().ToLower()).Contains((SearchItem.name.Trim().ToLower())));
            }
            else if (!String.IsNullOrEmpty(SearchItem.lastname))
            {
                lstData = lstData.Where(w => w.last_name_en.Contains(SearchItem.lastname));
            }
            else if (!String.IsNullOrEmpty(SearchItem.idcard))
            {
                lstData = lstData.Where(w => w.id_card == SearchItem.idcard);
            }
            else if (!String.IsNullOrEmpty(SearchItem.sex))
            {
                lstData = lstData.Where(w => w.Gender_Id.ToString() == SearchItem.sex);
            }
            else if (!String.IsNullOrEmpty(SearchItem.active_status))
            {
                lstData = lstData.Where(w => w.active_status == SearchItem.active_status);
            }


            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });

            string BackUrl = Uri.EscapeDataString(qryStr);
            string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
            var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;


            if (lstData.Any())
            {

                lstData_result = (from lstAD in lstData
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vCandidates_obj
                                  {
                                      name_en = lstAD.first_name_en + " " + lstAD.last_name_en,
                                      //name_en = lstAD.sub_group_name_en.StringRemark(70),
                                      //group_name = (lstAD.TM_Divisions.division_name_en + "").StringRemark(70),
                                      //active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      //description = lstAD.sub_group_description.StringRemark(),
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      //update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();
                result.lstData = lstData_result.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }

        public ActionResult AddShortCandidate()
        {
            vCandidates_obj_Save result = new vCandidates_obj_Save();
            if (CGlobal.UserInfo != null)
            {
                result.recruitment_id = CGlobal.UserInfo.EmployeeNo + "";
                
            }
            return PartialView("_ShortCandidate", result);
        }

        public ActionResult AddOldCandidate()
        {
            vCandidates_obj_Save result = new vCandidates_obj_Save();
            if (CGlobal.UserInfo != null)
            {
                result.recruitment_id = CGlobal.UserInfo.EmployeeNo + "";
            }
            return PartialView("_OldCandidate", result);
        }

        public ActionResult UpdateStatusCandidate(vCandidates_status_save ItemData)
        {
            vCandidates_obj_update result = new vCandidates_obj_update();
            if (ItemData != null && !string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                int nID = SystemFunction.GetIntNullToZero((HCMFunc.Decrypt(ItemData.IdEncrypt + "")).Replace("MPC", ""));
                if (nID != 0)
                {
                    var _getData = _TM_PR_Candidate_MappingService.Find(nID);
                    if (_getData != null)
                    {
                        result.lstStatus = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.IdEncrypt = ItemData.IdEncrypt;
                        result.group_id = _getData.PersonnelRequest.TM_Divisions.division_name_en + "";
                        result.sub_group_id = _getData.PersonnelRequest.TM_SubGroup != null ? _getData.PersonnelRequest.TM_SubGroup.sub_group_name_en : "-";
                        result.rank_id = _getData.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en + "";
                        result.position_id = _getData.PersonnelRequest.TM_Position.position_name_en + "";
                        result.recruitment_id = _getData.TM_Recruitment_Team.user_no + "";
                        result.candidate_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;
                        result.candidate_remark = _getData.description + "";
                        result.candidate_rank_id = _getData.TM_Candidate_Rank != null ? _getData.TM_Candidate_Rank.Id + "" : "";
                        result.active_status = _getData.active_status + "";
                        result.status_addnew = "";
                        result.activities_Id = _getData.TM_PInternAssessment_Activities != null ? _getData.TM_PInternAssessment_Activities.Id + "" : "";
                        if (_getData.TM_Candidate_Status_Cycle != null)
                        {
                            var _GetHistory = _getData.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y").ToList();
                            if (_GetHistory.Any())
                            {
                                result.lstHisStatus = _GetHistory.Select(s => new vCandidates_status_his
                                {
                                    status_name = s.TM_Candidate_Status.candidate_status_name_en + "",
                                    status_date = s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "",
                                }).ToList();
                            }
                        }

                        var _GetLastStatus = _getData.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y").OrderByDescending(o => o.seq).FirstOrDefault();
                        if (_GetLastStatus != null)
                        {
                            int[] aStatus = (_GetLastStatus.TM_Candidate_Status.TM_Candidate_Status_Next != null && _GetLastStatus.TM_Candidate_Status.TM_Candidate_Status_Next.Any()) ?
                                _GetLastStatus.TM_Candidate_Status.TM_Candidate_Status_Next.Select(s => s.next_status_id).ToArray() : null;
                            var _GetNextStatus = _TM_Candidate_StatusService.GetStatusForSave(aStatus);
                            if (_GetNextStatus != null && _GetNextStatus.Any())
                            {
                                result.lstStatus.AddRange(_GetNextStatus.OrderBy(o => o.seq).Select(s => new vSelect_PR { id = s.Id + "", name = s.candidate_status_name_en }).ToList());
                            }

                            result.current_status_id = _GetLastStatus.TM_Candidate_Status.Id + "";
                            if ((int)StatusCandidate.AddNew != _GetLastStatus.TM_Candidate_Status.Id)
                            {
                                result.status_addnew = "N";
                            }

                        }
                    }
                    else
                    {
                        result.IdEncrypt = "";
                    }
                }
                else
                {
                    result.IdEncrypt = "";
                }

            }
            else
            {
                result.IdEncrypt = "";
            }
            return PartialView("_CandidateStatusSingle", result);
        }
        [HttpPost]
        public ActionResult EditCandidate(vCandidates_obj_Save ItemData)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            VCandidates_Return result = new VCandidates_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_CandidatesService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.candidate_name))
                        {
                            int nIDCard_type = SystemFunction.GetIntNullToZero(ItemData.candidate_id_type);
                            //int nIDSourcingChannel = SystemFunction.GetIntNullToZero(ItemData.candidate_SourcingChannel_id);
                            int nPrefixEN = SystemFunction.GetIntNullToZero(ItemData.prefixEN_Id);
                            int nPrefixTH = SystemFunction.GetIntNullToZero(ItemData.prefixTH_Id);


                            int nIDGender = (int)SystemFunction.GetNumberNullToZero(ItemData.candidate_Gender + "");
                            int nMaritalID = (int)SystemFunction.GetNumberNullToZero(ItemData.candidate_MaritalStatus);
                            int nNationalitiesID = (int)SystemFunction.GetNumberNullToZero(ItemData.candidate_Nationality);
                            int nCountryOfBirth = (int)SystemFunction.GetIntNullToZero(ItemData.candidate_CountryOfBirth);


                            int pa_subdistrict_id = SystemFunction.GetIntNullToZero(ItemData.PAsubdistrict_id);
                            int ca_subdistrict_id = SystemFunction.GetIntNullToZero(ItemData.CAsubdistrict_id);
                            int pa_en_subdistrict_id = SystemFunction.GetIntNullToZero(ItemData.PA_EN_subdistrict_id);
                            int ca_en_subdistrict_id = SystemFunction.GetIntNullToZero(ItemData.CA_EN_subdistrict_id);

                            int ca_en_country_id = SystemFunction.GetIntNullToZero(ItemData.CA_EN_country_id);
                            int ca_th_country_id = SystemFunction.GetIntNullToZero(ItemData.CAcountry_id);
                            int pa_en_country_id = SystemFunction.GetIntNullToZero(ItemData.PA_EN_country_id);
                            int pa_th_country_id = SystemFunction.GetIntNullToZero(ItemData.PAcountry_id);


                            int nIDTestName = SystemFunction.GetIntNullToZero(ItemData.candidate_Test_Id);


                            var _getTypeID = _TM_Candidate_TypeService.Find(nIDCard_type);
                            //var _getSourcingChannelID = _TM_SourcingChannelService.Find(nIDSourcingChannel);
                            var _getMaritalID = _TM_MaritalStatusService.Find(nMaritalID);


                            var _getGender = _TM_GenderService.Find(nIDGender);
                            var _getNationalities = _TM_NationalitiesService.Find(nNationalitiesID);
                            var _getCountryOfBirth = _TM_CountryService.Find(nCountryOfBirth);


                            var _getPrefixTH = _TM_PrefixService.Find(nPrefixTH);
                            if (_getPrefixTH != null)
                            {
                                _getData.prefixTH_Id = _getPrefixTH != null ? _getPrefixTH.Id : (int?)null;

                            }
                            else
                            {
                                _getData.prefixTH_Id = null;
                            }



                            var _getPrefixEN = _TM_PrefixService.Find(nPrefixEN);
                            if (_getPrefixEN != null)
                            {
                                _getData.prefixEN_Id = _getPrefixEN != null ? _getPrefixEN.Id : (int?)null;
                            }
                            else
                            {
                                _getData.prefixEN_Id = null;
                            }




                            var _getPASubDistrict = _TM_SubDistrictService.Find(pa_subdistrict_id);

                            if (_getPASubDistrict != null)
                            {
                                _getData.PA_TM_SubDistrict_Id = _getPASubDistrict != null ? _getPASubDistrict.Id : (int?)null;
                            }
                            else
                            {
                                _getData.PA_TM_SubDistrict_Id = null;
                            }

                            var _getPA_TH_CountryAbroad = _TM_CountryService.Find(pa_th_country_id);

                            if (_getPA_TH_CountryAbroad != null)
                            {
                                _getData.PA_TH_CountryAbroad_Id = _getPA_TH_CountryAbroad.Id;
                            }
                            else
                            {
                                _getData.PA_TH_CountryAbroad_Id = null;
                            }



                            var _getPA_EN_SubDistrict = _TM_SubDistrictService.Find(pa_en_subdistrict_id);

                            if (_getPA_EN_SubDistrict != null)
                            {
                                _getData.PA_EN_TM_SubDistrict_Id = _getPA_EN_SubDistrict != null ? _getPA_EN_SubDistrict.Id : (int?)null;
                            }
                            else
                            {
                                _getData.PA_EN_TM_SubDistrict_Id = null;
                            }

                            var _getPA_EN_CountryAbroad = _TM_CountryService.Find(pa_en_country_id);
                            if (_getPA_EN_CountryAbroad != null)
                            {
                                _getData.PA_EN_CountryAbroad_Id = _getPA_EN_CountryAbroad.Id;
                            }
                            else
                            {
                                _getData.PA_EN_CountryAbroad_Id = null;
                            }





                            var _getCA_TH_SubDistrict = _TM_SubDistrictService.Find(ca_subdistrict_id);

                            if (_getCA_TH_SubDistrict != null)
                            {
                                _getData.CA_TM_SubDistrict_Id = _getCA_TH_SubDistrict != null ? _getCA_TH_SubDistrict.Id : (int?)null;
                            }
                            else
                            {
                                _getData.CA_TM_SubDistrict_Id = null;


                            }

                            var _getCA_TH_CountryAbroad = _TM_CountryService.Find(ca_th_country_id);
                            if (_getCA_TH_CountryAbroad != null)
                            {
                                _getData.CA_TH_CountryAbroad_Id = _getCA_TH_CountryAbroad.Id;
                            }
                            else
                            {
                                _getData.CA_TH_CountryAbroad_Id = null;
                            }

                            var _getCA_EN_SubDistrict = _TM_SubDistrictService.Find(ca_en_subdistrict_id);
                            if (_getCA_EN_SubDistrict != null)
                            {
                                _getData.CA_EN_TM_SubDistrict_Id = _getCA_EN_SubDistrict.Id;
                            }
                            else
                            {
                                _getData.CA_EN_TM_SubDistrict_Id = null;
                            }

                            var _getCA_EN_CountryAbroad = _TM_CountryService.Find(ca_en_country_id);
                            if (_getCA_EN_CountryAbroad != null)
                            {
                                _getData.CA_EN_CountryAbroad_Id = _getCA_EN_CountryAbroad.Id;
                            }
                            else
                            {
                                _getData.CA_EN_CountryAbroad_Id = null;
                            }



                            _getData.update_user = dbHr.AllInfo_WS.Where(x => x.UserID.Contains(CGlobal.UserInfo.UserId)).Select(y => y.EmpFullName).FirstOrDefault();

                            _getData.update_date = dNow;
                            _getData.update_user = CGlobal.UserInfo.UserId;

                            _getData.first_name_en = ItemData.candidate_name;
                            _getData.last_name_en = ItemData.candidate_lastname;
                            _getData.candidate_NickName = ItemData.candidate_NickName;
                            _getData.TM_Candidate_Type = _getTypeID != null ? _getTypeID : null;
                            _getData.candidate_FNameTH = ItemData.candidate_FNameTH;
                            _getData.candiate_LNameTH = ItemData.candidate_LNameTH;
                            _getData.candidate_ProfessionalQualification = ItemData.candidate_ProfessionalQualification;
                            _getData.candidate_phone = ItemData.candidate_phone;
                            _getData.candidate_NMGTestDate = SystemFunction.ConvertStringToDateTime(ItemData.candidate_NMGTestDate, "");
                            _getData.candidate_oxfordscore = SystemFunction.GetNumberNullToZero(ItemData.candidate_OxfordScore + "");
                            _getData.candidate_TotalYearsOfWorkRelatedToThisPosition = SystemFunction.GetNumberNullToZero(ItemData.candidate_TotalYearsOfWorkRelatedToThisPosition + "");
                            _getData.candidate_TotalYearsOfWorkNotRelatedToThisPosition = SystemFunction.GetNumberNullToZero(ItemData.candidate_TotalYearsOfWorkNotRelatedToThisPosition + "");
                            _getData.candidate_BaseSalary = SystemFunction.GetNumberNullToZero(ItemData.candidate_BaseSalary + "");
                            _getData.candidate_NMGTestScore = SystemFunction.GetNumberNullToZero(ItemData.candidate_NMGTestScore + "");
                            _getData.candidate_MobileAllowance = SystemFunction.GetNumberNullToZero(ItemData.candidate_MobileAllowance + "");
                            _getData.candidate_CurrentIndustry = ItemData.candidate_CurrentIndustry;
                            _getData.candidate_CurrentPositionName = ItemData.candidate_CurrentPositionName;
                            _getData.candidate_TransportationAllowance = SystemFunction.GetNumberNullToZero(ItemData.candidate_TransportationAllowance + "");
                            _getData.candidate_OtherAllowance = SystemFunction.GetNumberNullToZero(ItemData.candidate_OtherAllowance + "");
                            _getData.candidate_VariableBonus = SystemFunction.GetNumberNullToZero(ItemData.candidate_VariableBonus + "");
                            _getData.candidate_FixedBonus = SystemFunction.GetNumberNullToZero(ItemData.candidate_FixedBonus + "");
                            _getData.candidate_ExpectedSalary = SystemFunction.GetNumberNullToZero(ItemData.candidate_ExpectedSalary + "");
                            _getData.candidate_Company = ItemData.candidate_Company;
                            _getData.candidate_OrgUnit = ItemData.candidate_OrgUnit;
                            _getData.candidate_CostCenter = ItemData.candidate_CostCenter;
                            _getData.candidate_Position = ItemData.candidate_Position;
                            _getData.candidate_TypeOfEmployment = ItemData.candidate_TypeOfEmployment;
                            _getData.candidate_AlternativeNameTH = ItemData.candidate_AlternativeNameTH;
                            _getData.candidate_BirthPlace = ItemData.candidate_BirthPlace;

                            _getData.prefixEN = _getPrefixEN != null ? _getPrefixEN : null;
                            _getData.prefixTH = _getPrefixTH != null ? _getPrefixTH : null;

                            _getData.MaritalStatusName = _getMaritalID != null ? _getMaritalID : null;
                            _getData.Nationalities = _getNationalities != null ? _getNationalities : null;
                            _getData.candidate_MilitaryServicesDoc = ItemData.candidate_MilitaryServicesDoc;
                            _getData.CountryOfBirth = _getCountryOfBirth != null ? _getCountryOfBirth : null;
                            _getData.candidate_PAHouseNo = ItemData.candidate_PAHouseNo;
                            _getData.candidate_PAHouseNo = ItemData.candidate_PAHouseNo;
                            _getData.candidate_PAMooAndSoi = ItemData.candidate_PAMooAndSoi;
                            _getData.candidate_PAPostalCode = ItemData.candidate_PAPostalCode;
                            _getData.candidate_PATelephoneNumber = ItemData.candidate_PATelephoneNumber;
                            _getData.candidate_PAMobileNumber = ItemData.candidate_PAMobileNumber;
                            _getData.candidate_CAHouseNo = ItemData.candidate_CAHouseNo;
                            _getData.candidate_CAMooAndSoi = ItemData.candidate_CAMooAndSoi;
                            _getData.candidate_CAStreet = ItemData.candidate_CAStreet;
                            _getData.candidate_CAPostalCode = ItemData.candidate_CAPostalCode;
                            _getData.candidate_CATelephoneNumber = ItemData.candidate_CATelephoneNumber;
                            _getData.candidate_CAMobileNumber = ItemData.candidate_CAMobileNumber;
                            _getData.candidate_BankAccountName = ItemData.candidate_BankAccountName;
                            _getData.candidate_BankAccountNumber = ItemData.candidate_BankAccountNumber;
                            _getData.candidate_CompleteInfoForOnBoard = ItemData.candidate_CompleteInfoForOnBoard;
                            _getData.candidate_SocialSecurityTH = ItemData.candidate_SocialSecurityTH + "";
                            _getData.candidate_ProvidentFundTH = ItemData.candidate_ProvidentFundTH + "";
                            _getData.candidate_DeathContribution = ItemData.candidate_DeathContribution + "";
                            _getData.candidate_EduInstituteOrLocationOfTraining = ItemData.candidate_EduInstituteOrLocationOfTraining;
                            _getData.candidate_EduCountry = ItemData.candidate_EduCountry;
                            _getData.candidate_EduCurrentGPATranscript = ItemData.candidate_EduCurrentGPATranscript;
                            _getData.candidate_EduCurrentOrLatestDegree = ItemData.candidate_EduCurrentOrLatestDegree;
                            _getData.candidate_EduMajorStudy = ItemData.candidate_EduMajorStudy;
                            //_getData.candidate_TraineeNumber = ItemData.candidate_TraineeNumber;
                            _getData.candidate_Email = ItemData.candidate_Email;
                            //_getData.candidate_Program = ItemData.candidate_Program;
                            _getData.candidate_IndustryPrerences1 = ItemData.candidate_IndustryPrerences1;
                            _getData.candidate_IndustryPrerences2 = ItemData.candidate_IndustryPrerences2;
                            _getData.candidate_IndustryPrerences3 = ItemData.candidate_IndustryPrerences3;
                            _getData.candidate_IndustryPrerences4 = ItemData.candidate_IndustryPrerences4;
                            _getData.candidate_IndustryPrerences5 = ItemData.candidate_IndustryPrerences5;
                            _getData.candidate_OfficialNote = ItemData.candidate_OfficialNote;
                            _getData.candidate_InternalNoteForHRTeam = ItemData.candidate_InternalNoteForHRTeam;
                            _getData.candidate_EnglishTestScoreOrOxford = SystemFunction.GetNumberNullToZero(ItemData.candidate_EnglishTestScoreOrOxford + "");
                            _getData.candidate_EduCurrentOrLatestDegree = ItemData.candidate_EduCurrentOrLatestDegree;
                            _getData.candidate_BankAccountNumber = ItemData.candidate_BankAccountNumber;
                            _getData.candidate_BankAccountName = ItemData.candidate_BankAccountName;
                            _getData.candidate_EnglishTestDate = SystemFunction.ConvertStringToDateTime(ItemData.candidate_EnglishTestDate, "");

                            //_getData.TM_SourcingChannel = _getSourcingChannelID != null ? _getSourcingChannelID : null;
                            _getData.Gender = _getGender != null ? _getGender : null;
                            _getData.candidate_AuditingTestDate = SystemFunction.ConvertStringToDateTime(ItemData.candidate_AuditingTestDate, "");
                            _getData.candidate_AuditingScore = SystemFunction.GetNumberNullToZero(ItemData.candidate_AuditingScore + "");
                            _getData.candidate_EnglishTestName = ItemData.candidate_EnglishTestName;
                            _getData.candidate_EnglishTestStatus = ItemData.candidate_EnglishTestStatus;
                            _getData.candidate_DateOfBirth = SystemFunction.ConvertStringToDateTime(ItemData.candidate_DateOfBirth, "");
                            _getData.id_card = ItemData.candidate_id;
                            _getData.candidate_EducationStartDate = SystemFunction.ConvertStringToDateTime(ItemData.candidate_EducationStartDate, "");
                            _getData.candidate_EducationEndDate = SystemFunction.ConvertStringToDateTime(ItemData.candidate_EducationEndDate, "");

                            _getData.candidate_PAHouseNo_EN = ItemData.candidate_PAHouseNo_EN;
                            _getData.candidate_PAVillageNoAndAlley_EN = ItemData.candidate_PAVillageNoAndAlley_EN;
                            _getData.candidate_PAStreet_EN = ItemData.candidate_PAStreet_EN;
                            _getData.candidate_PAStreet = ItemData.candidate_PAStreet;
                            _getData.candidate_PAPostalCode_EN = ItemData.candidate_PAPostalCode_EN;
                            _getData.candidate_PATelephoneNumber_EN = ItemData.candidate_PATelephoneNumber_EN;
                            _getData.candidate_PAMobileNumber_EN = ItemData.candidate_PAMobileNumber_EN;
                            _getData.candidate_CAHouseNo_EN = ItemData.candidate_CAHouseNo_EN;
                            _getData.candidate_CAVillageNoAndAlley_EN = ItemData.candidate_CAVillageNoAndAlley_EN;
                            _getData.candidate_CAStreet_EN = ItemData.candidate_CAStreet_EN;
                            _getData.candidate_CAPostalCode_EN = ItemData.candidate_CAPostalCode_EN;
                            _getData.candidate_CATelephoneNumber_EN = ItemData.candidate_CATelephoneNumber_EN;
                            _getData.candidate_CAMobileNumber_EN = ItemData.candidate_CAMobileNumber_EN;
                            _getData.candidate_IBMP = ItemData.candidate_IBMP + "";
                            _getData.CPAPassedStatus = ItemData.CPAPassedStatus;
                            _getData.CPAPassedYear = ItemData.CPAPassedYear;
                            _getData.CPALicenseNo = ItemData.CPALicenseNo;

                            _getData.CurrentOrLatestIndustry = ItemData.CurrentOrLatestIndustry;
                            _getData.CurrentOrLatestCompanyName = ItemData.CurrentOrLatestCompanyName;
                            _getData.CurrentOrLatestPositionName = ItemData.CurrentOrLatestPositionName;



                            if (_TM_CandidatesService.CanSave(_getData))
                            {
                                var sComplect = _TM_CandidatesService.Update(_getData);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Duplicate;
                                result.Msg = "Duplicate Type name.";
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter name";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Request Type Not Found.";
                    }
                }

            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult CreateShortCandidate(vCandidates_obj_Add ItemData)
        {
            objCandidates_Return result = new objCandidates_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.objData != null
                    //&& !string.IsNullOrEmpty(ItemData.objData.candidate_id)
                    && !string.IsNullOrEmpty(ItemData.objData.candidate_name)
                    && !string.IsNullOrEmpty(ItemData.objData.candidate_lastname)
                    //&& !string.IsNullOrEmpty(ItemData.objData.candidate_id_type)
                    && !string.IsNullOrEmpty(ItemData.objData.recruitment_id)
                    //&& !string.IsNullOrEmpty(ItemData.objData.activities_Id)
                    )
                {
                    int nPR_ID = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    int nIDCard_type = SystemFunction.GetIntNullToZero(ItemData.objData.candidate_id_type);
                    var _getTypeID = _TM_Candidate_TypeService.Find(nIDCard_type);
                    //if (_getTypeID != null)
                    //{
                    var _GerPR = _PersonnelRequestService.FindForAddCadidate(nPR_ID);
                    if (_GerPR == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, PR not found";
                        return Json(new
                        {
                            result
                        });
                    }
                    else
                    {
                        //if (_GerPR.TM_Employment_Request.TM_Employment_Type.personnel_type + "" == "T" && (!_GerPR.no_of_eva.HasValue || _GerPR.no_of_eva.Value <= 0))
                        //{
                        //    result.Status = SystemFunction.process_Failed;
                        //    result.Msg = "Error, Pleace input No. of Evaluation.";
                        //    return Json(new
                        //    {
                        //        result
                        //    });
                        //}
                    }
                    var _GetRecruit = _TM_Recruitment_TeamService.FindByEmpID(ItemData.objData.recruitment_id);
                    var _GetActivities = _TM_PInternAssessment_ActivitiesService.FindById2(ItemData.objData.activities_Id);
                    if (_GetRecruit == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Recruitment Team not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _GetCandidate = _TM_CandidatesService.FindName(ItemData.objData.candidate_name, ItemData.objData.candidate_lastname);
                    if (_GetCandidate != null)
                    {
                        var _getStatus = _TM_Candidate_StatusService.Find((int)StatusCandidate.AddNew);
                        // กรณี All new Candidate


                        TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                        {

                            seq = 1,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            TM_Candidate_Status = _getStatus,
                            action_date = dNow,
                        };

                        TM_PR_Candidate_Mapping objMapping = new TM_PR_Candidate_Mapping()
                        {

                            ntime = 1,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            PersonnelRequest = _GerPR,
                            TM_Candidate_Status_Cycle = new List<TM_Candidate_Status_Cycle>(),
                            TM_Recruitment_Team = _GetRecruit,
                            TM_Candidates = _GetCandidate,
                            TM_PInternAssessment_Activities = _GetActivities ,

                        };
                        objMapping.TM_Candidate_Status_Cycle.Add(objCycle);
                        if (_TM_PR_Candidate_MappingService.CanCreateMapping(objMapping))
                        {
                            var sComplect = _TM_PR_Candidate_MappingService.CreateNew(objMapping);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getData = _PersonnelRequestService.FindForAddCadidate(nPR_ID);
                                if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any())
                                {

                                    string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                                    var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                                    result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping
                                                            from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                            select new vPRCandidates_lstData
                                                            {
                                                                Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMapping('" + HCMFunc.Encrypt(lstAD.Id + "MPC") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                                candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                                owner_name = lstEmp.EmpFullName,
                                                                rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                                remark = lstAD.description + "",
                                                                action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                                active_status = lstAD.active_status + "" == "Y" ? "Active" : "InActive",
                                                                active_status_id = lstAD.active_status + "",
                                                                Id = HCMFunc.Encrypt(lstAD.Id + "MPC"),
                                                            }).ToList();
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Duplicate;
                            result.Msg = "Duplicate candidate in PR";
                            return Json(new
                            {
                                result
                            });
                        }



                    }
                    else
                    {
                        var _getStatus = _TM_Candidate_StatusService.Find((int)StatusCandidate.AddNew);
                        // กรณี All new Candidate


                        TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                        {
                            Id = 0,
                            seq = 1,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            TM_Candidate_Status = _getStatus,
                            action_date = dNow,
                        };

                        TM_PR_Candidate_Mapping objMapping = new TM_PR_Candidate_Mapping()
                        {
                            Id = 0,
                            ntime = 1,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            PersonnelRequest = _GerPR,
                            TM_Candidate_Status_Cycle = new List<TM_Candidate_Status_Cycle>(),
                            TM_Recruitment_Team = _GetRecruit,
                            TM_PInternAssessment_Activities = _GetActivities,
                        };
                        objMapping.TM_Candidate_Status_Cycle.Add(objCycle);
                        //objMapping.Id = 0;
                        //objMapping.ntime = 1;
                        //ob

                        TM_Candidates objSave = new TM_Candidates()
                        {
                            Id = 0,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            id_card = (ItemData.objData.candidate_id + "").Trim(),
                            first_name_en = (ItemData.objData.candidate_name + "").Trim(),
                            last_name_en = (ItemData.objData.candidate_lastname + "").Trim(),
                            save_success = "N",
                            TM_Candidate_Type = _getTypeID != null ? _getTypeID : null,
                            TM_PR_Candidate_Mapping = new List<TM_PR_Candidate_Mapping>(),
                        };
                        objSave.TM_PR_Candidate_Mapping.Add(objMapping);
                        //add new candidate

                        //map pr candidate first status = 1


                        if (_TM_CandidatesService.CanSave(objSave))
                        {
                            var sComplect = _TM_CandidatesService.CreateNew(objSave);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getData = _PersonnelRequestService.FindForAddCadidate(nPR_ID);
                                if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any())
                                {

                                    string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                                    var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                                    result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping
                                                            from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                            select new vPRCandidates_lstData
                                                            {
                                                                Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMapping('" + HCMFunc.Encrypt(lstAD.Id + "MPC") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                                candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                                owner_name = lstEmp.EmpFullName,
                                                                rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                                remark = lstAD.description + "",
                                                                action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                                active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                                active_status_id = lstAD.active_status + "",
                                                                Id = HCMFunc.Encrypt(lstAD.Id + "MPC"),
                                                            }).ToList();
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Duplicate;
                            result.Msg = "Duplicate ID Card";
                        }
                    }
                


                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please enter name-lastname or recruiment";
                }
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult UpdateStatusCandi(vCandidates_obj_update ItemData)
        {
            objCandidates_Return result = new objCandidates_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData != null && !string.IsNullOrEmpty(ItemData.status_id) && !string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nID = SystemFunction.GetIntNullToZero((HCMFunc.Decrypt(ItemData.IdEncrypt + "")).Replace("MPC", ""));
                    int nStatusID = SystemFunction.GetIntNullToZero(ItemData.status_id);
                    var _getData = _TM_PR_Candidate_MappingService.Find(nID);
                    var _getStatus = _TM_Candidate_StatusService.Find(nStatusID);
                    if (_getData != null && nStatusID != 0 && _getStatus != null)
                    {
                        int prID = _getData.PersonnelRequest.Id;
                        DateTime? action_date = null;
                        if (!string.IsNullOrEmpty(ItemData.action_date))
                        {
                            action_date = SystemFunction.ConvertStringToDateTime(ItemData.action_date, "");
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter action date";
                            return Json(new
                            {
                                result
                            });
                        }

                        if (_getStatus.remark_validate + "" == "Y" && string.IsNullOrEmpty((ItemData.action_remark + "").Trim()))
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter remark status.";
                            return Json(new
                            {
                                result
                            });
                        }

                        #region Edit Recruitment team
                        if (_getData.TM_Recruitment_Team.user_no + "" != ItemData.recruitment_id)
                        {
                            var _GetRecruit = _TM_Recruitment_TeamService.FindByEmpID(ItemData.recruitment_id);
                            if (_GetRecruit != null)
                            {
                                _getData.TM_Recruitment_Team = _GetRecruit;
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Recruitment Team not found.";
                                return Json(new
                                {
                                    result
                                });
                            }
                        }
                        #endregion
                        if (_getData.TM_PInternAssessment_Activities != null)
                        {


                            if (_getData.TM_PInternAssessment_Activities.Id + "" != ItemData.activities_Id)
                            {
                                var _GetActivities = _TM_PInternAssessment_ActivitiesService.FindById2(ItemData.activities_Id);
                                if (_GetActivities != null)
                                {
                                    _getData.TM_PInternAssessment_Activities = _GetActivities;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Activities not found.";
                                    return Json(new
                                    {
                                        result
                                    });
                                }
                            }
                        }
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        //_getData.seq = _getData.seq++;
                        _getData.description = ItemData.candidate_remark;
                        if (!string.IsNullOrEmpty(ItemData.candidate_rank_id))
                        {
                            int nRID = SystemFunction.GetIntNullToZero(ItemData.candidate_rank_id);
                            var _GetRank = _TM_Candidate_RankService.FindForSelect(nRID);
                            if (_GetRank != null)
                            {
                                _getData.TM_Candidate_Rank = _GetRank;
                            }
                        }
                        var _getLastStatus = _getData.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).FirstOrDefault();

                        // กรณี All new Candidate
                        if (_getLastStatus.TM_Candidate_Status.Id != _getStatus.Id)
                        {

                            TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                            {
                                seq = _getLastStatus.seq + 1,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                TM_Candidate_Status = _getStatus,
                                action_date = action_date,
                                description = ItemData.action_remark,
                                TM_PR_Candidate_Mapping = _getData,
                            };

                            var sComplect = _TM_PR_Candidate_MappingService.Update(_getData);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                sComplect = _TM_Candidate_Status_CycleService.CreateNew(objCycle);
                                if (sComplect > 0)
                                {
                                    var _getPR = _PersonnelRequestService.FindForAddCadidate(prID);
                                    if (_getPR.TM_PR_Candidate_Mapping != null && _getPR.TM_PR_Candidate_Mapping.Any())
                                    {

                                        string[] User = _getPR.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                                        var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                                        result.lstCandidates = (from lstAD in _getPR.TM_PR_Candidate_Mapping
                                                                from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                                select new vPRCandidates_lstData
                                                                {
                                                                    Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMapping('" + HCMFunc.Encrypt(lstAD.Id + "MPC") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                    candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                                    candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                                    owner_name = lstEmp.EmpFullName,
                                                                    rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                                    remark = lstAD.description + "",
                                                                    action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                                    active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                                    active_status_id = lstAD.active_status + "",
                                                                    Id = HCMFunc.Encrypt(lstAD.Id + "MPC"),
                                                                }).ToList();
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Can't change status try again.";
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Can't find status.";
                        }


                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, please status type";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Data not found.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult UpdateStatusCandiRemark(vCandidates_obj_update ItemData)
        {
            objCandidates_Return result = new objCandidates_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData != null && !string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nID = SystemFunction.GetIntNullToZero((HCMFunc.Decrypt(ItemData.IdEncrypt + "")).Replace("MPC", ""));
                    int nStatusID = SystemFunction.GetIntNullToZero(ItemData.status_id);
                    var _getData = _TM_PR_Candidate_MappingService.Find(nID);
                    if (_getData != null)
                    {
                        int prID = _getData.PersonnelRequest.Id;
                        #region Edit Recruitment team
                        if (_getData.TM_Recruitment_Team.user_no + "" != ItemData.recruitment_id)
                        {
                            var _GetRecruit = _TM_Recruitment_TeamService.FindByEmpID(ItemData.recruitment_id);
                            if (_GetRecruit != null)
                            {
                                _getData.TM_Recruitment_Team = _GetRecruit;
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Recruitment Team not found.";
                                return Json(new
                                {
                                    result
                                });
                            }
                        }
                        if (ItemData.activities_Id != null)
                        {
                            #endregion
                            if (_getData.TM_PInternAssessment_Activities == null)
                            {
                                var _GetActivities = _TM_PInternAssessment_ActivitiesService.FindById2(ItemData.activities_Id);
                                if (_GetActivities != null)
                                {
                                    _getData.TM_PInternAssessment_Activities = _GetActivities;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Activities not found.";
                                    return Json(new
                                    {
                                        result
                                    });
                                }
                            }
                            else if (_getData.TM_PInternAssessment_Activities.Id + "" != ItemData.activities_Id)
                            {
                                var _GetActivities = _TM_PInternAssessment_ActivitiesService.FindById2(ItemData.activities_Id);
                                if (_GetActivities != null)
                                {
                                    _getData.TM_PInternAssessment_Activities = _GetActivities;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Activities not found.";
                                    return Json(new
                                    {
                                        result
                                    });
                                }
                            }

                        }

                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = ItemData.active_status + "";
                        //_getData.seq = _getData.seq++;
                        _getData.description = ItemData.candidate_remark;
                        if (!string.IsNullOrEmpty(ItemData.candidate_rank_id))
                        {
                            int nRID = SystemFunction.GetIntNullToZero(ItemData.candidate_rank_id);
                            var _GetRank = _TM_Candidate_RankService.FindForSelect(nRID);
                            if (_GetRank != null)
                            {
                                _getData.TM_Candidate_Rank = _GetRank;
                            }
                        }
                        var sComplect = _TM_PR_Candidate_MappingService.Update(_getData);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getPR = _PersonnelRequestService.FindForAddCadidate(prID);
                            if (_getPR.TM_PR_Candidate_Mapping != null && _getPR.TM_PR_Candidate_Mapping.Any())
                            {
                                string[] User = _getPR.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                                var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                                result.lstCandidates = (from lstAD in _getPR.TM_PR_Candidate_Mapping
                                                        from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                        select new vPRCandidates_lstData
                                                        {
                                                            Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMapping('" + HCMFunc.Encrypt(lstAD.Id + "MPC") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                            candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                            candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                            owner_name = lstEmp.EmpFullName,
                                                            rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                            remark = lstAD.description + "",
                                                            action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                            active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                            active_status_id = lstAD.active_status + "",
                                                            Id = HCMFunc.Encrypt(lstAD.Id + "MPC"),
                                                        }).ToList();
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, please status type";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Data not found.";
                }
            }
            return Json(new
            {
                result
            });
        }

        #endregion  For Candidate

        #region TechnicalTest
        [HttpPost]
        public ActionResult SaveCandidateTechnicalTest(vTechnicalTestTransaction_Save ItemData)
        {


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            VCandidatesTechnicalTestTransaction_Return result = new VCandidatesTechnicalTestTransaction_Return();
            if (ItemData != null)
            {
                if (ItemData != null)
                {
                    DateTime dNow = DateTime.Now;
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    if (nId != 0)
                    {
                        var _GetCandidate = _TM_CandidatesService.Find(nId);
                        if (_GetCandidate != null)
                        {
                            int nTechID = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjTechnicalTest.IdEncrypt));
                            int nTechnicalTest = SystemFunction.GetIntNullToZero(ItemData.ObjTechnicalTest.technicaltest_id);
                            var _getTechnicalTest = _TM_TechnicalTestService.Find(nTechnicalTest);
                            var getData = _TM_TechnicalTestTransactionService.Find(nTechID);
                            TM_TechnicalTestTransaction objSave = new TM_TechnicalTestTransaction()
                            {
                                Id = nTechID,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                TM_Candidates = _GetCandidate,
                                TM_TechnicalTest = _getTechnicalTest,
                                Test_Date = SystemFunction.ConvertStringToDateTime(ItemData.ObjTechnicalTest.technicaltest_date, ""),
                                Test_Score = SystemFunction.GetNumberNullToZero(ItemData.ObjTechnicalTest.technicaltest_score),
                            };

                            if (_TM_TechnicalTestTransactionService.CanSave(objSave))
                            {
                                var sComplect = _TM_TechnicalTestTransactionService.CreateNewAndEdit(objSave);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    var _getEditList = _TM_CandidatesService.Find(nId);
                                    if (_getEditList != null && _getEditList.TM_TechnicalTestTransaction != null && _getEditList.TM_TechnicalTestTransaction.Any())
                                    {

                                        result.lstTechnicalTestTransaction = (from lstAD in _getEditList.TM_TechnicalTestTransaction.Where(w => w.active_status == "Y")
                                                                              select new vcandidatesTechnicalTestTransaction_obj_Save
                                                                              {

                                                                                  technicaltest_date = lstAD.Test_Date.HasValue ? lstAD.Test_Date.Value.DateTimebyCulture() : "",
                                                                                  technicaltest_score = lstAD.Test_Score + "",
                                                                                  Test_name_en = lstAD.TM_TechnicalTest.Test_name_en,
                                                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddTechnicalTest('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                                  Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteTechnicalTest('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                                              }).ToList();
                                    }
                                    else
                                    {
                                        result.lstTechnicalTestTransaction = new List<vcandidatesTechnicalTestTransaction_obj_Save>();


                                    }

                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Duplicate;
                                result.Msg = "Duplicate technical test name.";
                            }
                            ////////////////////////

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, candidate not found.";
                        }

                    }

                }

            }
            return Json(new
            {
                result
            });

        }
        [HttpPost]
        public ActionResult EditCandidateTechnicalTest(vTechnicalTestTransaction_Save ItemData)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }


            vcandidatesTechnicalTestTransaction_obj_Save result = new vcandidatesTechnicalTestTransaction_obj_Save();
            //VCandidatesTechnicalTestTransaction_Return result = new VCandidatesTechnicalTestTransaction_Return();

            int Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));

            if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));

                if (nId != 0)
                {
                    var _getEditList = _TM_CandidatesService.Find(nId);
                    if (_getEditList != null && _getEditList.TM_TechnicalTestTransaction != null && _getEditList.TM_TechnicalTestTransaction.Any())
                    {

                        //_getEditList.update_user = CGlobal.UserInfo.UserId;
                        //_getEditList.update_date = dNow;
                        //_getEditList.active_status = "Y";
                        //_getEditList.technicaltest_date = SystemFunction.ConvertStringToDateTime(ItemData.ObjTechnicalTest.technicaltest_date , "");


                        //update_date = dNow,
                        //create_user = CGlobal.UserInfo.UserId,
                        //create_date = dNow,
                        //active_status = "Y",
                        //technicaltest_date = lstAD.Test_Date.HasValue ? lstAD.Test_Date.Value.DateTimebyCulture() : "",
                        //technicaltest_score = lstAD.Test_Score + "",
                        //Test_name_en = lstAD.TM_TechnicalTest.Test_name_en,

                    }

                }



                /*
                                if (nId != 0)
                {
                 
                    var _getEditList = _TM_CandidatesService.Find(nId);
                    if (_getEditList != null && _getEditList.TM_TechnicalTestTransaction != null && _getEditList.TM_TechnicalTestTransaction.Any())
                    {

                        result.lstTechnicalTestTransaction  = (from lstAD in _getEditList.TM_TechnicalTestTransaction.Where(w => w.active_status == "Y")
                                                              select new vcandidatesTechnicalTestTransaction_obj_Save
                                                              {
                                                                  update_user = CGlobal.UserInfo.UserId,
                                                                  update_date = dNow,
                                                                  create_user = CGlobal.UserInfo.UserId,
                                                                  create_date = dNow,
                                                                  active_status = "Y",
                                                                  technicaltest_date = lstAD.Test_Date.HasValue ? lstAD.Test_Date.Value.DateTimebyCulture() : "",
                                                                  technicaltest_score = lstAD.Test_Score + "",
                                                                  Test_name_en = lstAD.TM_TechnicalTest.Test_name_en,

                                                              }).ToList();
                    } 
             
                */


            }

            return View(result);



        }
        [HttpPost]
        public ActionResult DeleteCandidateTechnicalTest(vTechnicalTestTransaction_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            VCandidatesTechnicalTestTransaction_Return result = new VCandidatesTechnicalTestTransaction_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetCandidate = _TM_CandidatesService.Find(nId);
                    if (_GetCandidate != null)
                    {
                        TM_TechnicalTestTransaction objSave = new TM_TechnicalTestTransaction()
                        {
                            Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjTechnicalTest.IdEncrypt + "")),
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            active_status = "N",
                        };



                        var sComplect = _TM_TechnicalTestTransactionService.InActive(objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getEditList = _TM_CandidatesService.Find(nId);
                            if (_getEditList != null && _getEditList.TM_TechnicalTestTransaction != null && _getEditList.TM_TechnicalTestTransaction.Any())
                            {

                                result.lstTechnicalTestTransaction = (from lstTechTest in _getEditList.TM_TechnicalTestTransaction.Where(w => w.active_status == "Y")
                                                                      select new vcandidatesTechnicalTestTransaction_obj_Save
                                                                      {
                                                                          technicaltest_id = lstTechTest.TM_TechnicalTest.Id + "",
                                                                          Test_name_en = lstTechTest.TM_TechnicalTest.Test_name_en,
                                                                          technicaltest_score = lstTechTest.Test_Score.HasValue ? lstTechTest.Test_Score.Value.ToString() : null,
                                                                          technicaltest_date = lstTechTest.Test_Date.HasValue ? lstTechTest.Test_Date.Value.DateTimebyCulture() : "-- / -- / ----",
                                                                          Test_Status = lstTechTest.TM_TechnicalTest.Test_Status,
                                                                          active_status = lstTechTest.active_status,
                                                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddTechnicalTest('" + HCMFunc.Encrypt(lstTechTest.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                          Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteTechnicalTest('" + HCMFunc.Encrypt(lstTechTest.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",

                                                                      }).ToList();
                            }
                            else
                            {
                                result.lstTechnicalTestTransaction = new List<vcandidatesTechnicalTestTransaction_obj_Save>();
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, candidate not found.";
                    }


                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select candidate.";
                }
            }
            return Json(new
            {
                result
            });
        }
        #endregion TechnicalTest

        #region EducationHistory
        [HttpPost]
        public ActionResult SaveCandidateEducationHistory(vEducation_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            VCandidatesEduHistory_Return result = new VCandidatesEduHistory_Return();
            if (ItemData != null)
            {
                var dT_sT = string.IsNullOrEmpty(ItemData.ObjEdu.start_date) ? DateTime.Now : SystemFunction.ConvertStringToDateTime(ItemData.ObjEdu.start_date, "");
                var dT_eD = string.IsNullOrEmpty(ItemData.ObjEdu.end_date) ? DateTime.Now : SystemFunction.ConvertStringToDateTime(ItemData.ObjEdu.end_date, "");

                if (dT_eD > dT_sT)
                {
                    DateTime dNow = DateTime.Now;
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    if (nId != 0)
                    {
                        var _GetCandidate = _TM_CandidatesService.Find(nId);
                        if (_GetCandidate != null)
                        {
                            int nDegree = SystemFunction.GetIntNullToZero(ItemData.ObjEdu.degree + "");
                            var _getDegree = _TM_Education_DegreeServices.Find(nDegree);
                            int nMajor = SystemFunction.GetIntNullToZero(ItemData.ObjEdu.major_id + "");
                            var _getMajor = _TM_Universitys_MajorService.Find(nMajor);
                            int nFaculty = SystemFunction.GetIntNullToZero(ItemData.ObjEdu.faculty_id + "");
                            int EduHis = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjEdu.IdEncrypt + ""));


                            TM_Education_History objSave = new TM_Education_History()
                            {
                                Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjEdu.IdEncrypt + "")),
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                TM_Education_Degree = _getDegree != null ? _getDegree : null,
                                TM_Candidates = _GetCandidate,
                                TM_Universitys_Major = _getMajor,
                                education_history_description = ItemData.ObjEdu.education_history_description + "",
                                Degree = ItemData.ObjEdu.degree,
                                grade = SystemFunction.GetNumberNullToZero(ItemData.ObjEdu.grade),
                                start_date = SystemFunction.ConvertStringToDateTime(ItemData.ObjEdu.start_date, ""),
                                end_date = SystemFunction.ConvertStringToDateTime(ItemData.ObjEdu.end_date, ""),
                                Ref_Cert_ID = ItemData.ObjEdu.Ref_Cert_ID + "",

                            };


                            if (_TM_Education_HistoryServices.CanSave(objSave))
                            {
                                var sComplect = _TM_Education_HistoryServices.CreateNewAndEdit(objSave);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    var _getEditList = _TM_CandidatesService.Find(nId);
                                    if (_getEditList != null && _getEditList.TM_Education_History != null && _getEditList.TM_Education_History.Any())
                                    {

                                        result.lstEduHistory = (from lstAD in _getEditList.TM_Education_History.Where(w => w.active_status == "Y")
                                                                select new vEduHistory_obj
                                                                {
                                                                    major_name = lstAD.TM_Universitys_Major.universitys_major_name_en,
                                                                    degree = lstAD.TM_Education_Degree.degree_name_en,
                                                                    faculty_name = lstAD.TM_Universitys_Major.TM_Universitys_Faculty.universitys_faculty_name_en,
                                                                    education_history_description = lstAD.education_history_description,
                                                                    active_status = lstAD.active_status,
                                                                    grade = lstAD.grade,
                                                                    start_date = lstAD.start_date.HasValue ? lstAD.start_date.Value.DateTimebyCulture() : "",
                                                                    end_date = lstAD.end_date.HasValue ? lstAD.end_date.Value.DateTimebyCulture() : "",
                                                                    Ref_Cert_ID = lstAD.Ref_Cert_ID + "",
                                                                    Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddEducation('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                    //Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteEduHis('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                                    Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteEduHis('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                                }).ToList();
                                    }
                                    else
                                    {
                                        result.lstEduHistory = new List<vEduHistory_obj>();
                                    }

                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Duplicate;
                                result.Msg = "Duplicate Major name.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, candidate not found.";
                        }


                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, please select candidate.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please check date range. : " + dT_sT.ToString("dd-MMM-yyyy") + " to " + dT_eD.ToString("dd-MMM-yyyy");
                }

            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Empty Data.";
            }
            return Json(new
            {
                result
            });

        }
        [HttpPost]
        public ActionResult DeleteCandidateEducationHistory(vEducation_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            VCandidatesEduHistory_Return result = new VCandidatesEduHistory_Return();
            if (ItemData != null)
            {

                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetCandidate = _TM_CandidatesService.Find(nId);
                    var _getFindStatus = _TM_Education_HistoryServices.Find(SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjEdu.IdEncrypt + "")));
                    if (_GetCandidate != null)
                    {
                        TM_Education_History objSave = new TM_Education_History()
                        {
                            Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjEdu.IdEncrypt + "")),
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            active_status = _getFindStatus.active_status == "N" ? "Y" : "N"
                        };

                        var sComplect = _TM_Education_HistoryServices.InActive(objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getEditList = _TM_CandidatesService.Find(nId);
                            if (_getEditList != null && _getEditList.TM_Education_History != null && _getEditList.TM_Education_History.Any())
                            {

                                result.lstEduHistory = (from lstAD in _getEditList.TM_Education_History
                                                        select new vEduHistory_obj
                                                        {
                                                            major_name = lstAD.TM_Universitys_Major.universitys_major_name_en,
                                                            degree = lstAD.TM_Education_Degree.degree_name_en,
                                                            faculty_name = lstAD.TM_Universitys_Major.TM_Universitys_Faculty.universitys_faculty_name_en,
                                                            education_history_description = lstAD.education_history_description,
                                                            active_status = lstAD.active_status,
                                                            grade = lstAD.grade,
                                                            start_date = lstAD.start_date.HasValue ? lstAD.start_date.Value.DateTimebyCulture() : "",
                                                            end_date = lstAD.end_date.HasValue ? lstAD.end_date.Value.DateTimebyCulture() : "",
                                                            Ref_Cert_ID = lstAD.Ref_Cert_ID + "",
                                                            Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddEducation('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                            Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteEduHis('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                        }).ToList();
                            }
                            else
                            {
                                result.lstEduHistory = new List<vEduHistory_obj>();
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, candidate not found.";
                    }


                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select candidate.";
                }
            }


            return Json(new
            {
                result
            });
        }

        #endregion EducationHistory

        #region Work Experience
        [HttpPost]
        public ActionResult SaveCandidateWorkExp(vWorkExp_Save ItemData)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            VCandidatesWorkExpHistory_Return result = new VCandidatesWorkExpHistory_Return();
            if (ItemData != null)
            {
                var dT_sT = string.IsNullOrEmpty(ItemData.ObjWorkExp.StartDate) ? DateTime.Now : SystemFunction.ConvertStringToDateTime(ItemData.ObjWorkExp.StartDate, "");
                var dT_eD = string.IsNullOrEmpty(ItemData.ObjWorkExp.EndDate) ? DateTime.Now : SystemFunction.ConvertStringToDateTime(ItemData.ObjWorkExp.EndDate, "");


                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetCandidate = _TM_CandidatesService.Find(nId);
                    var _GetWorkExp = ItemData.ObjWorkExp;
                    List<TM_WorkExperience> get_check = _GetCandidate.TM_WorkExperience.Where(w => w.StartDate >= dT_sT && w.EndDate >= dT_eD).ToList();
                    if (dT_eD > dT_sT)
                    {
                        if (get_check.Count() == 0)
                        {
                            if (_GetCandidate != null)
                            {
                                var testrun = _TM_WorkExperienceService.GetDataForSelect_AllStatus().ToList();
                                var run_seq = testrun.Where(w => w.TM_Candidates.Id == nId).Count();

                                TM_WorkExperience objSave = new TM_WorkExperience()
                                {
                                    Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjWorkExp.IdEncrypt + "")),
                                    CompanyName = ItemData.ObjWorkExp.CompanyName,
                                    JobPosition = ItemData.ObjWorkExp.JobPosition,
                                    seq = SystemFunction.GetIntNullToZero(ItemData.ObjWorkExp.Id) == 0 ? run_seq + 1 : SystemFunction.GetIntNullToZero(ItemData.ObjWorkExp.Id),
                                    TM_Candidates = _GetCandidate,
                                    active_status = "Y",
                                    StartDate = SystemFunction.ConvertStringToDateTime(ItemData.ObjWorkExp.StartDate, ""),
                                    EndDate = SystemFunction.ConvertStringToDateTime(ItemData.ObjWorkExp.EndDate, ""),
                                    TypeOfRelatedToJob = ItemData.ObjWorkExp.TypeOfRelatedToJob,
                                    base_salary = SystemFunction.GetNumberNullToZero(ItemData.ObjWorkExp.base_salary),
                                    transportation = SystemFunction.GetNumberNullToZero(ItemData.ObjWorkExp.transportation),
                                    mobile_allowance = SystemFunction.GetNumberNullToZero(ItemData.ObjWorkExp.mobile_allowance),
                                    position_allowance = SystemFunction.GetNumberNullToZero(ItemData.ObjWorkExp.position_allowance),
                                    other_allowance = SystemFunction.GetNumberNullToZero(ItemData.ObjWorkExp.other_allowance),
                                    annual_leave = SystemFunction.GetNumberNullToZero(ItemData.ObjWorkExp.annual_leave),
                                    variable_bonus = SystemFunction.GetNumberNullToZero(ItemData.ObjWorkExp.variable_bonus),
                                    expected_salary = SystemFunction.GetNumberNullToZero(ItemData.ObjWorkExp.expected_salary),
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = DateTime.Now
                                };

                                var sComplect = _TM_WorkExperienceService.CreateNewAndEdit(objSave);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    var _getEditList = _TM_CandidatesService.Find(nId);
                                    if (_getEditList != null && _getEditList.TM_WorkExperience != null && _getEditList.TM_WorkExperience.Any())
                                    {

                                        result.lstWorkExpHistory = (from lstAD in _getEditList.TM_WorkExperience
                                                                    select new vWorkExpHistory_obj
                                                                    {
                                                                        CompanyName = lstAD.CompanyName,
                                                                        JobPosition = lstAD.JobPosition,
                                                                        StartDate = lstAD.StartDate.HasValue ? lstAD.StartDate.Value.DateTimebyCulture() : "",
                                                                        EndDate = lstAD.EndDate.HasValue ? lstAD.EndDate.Value.DateTimebyCulture() : "",
                                                                        TypeOfRelatedToJob = lstAD.TypeOfRelatedToJob,
                                                                        active_status = lstAD.active_status,
                                                                        base_salary = lstAD.base_salary.HasValue ? lstAD.base_salary.Value + "" : "",
                                                                        transportation = lstAD.transportation.HasValue ? lstAD.transportation.Value + "" : "",
                                                                        mobile_allowance = lstAD.mobile_allowance.HasValue ? lstAD.mobile_allowance.Value + "" : "",
                                                                        position_allowance = lstAD.position_allowance.HasValue ? lstAD.position_allowance.Value + "" : "",
                                                                        other_allowance = lstAD.other_allowance.HasValue ? lstAD.other_allowance.Value + "" : "",
                                                                        annual_leave = lstAD.annual_leave.HasValue ? lstAD.annual_leave.Value + "" : "",
                                                                        variable_bonus = lstAD.variable_bonus.HasValue ? lstAD.variable_bonus.Value + "" : "",
                                                                        expected_salary = lstAD.expected_salary.HasValue ? lstAD.expected_salary.Value + "" : "",
                                                                        Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddWorkExperience('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                        Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteWorkExperience('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                                    }).ToList();
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, can't call to CandidatesService.";
                                        result.lstWorkExpHistory = new List<vWorkExpHistory_obj>();
                                    }

                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Duplication of the date range.";
                            return Json(new
                            {
                                result
                            });
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, please check date range. : " + dT_sT.ToString("dd-MMM-yyyy") + " to " + dT_eD.ToString("dd-MMM-yyyy");
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select candidate.";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Empty Data.";
            }
            return Json(new
            {
                result
            });

        }

        [HttpPost]
        public ActionResult DeleteCandidateWorkExp(vWorkExp_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            VCandidatesWorkExpHistory_Return result = new VCandidatesWorkExpHistory_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    //var _GetCandidateAll = _TM_CandidatesService.GetDataForSelect().ToList();
                    var _GetCandidate = _TM_CandidatesService.Find(nId);
                    var _GetWorkExp = _TM_WorkExperienceService.Find(SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjWorkExp.IdEncrypt + "")));
                    List<TM_WorkExperience> get_check = _GetCandidate.TM_WorkExperience.Where(w => w.StartDate >= _GetWorkExp.StartDate && w.EndDate <= _GetWorkExp.EndDate && w.Id != _GetWorkExp.Id && w.active_status == "Y").ToList();

                    if (get_check.Count() == 0 || _GetWorkExp.active_status == "Y")
                    {


                        if (_GetCandidate != null)
                        {

                            TM_WorkExperience objSave = new TM_WorkExperience()
                            {
                                Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.ObjWorkExp.IdEncrypt + "")),
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                active_status = _GetWorkExp.active_status == "N" ? "Y" : "N"
                            };



                            var sComplect = _TM_WorkExperienceService.InActive(objSave);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _TM_CandidatesService.Find(nId);
                                if (_getEditList != null && _getEditList.TM_WorkExperience != null && _getEditList.TM_WorkExperience.Any())
                                {

                                    result.lstWorkExpHistory = (from lstWorkExp in _getEditList.TM_WorkExperience
                                                                select new vWorkExpHistory_obj
                                                                {
                                                                    Id = lstWorkExp.Id.ToString(),
                                                                    CompanyName = lstWorkExp.CompanyName,
                                                                    JobPosition = lstWorkExp.JobPosition,
                                                                    StartDate = lstWorkExp.StartDate.HasValue ? lstWorkExp.StartDate.Value.DateTimebyCulture() : "",
                                                                    EndDate = lstWorkExp.EndDate.HasValue ? lstWorkExp.EndDate.Value.DateTimebyCulture() : "",
                                                                    TypeOfRelatedToJob = lstWorkExp.TypeOfRelatedToJob,
                                                                    active_status = lstWorkExp.active_status,
                                                                    base_salary = lstWorkExp.base_salary.HasValue ? lstWorkExp.base_salary.Value + "" : "",
                                                                    transportation = lstWorkExp.transportation.HasValue ? lstWorkExp.transportation.Value + "" : "",
                                                                    mobile_allowance = lstWorkExp.mobile_allowance.HasValue ? lstWorkExp.mobile_allowance.Value + "" : "",
                                                                    position_allowance = lstWorkExp.position_allowance.HasValue ? lstWorkExp.position_allowance.Value + "" : "",
                                                                    other_allowance = lstWorkExp.other_allowance.HasValue ? lstWorkExp.other_allowance.Value + "" : "",
                                                                    annual_leave = lstWorkExp.annual_leave.HasValue ? lstWorkExp.annual_leave.Value + "" : "",
                                                                    variable_bonus = lstWorkExp.variable_bonus.HasValue ? lstWorkExp.variable_bonus.Value + "" : "",
                                                                    expected_salary = lstWorkExp.expected_salary.HasValue ? lstWorkExp.expected_salary.Value + "" : "",
                                                                    Edit = @"<button id=""btnEdit""  type=""button"" onclick=""AddWorkExperience('" + HCMFunc.Encrypt(lstWorkExp.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                    Delete = @"<button id=""btnDelete""  type=""button""  onclick=""DeleteWorkExperience('" + HCMFunc.Encrypt(lstWorkExp.Id + "") + @"')""    class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>",
                                                                }).ToList();
                                }
                                else
                                {
                                    result.lstWorkExpHistory = new List<vWorkExpHistory_obj>();
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, candidate not found.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Duplication of the date range.";
                        return Json(new
                        {
                            result
                        });
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select candidate.";
                }
            }
            return Json(new
            {
                result
            });
        }

        #endregion Work Experience

        #endregion
    }
}