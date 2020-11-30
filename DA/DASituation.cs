using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DASituation
    {
        public List<tbl_Situation> GetAllSituation()
        {
            Entities db = new Entities();
            return (from q in db.tbl_Situation select q).ToList();
        }
    }
}
