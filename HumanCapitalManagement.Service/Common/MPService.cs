using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class MPService : MPBase<Menu>
    {

        public MPService(IRepository<Menu> repo) : base(repo)
        {
            //
        }

        public Menu Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<Menu> GetlstMenu()
        {
            return Query().Where(s => s.ACTIVE_FLAG == "Y").ToList();
        }
        public IEnumerable<Menu> GetPESMenu()
        {
            return Query().Where(s => s.ACTIVE_FLAG == "Y" && s.menu_type == "PES").ToList();
        }

        //public Ticket CreateTicket(Service s)
        //{
        //    var t = s.CreateAndEnqueueTicket();
        //    SaveChanges();

        //    return t;
        //}

    }
}
