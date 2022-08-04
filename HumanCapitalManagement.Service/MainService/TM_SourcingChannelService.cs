using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public  class TM_SourcingChannelService : ServiceBase<TM_SourcingChannel>
    {
        public TM_SourcingChannelService(IRepository<TM_SourcingChannel> repo) : base(repo)
        {
            //
        }
        public TM_SourcingChannel Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_SourcingChannel> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_SourcingChannel> GetSourcingChannelForSave(int[] aID)//,bool isAdmin
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

    
        #region Save Edit Delect 
        public int CreateNew(TM_SourcingChannel s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_SourcingChannel s)
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
