using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class AppSettingClass
    {
        public static AppSettings setting = new AppSettings();

        public AppSettingClass()
        {

        }

        public AppSettingClass(int UseID)
        {

        }

        public AppSettings appSetting = new AppSettings();

        public static void GetAppSetting(int UserID)
        {
            Entities db = new Entities();
            setting = (from q in db.AppSettings where q.userID == UserID select q).Single();
        }

        public void UpdateAppSetting(int UserID)
        {
            Entities db = new Entities();

            var query = (from q in db.AppSettings where q.userID == UserID select q).Single();

            query.PagingQty = appSetting.PagingQty;
            query.CashPaymentText = appSetting.CashPaymentText;
            query.MixPaymentText = appSetting.MixPaymentText;

            query.AutoDefaultPaymentText = appSetting.AutoDefaultPaymentText;
            query.LoadVisitorInCustomerDefine = appSetting.LoadVisitorInCustomerDefine;
            query.GridPaging = appSetting.GridPaging;
            query.WarningRestInApointment = appSetting.WarningRestInApointment;
            query.SaveGridLayout = appSetting.SaveGridLayout;
            query.SaveSizes = appSetting.SaveSizes;
            query.InsertContractAfterOrder = appSetting.InsertContractAfterOrder;
            query.ShowApointmentInStart = appSetting.ShowApointmentInStart;
            query.ShowApointmentInInsert = appSetting.ShowApointmentInInsert;
            query.ShowFollowInStart = appSetting.ShowFollowInStart;
            query.ShowNextApointmentInExit = appSetting.ShowNextApointmentInExit;
            query.ShowNextFollowInExit = appSetting.ShowNextFollowInExit;
            query.ShowNextRestInExit = appSetting.ShowNextRestInExit;
            query.ShowRestInStart = appSetting.ShowRestInStart;
            query.TimeInterval = appSetting.TimeInterval;

            db.SaveChanges();
        }

        public void InserAppSetting(int UserID)
        {
            Entities db = new Entities();

            AppSettings setting = new AppSettings();

            setting.PagingQty = 50;
            setting.PrintQty = 1;
            setting.CashPaymentText = "";
            setting.MixPaymentText = "";

            setting.AutoDefaultPaymentText = true;
            setting.LoadVisitorInCustomerDefine = true;
            setting.GridPaging = true;
            setting.WarningRestInApointment = true;
            setting.SaveGridLayout = true;
            setting.SaveSizes = true;
            setting.InsertContractAfterOrder = true;
            setting.ShowApointmentInStart = true;
            setting.ShowApointmentInInsert = true;
            setting.ShowFollowInStart = true;
            setting.ShowNextApointmentInExit = true;
            setting.ShowNextFollowInExit = true;
            setting.ShowNextRestInExit = true;
            setting.ShowRestInStart = true;
            setting.TimeInterval = "01:00";

            setting.userID = UserID;

            db.AppSettings.Add(setting);

            db.SaveChanges();
        }
    }
}
