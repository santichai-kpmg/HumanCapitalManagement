using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.HCMServiceClass;

namespace HumanCapitalManagement.Service.MainService
{
    public class Probation_With_OutService : ServiceBase<Probation_With_Out>
    {
        public Probation_With_OutService(IRepository<Probation_With_Out> repo) : base(repo)
        {
            //
        }
        public Probation_With_Out Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.Active_Status == "Y");
        }

  
        public IEnumerable<Probation_With_Out> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y");
            return sQuery.ToList();
        }
        public Probation_With_Out FindByEmpno(string empno)
        {
            return Query().SingleOrDefault(s => s.Staff_No == empno && s.Active_Status == "Y");
        }

        #region Save Edit Delect 
        public int CreateNew(ref Probation_With_Out s)
        {
            //s.Id = SelectMax();


            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }

        public int CreateNewOrUpdate(Probation_With_Out s)
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
                

                _getData.Update_Date = DateTime.Now;
                _getData.Update_User = s.Update_User;
              

                sResult = SaveChanges();


            }
            //sResult = SaveChanges();
            return sResult;
        }

        public int UpdateStatus(int id, string update_user, string status)
        {
            var sResult = 0;
            
            var _getData = Query().Where(w => w.Id == id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.Active_Status = status;
                _getData.Update_Date = DateTime.Now;
                _getData.Update_User = update_user;

                sResult = SaveChanges();
            }

            return sResult;
        }

        public int SetInActive(int id, string update_user, string status)
        {
            var sResult = 0;

            var _getData = Query().Where(w => w.Id == id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.Active_Status =status;
                _getData.Update_Date = DateTime.Now;
                _getData.Update_User = update_user;

                sResult = SaveChanges();
            }

            return sResult;
        }

   
        #endregion
    }
}
