using System;
using System.Windows;
using System.Windows.Input;
using BL;
using DA;
using System.Data;
using ParsianOffice.Classes;
using System.IO;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winDisplay.xaml
    /// </summary>
    public partial class winDisplay : Window
    {
        public winDisplay()
        {
            InitializeComponent();
        }

        public string tableName;
        public int kind;

        void ContractHeaderText()
        {
            grdMain.Columns["ID"].Header = "شماره قرارداد";
            grdMain.Columns["ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["payment_Text"].Header = "شرایط پرداخت";
            grdMain.Columns["payment_Text"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["sh_Date"].Header = "تاریخ";
            grdMain.Columns["sh_Date"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["order_ID"].Header = "شماره فاکتور";
            grdMain.Columns["order_ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["order_BasePrice"].Header = "مبلغ فاکتور";
            grdMain.Columns["order_BasePrice"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["order_Dsicount"].Header = "تخفیف";
            grdMain.Columns["order_Dsicount"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["order_Price"].Header = "مبلغ نهایی";
            grdMain.Columns["order_Price"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Name"].Header = "نام مشتری";
            grdMain.Columns["cu_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vt_ID"].Header = "کد بازاریاب";
            grdMain.Columns["vt_ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["vt_Name"].Header = "نام بازاریاب";
            grdMain.Columns["vt_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

        }

        public void VacationHeaderText()
        {
            grdMain.Columns["ID"].Header = "کد";
            grdMain.Columns["ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["ep_Name"].Header = "نام کارشناس";
            grdMain.Columns["ep_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vc_StartDate"].Header = "تاریخ شروع میلادی";
            grdMain.Columns["vc_StartDate"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vc_EndDate"].Header = "تاریخ پایان میلادی";
            grdMain.Columns["vc_EndDate"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ep_ID"].Header = "کد کارشناس";
            grdMain.Columns["ep_ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["sh_StartDate"].Header = "تاریخ شروع";
            grdMain.Columns["sh_StartDate"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
            grdMain.Columns["sh_StartDate"].AllowedBetweenFilters = DevExpress.Xpf.Grid.AllowedBetweenFilters.Between;

            grdMain.Columns["sh_EndDate"].Header = "تاریخ پایان";
            grdMain.Columns["sh_EndDate"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
            grdMain.Columns["sh_EndDate"].AllowedBetweenFilters = DevExpress.Xpf.Grid.AllowedBetweenFilters.Between;

            grdMain.Columns["startTime"].Header = "ساعت شروع";
            grdMain.Columns["startTime"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["endTime"].Header = "ساعت پایان";
            grdMain.Columns["endTime"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
        }
        public void FollowHeaderText()
        {
            grdMain.Columns["ID"].Header = "کد پیگیری";
            grdMain.Columns["ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["cu_Name"].Header = "نام مشتری";
            grdMain.Columns["cu_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["fl_Text"].Header = "متن پیگیری";
            grdMain.Columns["fl_Text"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_NationalCode"].Header = "کد ملی";
            grdMain.Columns["cu_NationalCode"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Email"].Header = "ایمیل";
            grdMain.Columns["cu_Email"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Address"].Header = "آدرس مشتری";
            grdMain.Columns["cu_Address"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vt_Name"].Header = "نام بازاریاب";
            grdMain.Columns["vt_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["visitorID"].Header = "کد بازاریاب";
            grdMain.Columns["visitorID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["fl_Date"].Header = "تاریخ میلادی";
            grdMain.Columns["fl_Date"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
            grdMain.Columns["fl_Date"].FilterPopupMode = DevExpress.Xpf.Grid.FilterPopupMode.Date;
            grdMain.Columns["fl_Date"].AllowedBetweenFilters = DevExpress.Xpf.Grid.AllowedBetweenFilters.Between;

            grdMain.Columns["Date"].Header = "تاریخ";
            grdMain.Columns["Date"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
            grdMain.Columns["Date"].AllowedBetweenFilters = DevExpress.Xpf.Grid.AllowedBetweenFilters.Between;

            grdMain.Columns["username"].Header = "کاربر ثبت";
            grdMain.Columns["username"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
        }
        public void ApointmentHeaderText()
        {
            grdMain.Columns["ID"].Header = "سریال";
            grdMain.Columns["ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ep_Name"].Header = "نام کارشناس";
            grdMain.Columns["ep_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ap_Date"].Header = "تاریخ میلادی";
            grdMain.Columns["ap_Date"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
            grdMain.Columns["ap_Date"].AllowedBetweenFilters = DevExpress.Xpf.Grid.AllowedBetweenFilters.Between;
            grdMain.Columns["ap_Date"].FilterPopupMode = DevExpress.Xpf.Grid.FilterPopupMode.Date;


            grdMain.Columns["ap_Area"].Header = "منطقه";
            grdMain.Columns["ap_Area"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Name"].Header = "نام مشتری";
            grdMain.Columns["cu_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Phone"].Header = "شماره تماس";
            grdMain.Columns["cu_Phone"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_NationalCode"].Header = "کد ملی";
            grdMain.Columns["cu_NationalCode"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_ID"].Header = "کد مشتری";
            grdMain.Columns["cu_ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["cu_Email"].Header = "آدرس ایمیل";
            grdMain.Columns["cu_Email"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Address"].Header = "آدرس";
            grdMain.Columns["cu_Address"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vt_Name"].Header = "نام بازاریاب";
            grdMain.Columns["vt_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vt_ID"].Header = "کد بازاریاب";
            grdMain.Columns["vt_ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["st_Name"].Header = "وضعیت";
            grdMain.Columns["st_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["kind_Name"].Header = "نوع قرار";
            grdMain.Columns["kind_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["username"].Header = "نام کاربر";
            grdMain.Columns["username"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["Date"].Header = "تاریخ";
            grdMain.Columns["Date"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
            grdMain.Columns["Date"].AllowedBetweenFilters = DevExpress.Xpf.Grid.AllowedBetweenFilters.Between;

            grdMain.Columns["Time"].Header = "ساعت";
            grdMain.Columns["Time"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
        }

        private void OrderHeaderText()
        {
            grdMain.Columns["orderHeader"].Header = "کد سفارش";
            grdMain.Columns["orderHeader"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["cu_Name"].Header = "نام مشتری";
            grdMain.Columns["cu_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vt_Name"].Header = "نام بازاریاب";
            grdMain.Columns["vt_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["sh_Date"].Header = "تاریخ";
            grdMain.Columns["sh_Date"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["order_BasePrice"].Header = "مبلغ کل";
            grdMain.Columns["order_BasePrice"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["order_Dsicount"].Header = "تخفیف";
            grdMain.Columns["order_Dsicount"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["order_Price"].Header = "مبلغ نهایی";
            grdMain.Columns["order_Price"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["st_Name"].Header = "وضعیت";
            grdMain.Columns["st_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["username"].Header = "کاربر ثبت";
            grdMain.Columns["username"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["visitor_ID"].Header = "کد بازاریاب";
            grdMain.Columns["visitor_ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["cu_Address"].Header = "آدرس";
            grdMain.Columns["cu_Address"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["kind_name"].Header = "نوع";
            grdMain.Columns["kind_name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
        }

        public void ExpertHeaderText()
        {
            grdMain.Columns["ID"].Header = "کد";
            grdMain.Columns["ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["ep_Name"].Header = "نام کارشناس";
            grdMain.Columns["ep_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ep_NationalCode"].Header = "کد ملی";
            grdMain.Columns["ep_NationalCode"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ep_Address"].Header = "آدرس";
            grdMain.Columns["ep_Address"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ep_IsActive"].Header = "فعال";
            grdMain.Columns["ep_IsActive"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;
        }

        public void CustomerHeaderText()
        {
            grdMain.Columns["cu_ID"].Header = "کد";
            grdMain.Columns["cu_ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["cu_Name"].Header = "نام مشتری";
            grdMain.Columns["cu_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Email"].Header = "ایمیل";
            grdMain.Columns["cu_Email"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_NationalCode"].Header = "کد ملی/شماره ثبت";
            grdMain.Columns["cu_NationalCode"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Address"].Header = "آدرس";
            grdMain.Columns["cu_Address"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vt_Name"].Header = "نام بازاریاب";
            grdMain.Columns["vt_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["vt_ID"].Header = "کد بازاریاب";
            grdMain.Columns["vt_ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["cu_CustomName"].Header = "نام اختصاری";
            grdMain.Columns["cu_CustomName"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_PhoneNumber"].Header = "شماره موبایل";
            grdMain.Columns["cu_PhoneNumber"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_Tell"].Header = "شماره تلفن";
            grdMain.Columns["cu_Tell"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["cu_PostalCod"].Header = "کد پستی";
            grdMain.Columns["cu_PostalCod"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
        }

        public void IssueHeaderText()
        {
            grdMain.Columns["issue_Name"].Header = "عنوان نسخه";
            grdMain.Columns["issue_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["issue_Price"].Header = "قیمت نسخه";
            grdMain.Columns["issue_Price"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ID"].Header = "شماره";
            grdMain.Columns["ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

        }

        private void VisitorHeaderText()
        {
            grdMain.Columns["ID"].Header = "کد";
            grdMain.Columns["ID"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

            grdMain.Columns["ep_Name"].Header = "نام بازاریاب";
            grdMain.Columns["ep_Name"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ep_NationalCode"].Header = "کد ملی";
            grdMain.Columns["ep_NationalCode"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ep_Address"].Header = "آدرس";
            grdMain.Columns["ep_Address"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;

            grdMain.Columns["ep_IsActive"].Header = "فعال";
            grdMain.Columns["ep_IsActive"].AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Equals;

        }

        void ClearFilter()
        {
            for (int i = 0; i < grdMain.Columns.Count - 1; i++)
            {
                string fieldName = grdMain.Columns[i].FieldName;
                grdMain.ClearColumnFilter(fieldName);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch(tableName)
            {
                case "Issue":
                    BLIssuse issue = new BLIssuse();
                    grdMain.ItemsSource = issue.GetAllIssues();
                    IssueHeaderText();
                    break;
                case "Visitor":
                    BLVisitor visitor = new BLVisitor();
                    grdMain.ItemsSource = visitor.GetAllVisitors();
                    VisitorHeaderText();
                    break;
                case "Customer":
                    BLCustomer customer = new BLCustomer();
                    grdMain.ItemsSource = customer.GetAllCustomer();
                    CustomerHeaderText();
                    break;
                case "Expert":
                    BLExpert expert = new BLExpert();
                    grdMain.ItemsSource = expert.GetAllExports();
                    ExpertHeaderText();
                    break;
                case "Apointment":
                    BLApointment apointment = new BLApointment();
                    grdMain.ItemsSource = apointment.getApointment();
                    ApointmentHeaderText();
                    break;
                case "OrderHeader":
                    BLOrder order = new BLOrder();
                    grdMain.ItemsSource = order.GetAllOrders(kind);
                    OrderHeaderText();
                    break;
                case "FollowUp":
                    DA.Entities db = new DA.Entities();
                    SqlAdoClass.Command("sp_DisplayFollowUp", System.Data.CommandType.StoredProcedure);
                    grdMain.ItemsSource = SqlAdoClass.ExecuteQuery();
                    FollowHeaderText();
                    break;
                case "Vacation":
                    SqlAdoClass.Command("sp_DisplayVacation", System.Data.CommandType.StoredProcedure);
                    grdMain.ItemsSource = SqlAdoClass.ExecuteQuery();
                    VacationHeaderText();
                    break;
                case "Contract":
                    BLContract con = new BLContract();
                    grdMain.ItemsSource = con.GetAllContract();
                    ContractHeaderText();
                    break;
            }

            if (AppSettingClass.setting.SaveGridLayout)
            {
                if (File.Exists(clsDateClass.appStartupPath + "\\Grid Layout\\" + tableName + "Display.xml"))
                    grdMain.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\" + tableName + "Display.xml");
            }
        }


        private void grdMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdMain.SelectedItem != null)
            {
                DataRowView dr;
                dr = (DataRowView)grdMain.SelectedItem;
                switch (tableName)
                {
                    case "Issue":
                        int id = int.Parse(dr["ID"].ToString());
                        BLIssuse issue = new BLIssuse(id);
                        this.Close();
                        break;
                    case "Visitor":
                        int vis = int.Parse(dr["ID"].ToString());
                        BLVisitor visitor = new BLVisitor(vis);
                        this.Close();
                        break;
                    case "Customer":
                        int cu = int.Parse(dr["cu_ID"].ToString());
                        BLCustomer customer = new BLCustomer(cu);
                        this.Close();
                        break;
                    case "Expert":
                        int exp = int.Parse(dr["ID"].ToString());
                        BLExpert expert = new BLExpert(exp);
                        this.Close();
                        break;
                    case "Apointment":
                        int ap = int.Parse(dr["ID"].ToString());
                        BLApointment apointment = new BLApointment(ap);
                        this.Close();
                        break;
                    case "OrderHeader":
                        int order = int.Parse(dr["orderHeader"].ToString());
                        BLOrder orderHrader = new BLOrder(order);
                        this.Close();
                        break;
                    case "FollowUp":
                        int follow = int.Parse(dr["ID"].ToString());
                        winFollowUp.ID = follow;
                        this.Close();
                        break;
                    case "Vacation":
                        int vcID = int.Parse(dr["ID"].ToString());
                        winVacation.id = vcID;
                        this.Close();
                        break;
                    case "Contract":
                        int con = int.Parse(dr["ID"].ToString());
                        BLContract cont = new BLContract(con);
                        this.Close();
                        break;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppSettingClass.setting.SaveGridLayout)
            {
                ClearFilter();
                grdMain.SaveLayoutToXml(clsDateClass.appStartupPath + "\\Grid Layout\\" + tableName + "Display.xml");
            }
        }
    }
}
