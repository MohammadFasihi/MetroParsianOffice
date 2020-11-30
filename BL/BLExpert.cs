using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA;
using System.Windows.Forms;

namespace BL
{
    public class BLExpert : DAExpert
    {
        public static tbl_Experts expert { get; set; }

        public BLExpert()
        {

        }

        public BLExpert(int ID)
        {
            expert = base.getExpert(ID);
        }
        public void Insert(string Name, string NationalCode, string Address,bool IsActive,byte DailyTimeWork)
        {
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrWhiteSpace(Name))
            {
                if (base.IsExist(Name))
                {
                    DialogResult r = MessageBox.Show("کارشناس با نام وارد شده موجود می باشد" + "\n" + "آیا مایل به افزودن کارشناس با اسم مشابه هستید؟", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (r == DialogResult.Yes)
                    {
                        base.Insert(Name, NationalCode, Address,IsActive,DailyTimeWork);
                    }
                }
                else
                    base.Insert(Name, NationalCode, Address,IsActive,DailyTimeWork);
            }
            else
            {
                throw new Exception("نام را وارد کنید");
            }
        }

        public void Update(int ID, string Name, string NationalCode, string Address,bool IsActive, byte DailyTimeWork)
        {
            if (base.IsExist(ID))
            {
                base.Update(ID, Name, NationalCode, Address,IsActive, DailyTimeWork);
            }
            else
            {
                throw new Exception("کارشناس مورد نظر یافت نشد");
            }
        }

        public void Delete(int ID)
        {
            try
            {
                DialogResult r = MessageBox.Show("آیا مایل به حذف این کارشناس می باشید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (base.IsExist(ID))
                    {
                        base.Delete(ID);
                    }
                    else
                    {
                        throw new Exception("کارشناس مورد نظر یافت نشد");
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
