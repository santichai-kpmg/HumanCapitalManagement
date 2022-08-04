using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.eGreetings;

using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.HCMServiceClass;

namespace HumanCapitalManagement.Service.eGreetingsService
{
    public class TM_eGreetings_PeroidService : ServiceBase<TM_eGreetings_Peroid>
    {
        public TM_eGreetings_PeroidService(IRepository<TM_eGreetings_Peroid> repo) : base(repo)
        {
            //
        }
        public TM_eGreetings_Peroid Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.Active_Status == "Y");
        }
     
        public IEnumerable<TM_eGreetings_Peroid> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y");
            return sQuery.ToList();
        }
        

        public IEnumerable<TM_eGreetings_Peroid> GetDataForSelectAll()
        {
            var sQuery = Query().Where(w => w.Active_Status != null);
            return sQuery.ToList();
        }

        #region Save Edit Delect 
        public int CreateNew(ref TM_eGreetings_Peroid s)
        {
            //s.Id = SelectMax();


            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }

        public int CreateNewOrUpdate(TM_eGreetings_Peroid s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData == null)
            {
                Add(s);

                sResult = SaveChanges();
            }
            else
            {
                    //_getData.Positive = s.Positive;
                    //_getData.Strength = s.Strength;
                    //_getData.Need_Improvement = s.Need_Improvement;
                    //_getData.Recommendations = s.Recommendations;
                    //_getData.Rate = s.Rate;
                    //_getData.Type = s.Type;
                    //_getData.Update_Date = DateTime.Now;
                    //_getData.Update_User = s.Update_User;
                    //_getData.Given_User = s.Given_User;
                    //_getData.Given_Date = s.Given_Date;
                    //_getData.Status = s.Status;

                    sResult = SaveChanges();
               

            }
            //sResult = SaveChanges();
            return sResult;
        }

        public int UpdateStatus(int id,string approve_user,string status)
        {
            var sResult = 0;

            sResult = SaveChanges();
            var _getData = Query().Where(w => w.Id == id).FirstOrDefault();
            if (_getData != null)
            {
                //_getData.Status = status;
                //_getData.Update_Date = DateTime.Now;
                //_getData.Approve_Date = DateTime.Now;
                //_getData.Approve_User = approve_user;

                sResult = SaveChanges();
            }

            return sResult;
        }

        public int AdminUpdate(TM_eGreetings_Peroid s)
        {
            var sResult = 0;

            var _getDataAD = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataAD != null)
            {
                //_getDataAD.update_user = s.update_user;
                //_getDataAD.update_date = s.update_date;
                //_getDataAD.job_descriptions = s.job_descriptions;
                //_getDataAD.qualification_experience = s.qualification_experience;
                //_getDataAD.remark = s.remark;
                //_getDataAD.active_status = s.active_status;
                ////    _getDataAD.TM_SubGroup_Id = s.TM_SubGroup_Id;
                //_getDataAD.TM_Pool_Rank = s.TM_Pool_Rank;
                //_getDataAD.TM_Position = s.TM_Position;
                //_getDataAD.TM_PR_Status = s.TM_PR_Status;
                //_getDataAD.no_of_headcount = s.no_of_headcount;

                sResult = SaveChanges();
            }
            return sResult;

        }
        #endregion
    }
}
