using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_Nomination_FilesService : ServiceBase<PES_Nomination_Files>
    {
        public PES_Nomination_FilesService(IRepository<PES_Nomination_Files> repo) : base(repo)
        {
            //
        }
        public PES_Nomination_Files Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public PES_Nomination_Files FindForDelete(int id, int nFileid)
        {
            return Query().SingleOrDefault(s => s.Id == nFileid && s.PES_Nomination.Id == id);
        }
        public int CreateNew(PES_Nomination_Files s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(PES_Nomination_Files s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int DeleteFile(PES_Nomination_Files s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
    }
}
