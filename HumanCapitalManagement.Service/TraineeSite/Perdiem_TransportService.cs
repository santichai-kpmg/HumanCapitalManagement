using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TraineeSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TraineeSite
{
    public class Perdiem_TransportService : ServiceBase<Perdiem_Transport>
    {
        public Perdiem_TransportService(IRepository<Perdiem_Transport> repo) : base(repo)
        {
            //
        }
        public Perdiem_Transport Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<Perdiem_Transport> GetDataForSelect()
        {
            //var sQuery = Query();
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        #region Save Edit Delect 
        public int CreateNew(ref Perdiem_Transport s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(Perdiem_Transport s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(Perdiem_Transport s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).ToList();
            if (_getData.Any())
            {
                _getData.ForEach(ed =>
                {
                    ed.active_status = "N";
                    ed.trainee_update_date = DateTime.Now;
                    ed.trainee_update_user = s.trainee_update_user;
                });
                sResult = SaveChanges();
            }
            return sResult;
        }

        public int UpdateApprove_Status(Perdiem_Transport s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).ToList();
            if (_getData.Any())
            {

                _getData.ForEach(ed =>
                {
                    ed.active_status = "Y";
                    ed.approve_status = s.approve_status;
                    ed.remark = s.remark;
                    if (s.trainee_update_date != null)
                        ed.trainee_update_date = DateTime.Now;
                    ed.trainee_update_user = s.trainee_update_user;

                    if (s.approve_status == "A")
                    {
                        ed.review_date = DateTime.Now;
                        ed.review_date = s.review_date;
                    }
                    else if (s.approve_status == "P")
                    {
                        ed.paid_date = DateTime.Now;
                        ed.paid_user = s.paid_user;
                    }
                    else
                    {
                        if (s.approve_date != null)
                            ed.approve_date = DateTime.Now;
                    }
                });
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int CreateNewOrUpdate(Perdiem_Transport s)
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

                //if (_getData.approve_status != "Y")
                //{
                _getData.Type_of_withdrawal = s.Type_of_withdrawal;
                _getData.Company = s.Company;
                _getData.Reimbursable = s.Reimbursable;
                _getData.Business_Purpose = s.Business_Purpose;
                _getData.date_start = s.date_start;
                _getData.date_end = s.date_end;
                _getData.Description = s.Description;
                _getData.Amount = s.Amount;
                _getData.Engagement_Code = s.Engagement_Code;
                _getData.Approve_user = s.Approve_user;
                _getData.approve_status = s.approve_status;
                _getData.submit_status = s.submit_status;
                _getData.approve_status = s.approve_status;
                _getData.trainee_update_date = s.trainee_update_date;
                _getData.trainee_update_user = s.trainee_update_user;
                _getData.submit_date = s.submit_date;


                sResult = SaveChanges();
                //}
                //else
                //{
                //    sResult = 1;
                //}

            }
            //sResult = SaveChanges();
            return sResult;
        }


        #endregion
    }
}
