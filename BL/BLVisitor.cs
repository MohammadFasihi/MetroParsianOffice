using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DA;

namespace BL
{
    public class BLVisitor : DAVisitor
    {
        public static tbl_Experts visitor { get; set; }

        public BLVisitor()
        {

        }

        public BLVisitor(int ID)
        {
            visitor = base.getVisitor(ID);
        }
        public void Insert(string Name, string NationalCode, string Address, bool IsActive,byte time)
        {
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrWhiteSpace(Name))
            {
                if (base.IsExist(Name))
                {
                    DialogResult r = MessageBox.Show("بازاریاب وارد شده موجود می باشد" + "\n" + "آیا مایل به افزودن بازاریاب با اسم مشابه هستید؟", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (r == DialogResult.Yes)
                    {
                        base.Insert(Name, NationalCode, Address,IsActive,time);
                    }
                }
                else
                    base.Insert(Name, NationalCode, Address,IsActive,time);
            }
            else
            {
                throw new Exception("نام را وارد کنید");
            }
        }

        public void Update(int ID,string Name,string NationalCode,string Address,bool IsActive,byte time)
        {
            if(base.IsExist(ID))
            {
                base.Update(ID, Name, NationalCode, Address, IsActive,time);
            }
            else
            {
                throw new Exception("بازاریاب مورد نظر یافت نشد");
            }
        }

        public void Delete(int ID)
        {
            try
            {
                DialogResult r = MessageBox.Show("آیا مایل به حذف این بازاریاب می باشید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (base.IsExist(ID))
                    {
                        base.Delete(ID);
                    }
                    else
                    {
                        throw new Exception("بازاریاب مورد نظر یافت نشد");
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
