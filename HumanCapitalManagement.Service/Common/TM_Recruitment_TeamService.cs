using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_Recruitment_TeamService : ServiceBase<TM_Recruitment_Team>
    {
        public TM_Recruitment_TeamService(IRepository<TM_Recruitment_Team> repo) : base(repo)
        {
            //
        }
        public TM_Recruitment_Team Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Recruitment_Team FindByEmpID(string id)
        {
            return Query().SingleOrDefault(s => s.user_no == id);
        }
        public IEnumerable<TM_Recruitment_Team> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Recruitment_Team> GetAllRecruitments()
        {
            return Query().ToList();
        }
        public IEnumerable<TM_Recruitment_Team> GetRecruitmentList(string[] code, string status)//,bool isAdmin
        {
            var sQuery = Query();

            if (code != null)
            {
                sQuery = sQuery.Where(w => code.Contains(w.user_id));
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

        public bool CanSave(TM_Recruitment_Team TM_Recruitment_Team)
        {
            bool sCan = false;
            if (!string.IsNullOrEmpty(TM_Recruitment_Team.user_id))
            {
                if (TM_Recruitment_Team.Id == 0)
                {
                    var sCheck = Query().FirstOrDefault(w => w.user_id == TM_Recruitment_Team.user_id);
                    if (sCheck == null)
                    {
                        sCan = true;
                    }
                }
                else
                {
                    var sCheck = Query().FirstOrDefault(w => w.user_id == TM_Recruitment_Team.user_id && w.Id != TM_Recruitment_Team.Id);
                    if (sCheck == null)
                    {
                        sCan = true;
                    }
                }
            }

            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Recruitment_Team s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Recruitment_Team s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
