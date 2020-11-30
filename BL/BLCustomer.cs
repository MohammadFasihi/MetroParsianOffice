using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA;
using System.Windows.Forms;

namespace BL
{
    public class BLCustomer : DACustomer
    {
        public static tbl_Customer customer { get; set; }

        public BLCustomer()
        {

        }

        public BLCustomer(int ID)
        {
            customer = base.getCustomer(ID);
        }

        public void Insert(string Name, string Email, string NationalCode, string Address, int? visitorID, string customName)
        {
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrWhiteSpace(Name))
            {


                if (base.IsExist(Name))
                {
                    DialogResult r = MessageBox.Show("مشتری وارد شده موجود می باشد" + "\n" + "آیا مایل به افزودن مشتری با اسم مشابه هستید؟", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (r == DialogResult.Yes)
                    {
                        base.Insert(Name, NationalCode, Email, Address, visitorID,customName);
                    }
                }
                else
                    base.Insert(Name, NationalCode, Email, Address, visitorID,customName);
            }
            else
            {
                throw new Exception("نام را وارد کنید");
            }
        }

        public void Update(int ID, string Name, string Email, string NationalCode, string Address, int? visitorID,string customName)
        {
            if (base.IsExist(ID))
            {
                base.Update(ID, Name, NationalCode, Email, Address, visitorID,customName);
            }
            else
            {
                throw new Exception("مشتری مورد نظر یافت نشد");
            }
        }

        public void Delete(int ID)
        {
            try
            {
                DialogResult r = MessageBox.Show("آیا مایل به حذف این مشتری می باشید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (base.IsExist(ID))
                    {
                        base.Delete(ID);
                    }
                    else
                    {
                        throw new Exception("مشتری مورد نظر یافت نشد");
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
