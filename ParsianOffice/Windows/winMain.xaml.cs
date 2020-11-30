using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using BL;
using ParsianOffice.ReportWindows;
using DA;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Drawing;
using ParsianOffice.Classes;
using System.Data.SqlClient;
using System.Data;

namespace ParsianOffice.Windows
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "Exit",
                "Exit",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.End, ModifierKeys.Control)
                }
            );
    }
    /// <summary>
    /// Interaction logic for winMain.xaml
    /// </summary>
    /// ConnectionStringClass.GetConnectionString()
    public partial class winMain : MetroWindow
    {
        public winMain()
        {
            InitializeComponent();
        }

        public winLogin login;

        bool isLogOut = false;

        DispatcherTimer time = new DispatcherTimer();
        float opacityStep = -0.01f;

        bool mustLoad = false;
        bool AllowToExit = false;
        void ShowReport()
        {
            if (AppSettingClass.setting.ShowApointmentInStart || AppSettingClass.setting.ShowFollowInStart || AppSettingClass.setting.ShowRestInStart)
            {
                winReports report = new winReports();
                report.ShowDialog();
            }
        }

        void ShowExitReport()
        {
            if (AppSettingClass.setting.ShowNextApointmentInExit || AppSettingClass.setting.ShowNextFollowInExit || AppSettingClass.setting.ShowNextRestInExit)
            {
                winReportsExit report = new winReportsExit();
                report.ShowDialog();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtCurrentUser.Text = BLUserClass.Username;

            ADOClass.ConnectionString = ConnectionStringClass.GetADOConnectionString(30);
            ADOClass.OpenConnection();

            BLUserClass user = new BLUserClass();
            user.GetUserAccess();

            isLogOut = false;

            itmIssue.Visibility = (BLUserClass.Issue) ? Visibility.Visible : Visibility.Collapsed;
            itmCustomer.Visibility = (BLUserClass.Customer) ? Visibility.Visible : Visibility.Collapsed;
            itmExport.Visibility = (BLUserClass.Expert) ? Visibility.Visible : Visibility.Collapsed;
            itmVisitor.Visibility = (BLUserClass.Visitor) ? Visibility.Visible : Visibility.Collapsed;
            itmApointment.Visibility = (BLUserClass.Apointment) ? Visibility.Visible : Visibility.Collapsed;
            itmReminder.Visibility = (BLUserClass.FollowUp) ? Visibility.Visible : Visibility.Collapsed;
            itmSaleBill.Visibility = (BLUserClass.Order) ? Visibility.Visible : Visibility.Collapsed;
            itmReturnBill.Visibility = (BLUserClass.OrderReturn) ? Visibility.Visible : Visibility.Collapsed;
            itmRest.Visibility = (BLUserClass.Rest) ? Visibility.Visible : Visibility.Collapsed;
            itmContract.Visibility = (BLUserClass.Contract) ? Visibility.Visible : Visibility.Collapsed;
            itmAppointmentReport.Visibility = (BLUserClass.ApointmentReport) ? Visibility.Visible : Visibility.Collapsed;
            itmToDayAppointmentReport.Visibility = (BLUserClass.TodayApointmentReport) ? Visibility.Visible : Visibility.Collapsed;
            itmNextDayAppointmentReport.Visibility = (BLUserClass.NextDayApointmentReport) ? Visibility.Visible : Visibility.Collapsed;
            itmCustomerFollowUpReport.Visibility = (BLUserClass.CustomerFollowUpReport) ? Visibility.Visible : Visibility.Collapsed;
            itmTodayFollowUpReport.Visibility = (BLUserClass.TodayFollowUpReport) ? Visibility.Visible : Visibility.Collapsed;
            itmVacation.Visibility = (BLUserClass.VacationReport) ? Visibility.Visible : Visibility.Collapsed;
            itmVisitorReport.Visibility = (BLUserClass.VisitorReport) ? Visibility.Visible : Visibility.Collapsed;
            itmOrderReport.Visibility = (BLUserClass.OrderReport) ? Visibility.Visible : Visibility.Collapsed;
            itmBackup.Visibility = (BLUserClass.Backup) ? Visibility.Visible : Visibility.Collapsed;
            itmRestore.Visibility = (BLUserClass.Restore) ? Visibility.Visible : Visibility.Collapsed;
            itmUsers.Visibility = (BLUserClass.Users) ? Visibility.Visible : Visibility.Collapsed;

            AppSettingClass.GetAppSetting(BLUserClass.UserID);
            PrintSettingClass print = new PrintSettingClass(BLUserClass.UserID);

            txtPagingQty.Text = AppSettingClass.setting.PagingQty.ToString();
            txtCashPaymentText.Text = AppSettingClass.setting.CashPaymentText;
            txtMixPaymentText.Text = AppSettingClass.setting.MixPaymentText;

            tsbAutoDefault.IsChecked = AppSettingClass.setting.AutoDefaultPaymentText;
            tsbLoadVisitor.IsChecked = AppSettingClass.setting.LoadVisitorInCustomerDefine;
            tsbGridPaging.IsChecked = AppSettingClass.setting.GridPaging;
            tsbRestWarninigInApointmentInsert.IsChecked = AppSettingClass.setting.WarningRestInApointment;
            tsbSaveGriLayout.IsChecked = AppSettingClass.setting.SaveGridLayout;
            tsbSaveSize.IsChecked = AppSettingClass.setting.SaveSizes;
            tsbSetContractInOrderInsert.IsChecked = AppSettingClass.setting.InsertContractAfterOrder;
            tsbShowApointmentInStart.IsChecked = AppSettingClass.setting.ShowApointmentInStart;
            tsbShowAppointmentsInInsert.IsChecked = AppSettingClass.setting.ShowApointmentInInsert;
            tsbShowFollowInStart.IsChecked = AppSettingClass.setting.ShowFollowInStart;
            tsbShowNextApointmentInExit.IsChecked = AppSettingClass.setting.ShowNextApointmentInExit;
            tsbShowNextFollowInExit.IsChecked = AppSettingClass.setting.ShowNextFollowInExit;
            tsbShowNextRestInExit.IsChecked = AppSettingClass.setting.ShowNextRestInExit;
            tsbShowRestInStart.IsChecked = AppSettingClass.setting.ShowRestInStart;
            txtTimeInterval.Text = AppSettingClass.setting.TimeInterval;

            ShowReport();

            if (BLUserClass.Kind == "User")
                itmManagerSetting.Visibility = Visibility.Collapsed;
        }

        public async void ConfirmationClosingAsync()
        {
            MetroDialogSettings setting = new MetroDialogSettings();
            setting.AffirmativeButtonText = "بله";
            setting.AnimateHide = true;
            setting.AnimateShow = true;
            setting.ColorScheme = MetroDialogColorScheme.Theme;
            setting.DefaultButtonFocus = MessageDialogResult.Affirmative;
            setting.DialogResultOnCancel = MessageDialogResult.Canceled;
            setting.NegativeButtonText = "خیر";

            var res = await this.ShowMessageAsync("", "آیا مایل به خروج از برنامه هستید؟", MessageDialogStyle.AffirmativeAndNegative, setting);

            

            if (res == MessageDialogResult.Affirmative)
            {
                ShowExitReport();
                clsLanguage.RestoreLanguage();
                Application.Current.Shutdown();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isLogOut)
            {
                login.Show();
            }
            else
            {
                e.Cancel = true;
                CancellationToken token;
                TaskScheduler uiSched = TaskScheduler.FromCurrentSynchronizationContext();
                Task.Factory.StartNew(ConfirmationClosingAsync, token, TaskCreationOptions.None, uiSched);
            }
        }

        private void itmDefineIssuse_Click(object sender, RoutedEventArgs e)
        {
            winDefineIssuse issues = new winDefineIssuse();
            issues.main = this;
            issues.Show();
        }

        private void itmDefineVisitor_Click(object sender, RoutedEventArgs e)
        {
            winVisitor visitor = new winVisitor();
            visitor.main = this;
            visitor.Show();
        }

        private void itmDefineCustomer_Click(object sender, RoutedEventArgs e)
        {
            winCustomer customer = new winCustomer();
            customer.main = this;
            customer.Show();
        }

        private void itmDefineExpert_Click(object sender, RoutedEventArgs e)
        {
            winExpress exp = new winExpress();
            exp.main = this;
            exp.Show();
        }

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Close()
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            itmDefineIssuse_Click(null, null);
        }

        private void itmAppolintment_Click(object sender, RoutedEventArgs e)
        {
            winApointment apointment = new winApointment();
            apointment.main = this;
            apointment.Show();
        }

        private void itmOrder_Click(object sender, RoutedEventArgs e)
        {
            winOrder order = new winOrder();
            order.orderKindID = 1;
            order.Title = "فاکتور فروش";
            order.main = this;
            order.Show();
        }

        private void itmOrderReturn_Click(object sender, RoutedEventArgs e)
        {
            winOrder order = new winOrder();
            order.orderKindID = -1;
            order.Title = "فاکتور مرجوعی";
            order.main = this;
            order.Show();
        }

        private void itmFollowUp_Click(object sender, RoutedEventArgs e)
        {
            winFollowUp follow = new winFollowUp();
            follow.Show();
        }

        private void itmRest_Click(object sender, RoutedEventArgs e)
        {
            winVacation vac = new winVacation();
            vac.Show();
        }

        private void itmContract_Click(object sender, RoutedEventArgs e)
        {
            winContract contract = new winContract();
            contract.main = this;
            contract.Show();
        }

        private void itmAppointmentReport_Click(object sender, RoutedEventArgs e)
        {
            winApointmentReport report = new winApointmentReport();
            report.status = "All";
            report.Show();
        }

        private void itmCustomerAppointmentReport_Click(object sender, RoutedEventArgs e)
        {
            winApointmentReport report = new winApointmentReport();
            report.status = "All";
            report.Show();
        }

        private void itmNextDayAppointmentReport_Click(object sender, RoutedEventArgs e)
        {
            winApointmentReport report = new winApointmentReport();
            report.status = "NextDay";
            report.Show();
        }

        private void itmToDayAppointmentReport_Click(object sender, RoutedEventArgs e)
        {
            winApointmentReport report = new winApointmentReport();
            report.status = "ToDay";
            report.Show();
        }

        private void itmCustomerFollowUpReport_Click(object sender, RoutedEventArgs e)
        {
            winFollowUpReport report = new winFollowUpReport();
            report.status = "All";
            report.Show();
        }

        private void itmTodayFollowUpReport_Click(object sender, RoutedEventArgs e)
        {
            winFollowUpReport report = new winFollowUpReport();
            report.status = "ToDay";
            report.Show();
        }

        private void itmVacation_Click(object sender, RoutedEventArgs e)
        {
            winVacationReport report = new winVacationReport();
            report.Show();
        }

        private void itmVisitorReport_Click(object sender, RoutedEventArgs e)
        {
            winVisitorSaleReport report = new winVisitorSaleReport();
            report.Show();
        }

        private void itmOrdersReport_Click(object sender, RoutedEventArgs e)
        {
            winOrdersReport report = new winOrdersReport();
            report.Show();
        }

        private void itmAppSetting_Click(object sender, RoutedEventArgs e)
        {
            winAppSetting setting = new winAppSetting();
            setting.ShowDialog();

            AppSettingClass.GetAppSetting(BLUserClass.UserID);
        }

        private void itmPrintSetting_Click(object sender, RoutedEventArgs e)
        {
            winPrintingSetting setting = new winPrintingSetting();
            setting.main = this;
            setting.ShowDialog();
        }

        private void itmUsers_Click(object sender, RoutedEventArgs e)
        {
            winUsersSettings userSetting = new winUsersSettings();
            userSetting.Show();
        }

        private void itmBackup_Click(object sender, RoutedEventArgs e)
        {
            winBackup backup = new winBackup();
            backup.Show();
        }

        private void itmRestore_Click(object sender, RoutedEventArgs e)
        {
            winRestore restore = new winRestore();
            restore.Show();
        }


        private void itmManagerSetting_Click(object sender, RoutedEventArgs e)
        {
            winAdminSettings settings = new winAdminSettings();
            settings.Show();
        }

        private void btnUserSetting_Click(object sender, RoutedEventArgs e)
        {
            flySetting.IsOpen = !flySetting.IsOpen;

            if (!flySetting.IsOpen)
            {
                try
                {
                    string stpName = "";
                    stpName = "sp_UpdateOneUserSetting";

                    AppSettingClass setting = new AppSettingClass();

                    SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = con;
                    cmd.CommandText = stpName;
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@PagingQty", byte.Parse(txtPagingQty.Text));
                    cmd.Parameters.AddWithValue("@CashPaymentText", txtCashPaymentText.Text);
                    cmd.Parameters.AddWithValue("@MixPaymentText", txtMixPaymentText.Text);

                    cmd.Parameters.AddWithValue("@AutoDefaultPaymentText", tsbAutoDefault.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@LoadVisitorInCustomerDefine", tsbLoadVisitor.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@GridPaging", tsbGridPaging.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@WarningRestInApointment", tsbRestWarninigInApointmentInsert.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@SaveGridLayout", tsbSaveGriLayout.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@SaveSizes", tsbSaveSize.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@InsertContractAfterOrder", tsbSetContractInOrderInsert.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@ShowApointmentInStart", tsbShowApointmentInStart.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@ShowApointmentInInsert", tsbShowAppointmentsInInsert.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@ShowFollowInStart", tsbShowFollowInStart.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@ShowNextApointmentInExit", tsbShowNextApointmentInExit.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@ShowNextFollowInExit", tsbShowNextFollowInExit.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@ShowNextRestInExit", tsbShowNextRestInExit.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@ShowRestInStart", tsbShowRestInStart.IsChecked.Value);
                    cmd.Parameters.AddWithValue("@TimeInterval", txtTimeInterval.Text);
                    cmd.Parameters.AddWithValue("@userID", BLUserClass.UserID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "خطا در سیستم", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PersianCalendar_MouseEnter(object sender, MouseEventArgs e)
        {
            opacityStep = -opacityStep;
            time.Interval = TimeSpan.FromMilliseconds(5);
            time.Tick += time_Tick;
            time.Start();
        }

        private void time_Tick(object sender, EventArgs e)
        {
            perDate.Opacity += opacityStep;

            if (opacityStep > 0)
            {
                if (perDate.Opacity >= 1f)
                    time.Stop();
            }
            else
            {
                if (perDate.Opacity <= 0.5f)
                    time.Stop();
            }
        }

        private void perDate_MouseLeave(object sender, MouseEventArgs e)
        {
            opacityStep = -opacityStep;
            time.Start();
        }

        private void itmIssue_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void perDate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                winApointment apointment = new winApointment();
                apointment.doApointment = true;
                apointment.date = perDate.SelectedDate;

                perDate.SelectedDate = Arash.PersianDate.Today;

                apointment.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void itmChangePassword_Click(object sender, RoutedEventArgs e)
        {
            winChangePassword changePass = new winChangePassword();
            changePass.ShowDialog();
        }

        private void itmLogOut_Click(object sender, RoutedEventArgs e)
        {
            isLogOut = true;
            Close();
        }

        private void tsbSaveSize_IsCheckedChanged(object sender, EventArgs e)
        {

        }

        private void flySetting_ClosingFinished(object sender, RoutedEventArgs e)
        {
            AppSettingClass.GetAppSetting(BLUserClass.UserID);

            txtPagingQty.Text = AppSettingClass.setting.PagingQty.ToString();
            txtCashPaymentText.Text = AppSettingClass.setting.CashPaymentText;
            txtMixPaymentText.Text = AppSettingClass.setting.MixPaymentText;

            tsbAutoDefault.IsChecked = AppSettingClass.setting.AutoDefaultPaymentText;
            tsbLoadVisitor.IsChecked = AppSettingClass.setting.LoadVisitorInCustomerDefine;
            tsbGridPaging.IsChecked = AppSettingClass.setting.GridPaging;
            tsbRestWarninigInApointmentInsert.IsChecked = AppSettingClass.setting.WarningRestInApointment;
            tsbSaveGriLayout.IsChecked = AppSettingClass.setting.SaveGridLayout;
            tsbSaveSize.IsChecked = AppSettingClass.setting.SaveSizes;
            tsbSetContractInOrderInsert.IsChecked = AppSettingClass.setting.InsertContractAfterOrder;
            tsbShowApointmentInStart.IsChecked = AppSettingClass.setting.ShowApointmentInStart;
            tsbShowAppointmentsInInsert.IsChecked = AppSettingClass.setting.ShowApointmentInInsert;
            tsbShowFollowInStart.IsChecked = AppSettingClass.setting.ShowFollowInStart;
            tsbShowNextApointmentInExit.IsChecked = AppSettingClass.setting.ShowNextApointmentInExit;
            tsbShowNextFollowInExit.IsChecked = AppSettingClass.setting.ShowNextFollowInExit;
            tsbShowNextRestInExit.IsChecked = AppSettingClass.setting.ShowNextRestInExit;
            tsbShowRestInStart.IsChecked = AppSettingClass.setting.ShowRestInStart;
            txtTimeInterval.Text = AppSettingClass.setting.TimeInterval;
        }

        private void flySetting_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            btnUserSetting.Content = (flySetting.IsOpen) ? "ذخیره تغییرات" : "تنظیمات کاربر جاری";
        }
    }
}
