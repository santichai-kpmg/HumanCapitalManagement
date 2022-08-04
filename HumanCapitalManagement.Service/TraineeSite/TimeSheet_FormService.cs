using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TraineeSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TraineeSite
{
   public class TimeSheet_FormService : ServiceBase<TimeSheet_Form>
    {
        public TimeSheet_FormService(IRepository<TimeSheet_Form> repo) : base(repo)
        {
            //
        }
        public TimeSheet_Form Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TimeSheet_Form GetActiveTimeSheetForm()
        {
            return Query().Where(s => s.active_status == "Y").OrderByDescending(o => o.update_date).FirstOrDefault();
        }
        public IEnumerable<TimeSheet_Form> GetDataForSelect()
        {
            //var sQuery = Query();
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TimeSheet_Form> FindByCandidateID(int id)
        {
            return Query().Where(s => s.trainee_create_user == id).ToList();
        }

      

        public IEnumerable<TimeSheet_Form> FindByApproveUser(string Approve_user)
        {
            return Query().Where(s => s.Approve_user == Approve_user).ToList();
        }
        #region Save Edit Delect 
        public int CreateNew(ref TimeSheet_Form s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TimeSheet_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(TimeSheet_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id != s.Id && w.active_status == "Y" && w.trainee_create_user == s.trainee_create_user).ToList();
            if (_getData.Any())
            {
                _getData.ForEach(ed =>
                {
                    ed.active_status = "N";
                    ed.update_date = DateTime.Now;
                    ed.update_user = s.update_user;
                });
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateApproveFrom(TimeSheet_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).ToList();
            if (_getData.Any())
            {
                _getData.ForEach(ed =>
                {
                    ed.active_status = "Y";
                    ed.Approve_status = s.Approve_status;
                    ed.Approve_remark = s.Approve_remark;
                    ed.Approve_date =  DateTime.Now;
                    ed.trainee_update_date = DateTime.Now;
                    ed.trainee_update_user = s.trainee_update_user;
                });
                sResult = SaveChanges();
            }
            return sResult;
        }

        public int CreateNewOrUpdate(TimeSheet_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData == null)
            {
                Add(s);
            }
            else
            {
                _getData.trainee_update_date = DateTime.Now;
                _getData.trainee_update_user = s.trainee_update_user;
                if(!string.IsNullOrEmpty(s.Approve_user))
                _getData.Approve_user = s.Approve_user;

            }
            sResult = SaveChanges();
            return sResult;
        }

        #endregion
    }
}
