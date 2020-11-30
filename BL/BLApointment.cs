using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA;
using System.Windows.Forms;
using Arash;

namespace BL
{
    public class BLApointment : DAApointment
    {
        public static tbl_Apointments apointment { get; set; }

        public BLApointment()
        {

        }

        public BLApointment(int ID)
        {
            apointment = base.getApointment(ID);
        }
        public void Insert(object cu_ID, string Name, string Phone, object ep_ID, PersianDate Date, string Time, int st_ID, string area, int ap_Kind, string Address, int UserID)
        {
            int cust = -1;
            int? ID;
            int expert = -1;

            if (cu_ID == null)
                ID = null;
            else
                ID = int.Parse(cu_ID.ToString());

            int.TryParse(ep_ID.ToString(), out expert);

            if (expert == -1)
                throw new Exception("کارشناس انتخاب نشده");

            base.Insert(ID, Name, Phone, expert, Date, Time, st_ID, area, ap_Kind, Address, UserID);
        }

        public void Update(int ID, string Name, string Phone, object cu_ID, object ep_ID, PersianDate Date, string Time, int st_ID, string area, int ap_Kind, string Address, int UserID)
        {
            int cust = -1;
            int? id;
            int expert = -1;

            if (cu_ID == null)
                id = null;
            else
                id = int.Parse(cu_ID.ToString());

            int.TryParse(ep_ID.ToString(), out expert);

            if (expert == -1)
                throw new Exception("کارشناس انتخاب نشده");

            base.Update(ID, id, Name, Phone, expert, Date, Time, st_ID, area, ap_Kind, Address, UserID);
        }

        public void Delete(int ID)
        {
            try
            {
                DialogResult r = MessageBox.Show("آیا مایل به حذف قرار ملاقات می باشید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (base.IsExist(ID))
                    {
                        base.Delete(ID);
                    }
                    else
                    {
                        throw new Exception("قرار ملاقات وجود ندارد");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
