using Arash;
using DA;
using DevExpress.Xpf.Grid.LookUp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BL
{
    public class BLOrder : DAOrder
    {
        public static tbl_OrderHeader order { get; set; }

        public BLOrder()
        {

        }

        public BLOrder(int ID)
        {
            order = base.getOrder(ID);
        }
        public void Insert(object ID, LookUpEdit customer, PersianDate Date, object Status, int BasePrice, int Discount, int OrderPrice, int UserID, string Comment, DataTable Detail, int kind)
        {
            if (customer.SelectedItem == null)
                throw new Exception("مشتری انتخاب نشده");

            int id = int.Parse(ID.ToString());
            int cu_ID = int.Parse(customer.EditValue.ToString());
            int sts = int.Parse(Status.ToString());

            if (!base.IsExist(id))
            {
                base.Insert(id, cu_ID, Date, sts, BasePrice, Discount, OrderPrice, UserID, Comment, kind);
                base.Insert(id, Detail);
            }
            else
                throw new Exception("کد سفارش تکراری می باشد");
        }

        public void Update(object ID, LookUpEdit customer, PersianDate Date, object Status, int BasePrice, int Discount, int OrderPrice, int UserID, string Comment, DataTable Detail, int kind)
        {
            if (customer.SelectedItem == null)
                throw new Exception("مشتری انتخاب نشده");

            int id = int.Parse(ID.ToString());
            int cu_ID = int.Parse(customer.EditValue.ToString());
            int sts = int.Parse(Status.ToString());

            if (base.IsExist(id))
            {
                base.Update(id, cu_ID, Date, sts, BasePrice, Discount, OrderPrice, UserID, Comment, kind);
                base.DeleteDetail(id);
                base.Insert(id, Detail);
            }
            else
                throw new Exception("سفارش به کد : " + id.ToString() + " وجود ندارد");
        }

        public void Delete(int ID)
        {
            try
            {
                DialogResult r = MessageBox.Show("آیا مایل به حذف سفارش می باشید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (base.IsExist(ID))
                    {
                        base.Delete(ID);
                    }
                    else
                    {
                        throw new Exception("سفارش مورد نظر وجود ندارد");
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
