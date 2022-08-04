using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System.Collections.Generic;
using System.Linq;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_CandidatesService : ServiceBase<TM_Candidates>
    {
        public TM_CandidatesService(IRepository<TM_Candidates> repo) : base(repo)
        {
            //
        }
        public TM_Candidates Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Candidates FindAddUploadFile(int id)
        {
            if (id != 0)
            {
                return Query().FirstOrDefault(s => s.Id == id);
            }
            else
            {
                return null;
            }

        }
        public TM_Candidates FindIDCard(string id, int card_type)
        {
            return Query().SingleOrDefault(s => ((s.id_card + "").Trim().ToLower()) == ((id + "").Trim().ToLower()) && s.TM_Candidate_Type.Id == card_type);
        }
        public TM_Candidates FindName(string name, string lname)
        {
            return Query().FirstOrDefault(s => ((s.first_name_en + "").Trim().ToLower()) == ((name + "").Trim().ToLower())
            && ((s.last_name_en + "").Trim().ToLower()) == ((lname + "").Trim().ToLower()));
        }
        public TM_Candidates LoginUser(string username)
        {

            return Query().FirstOrDefault(s => s.candidate_user_id == username);

        }
        public TM_Candidates Login(string username, string password)
        {

            return Query().FirstOrDefault(s => s.candidate_user_id == username && s.candidate_password == password);

        }
        public int FindForUploadFile(string name, string lname)
        {
            int nReturn = 0;
            var GetCan = Query().FirstOrDefault(s => ((s.first_name_en + "").Trim().ToLower()) == ((name + "").Trim().ToLower())
            && ((s.last_name_en + "").Trim().ToLower()) == ((lname + "").Trim().ToLower()));
            if (GetCan != null)
            {
                nReturn = GetCan.Id;
            }
            return nReturn;
        }

        //public string  GetCandidateDivision(int CandidateId)
        //{
        //    //var sQuery 
        //    // var sCheck = Query().FirstOrDefault(w => ((w.first_name_en + "").Trim().ToLower()) == 
        //    //((TM_Candidates.first_name_en + "").Trim().ToLower())
        //    /*
        //    lstData = (from lst in _TM_CandidatesService.GetDataForSelect()
        //    join lst2 in _TM_PR_Candidate_MappingService.GetDataForSelect() on lst.Id equals lst2.TM_Candidates.Id
        //    join lst3 in _PersonnelRequestService.GetDataForSelect() on lst2.PersonnelRequest.Id equals lst3.Id
        //    join lst4 in _DivisionService.GetAll() on lst3.TM_Divisions.Id equals lst4.Id
        //    join lst5 in _PoolService.GetDataForSelect() on lst4.TM_Pool.Id equals lst5.Id
        //    join lst6 in _CompanyService.GetDataForSelect() on lst5.TM_Company.Id equals lst6.Id
        //    */
        //    var sQuery = Query().Where(w => w.active_status == "Y" && w.Id == CandidateId);
        //    List<TM_Candidates> lstCan = sQuery.ToList();

        //    var result = from lst in lstCan
        //                 select new  
        //}

        public IEnumerable<TM_Candidates> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidates> GetMajorForSave(int[] aID)//,bool isAdmin
        {
            var sQuery = Query();
            if (aID != null)
            {
                sQuery = sQuery.Where(w => aID.Contains(w.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidates> GetMajor(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.first_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.first_name_en + "").Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public bool CanSave(TM_Candidates TM_Candidates)
        {
            bool sCan = false;

            if (TM_Candidates.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => ((w.first_name_en + "").Trim().ToLower()) == ((TM_Candidates.first_name_en + "").Trim().ToLower())
            && ((w.last_name_en + "").Trim().ToLower()) == ((TM_Candidates.last_name_en + "").Trim().ToLower())
                );
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => ((w.first_name_en + "").Trim().ToLower()) == ((TM_Candidates.first_name_en + "").Trim().ToLower())
            && ((w.last_name_en + "").Trim().ToLower()) == ((TM_Candidates.last_name_en + "").Trim().ToLower())

                             && w.Id != TM_Candidates.Id
                            );
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }

        public IEnumerable<TM_Candidates> GetCandidate(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.first_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.first_name_en + "").Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }

        public IEnumerable<TM_Candidates> GetCandidate_Search(List<string> data)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);
            string valforwere = "";
            if (!string.IsNullOrEmpty(data[0]))
            {
                valforwere = data[0].ToString();
                sQuery = sQuery.Where(w => ((w.first_name_en + "").Trim().ToLower()).Contains((valforwere + "").Trim().ToLower() ?? ((w.first_name_en + "").Trim().ToLower())));
                //sQuery = sQuery.Where(w => ((w.first_name_en + "").Trim().ToLower()).Contains((data[0] + "").Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(data[1]))
            {
                valforwere = data[1].ToString();
                sQuery = sQuery.Where(w => ((w.last_name_en + "").Trim().ToLower()).Contains((valforwere + "").Trim().ToLower() ?? ((w.last_name_en + "").Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(data[2]))
            {
                valforwere = data[2].ToString();
                sQuery = sQuery.Where(w => w.id_card == valforwere);
            }
            if (!string.IsNullOrEmpty(data[3]))
            {
                valforwere = data[3].ToString();
                sQuery = sQuery.Where(w => w.Gender_Id.ToString() == valforwere);
            }
            if (!string.IsNullOrEmpty(data[4]))
            {
                valforwere = data[4].ToString();
                sQuery = sQuery.Where(w => w.active_status == valforwere);
            }
            return sQuery.ToList();
        }

        public IEnumerable<TM_Candidates> GetTrainee(string name, string status, string group_code, string ref_no)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1 && w.TM_PR_Candidate_Mapping.Any(a => a.active_status == "Y"
            && a.PersonnelRequest.TM_Employment_Request.TM_Employment_Type.personnel_type == "T"
            ));
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.Any(a3 => a3.PersonnelRequest.TM_Divisions.division_code == group_code));
            }
            if (!string.IsNullOrEmpty(ref_no))
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.Any(a2 => ((a2.PersonnelRequest.RefNo + "").Trim()).ToLower() == ((ref_no + "").Trim()).ToLower()));
            }
            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.first_name_en + "").Trim().ToLower()
                + (w.last_name_en + "").Trim().ToLower()
                + ((w.first_name_en + "").Trim().ToLower() + " " + (w.last_name_en + "").Trim().ToLower())).Contains((name + "").Trim().ToLower() ?? (
                (w.first_name_en + "").Trim().ToLower()
                + (w.last_name_en + "").Trim().ToLower()
                + ((w.first_name_en + "").Trim().ToLower() + " " + (w.last_name_en + "").Trim().ToLower()))));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Candidates s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Candidates s)
        {
            var sResult = 0;

            if (s != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
