using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DAApointmentKind
    {
        public List<tbl_ApKinds> GetAllKind()
        {
            Entities db = new Entities();
            return (from q in db.tbl_ApKinds select q).ToList();
        }
    }
}
