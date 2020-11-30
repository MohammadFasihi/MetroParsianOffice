using Arash;
using DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BL
{
    public class BLContract : DAContract
    {
        public static tbl_Contract contract { get; set; }

        public BLContract()
        {

        }

        public BLContract(int ID)
        {
            contract = base.getContract(ID);
        }
        public void Insert(object conID, object OrderID, string PaymentText, int UserID, PersianDate date, int? reportID)
        {
            int id = -1;
            int orderID = -1;

            int.TryParse(conID.ToString(), out id);
            int.TryParse(OrderID.ToString(), out orderID);

            if (id == -1)
                throw new Exception("شماره قرارداد معتبر نمی باشد");
            if (orderID == -1)
                throw new Exception("شماره فاکتور معتبر نمی باشد");


            if (!base.IsContractOrderExist(orderID))
                base.Insert(id, orderID, PaymentText, UserID, date, reportID);
            else
                throw new Exception("برای فاکتور ثبت شده ثبت قرارداد صورت گرفته است. امکان ثبت مجدد قرارداد وجود ندارد");
        }

        public void Update(object conID, object OrderID, string PaymentText, int UserID, PersianDate date, int? reportID)
        {
            int id = -1;
            int orderID = -1;

            int.TryParse(conID.ToString(), out id);
            int.TryParse(OrderID.ToString(), out orderID);

            if (id == -1)
                throw new Exception("شماره قرارداد معتبر نمی باشد");
            if (orderID == -1)
                throw new Exception("شماره فاکتور معتبر نمی باشد");

            base.Update(id, orderID, PaymentText, UserID, date, reportID);
        }

        public void Delete(int ID)
        {
            try
            {
                DialogResult r = MessageBox.Show("آیا مایل به حذف قرارداد می باشید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    if (base.IsExist(ID))
                    {
                        base.Delete(ID);
                    }
                    else
                    {
                        throw new Exception("قرارداد وجود ندارد");
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
