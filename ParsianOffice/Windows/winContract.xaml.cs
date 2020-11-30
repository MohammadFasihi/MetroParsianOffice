using Arash;
using BL;
using DA;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.Xpf.Printing;
using DevExpress.XtraReports.UI;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ParsianOffice.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winContract.xaml
    /// </summary>
    public partial class winContract : Window
    {
        public winContract()
        {
            InitializeComponent();
        }

        public MetroWindow main;

        BLUserClass user = new BLUserClass();

        string section = "Contract";
        string fileName = "vbnsprhasuydfcghvaeriqtgiwtfgvasdg";

        bool OnEdit = false;
        public bool isLoaded = false;
        public int orderID;

        int orderPrice, basePrice, discount;
        string orderDate;

        bool IsAutoPrint = false;

        BLContract contract = new BLContract();

        public static int designID;

        DevReport GetReportReady()
        {
            DevReport report = new DevReport();

            DataSet ds = new DataSet();
            DataTable detail;
            DataTable header;

            SqlClass sql = new SqlClass();

            sql.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);

            sql.OpenConnection();

            sql.Command("sp_OrderHeaderPrint", CommandType.StoredProcedure, orderID, "@id");
            header = sql.ExecuteQuery("Header");

            sql.Command("sp_OrderDetailsPrint", CommandType.StoredProcedure, orderID, "@id");
            detail = sql.ExecuteQuery("Detail");

            sql.CloseConnection();

            ds.Tables.Add(header);
            ds.Tables.Add(detail);

            report.DataSource = ds;

            return report;
        }

        public void PrintContract()
        {
            orderID = int.Parse(txtID.Text);

            PrintSettingClass print = new PrintSettingClass();
            byte[] file = null;

            if (PrintSettingClass.setting.LoadDesignBeforePrint)
            {
                winDesignList list = new winDesignList();
                list.ShowDialog();

                file = print.GetDesign(designID);
            }
            else
                file = print.GetDesign(PrintSettingClass.setting.DefaultDesignID);

            DevReport report = new DevReport();
            PrintSettingClass printSetting = new PrintSettingClass();

            int userDefaultDesign = 0;

            DataSet ds = new DataSet();
            DataTable detail;
            DataTable header;

            SqlAdoClass.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);

            SqlAdoClass.Command("sp_OrderHeaderPrint", CommandType.StoredProcedure, orderID, "@id");
            header = SqlAdoClass.ExecuteQuery("Header");

            SqlAdoClass.Command("sp_OrderDetailsPrint", CommandType.StoredProcedure, orderID, "@id");
            detail = SqlAdoClass.ExecuteQuery("Detail");

            designID = printSetting.GetDefaultDesign(BLUserClass.UserID).Value;

            File.WriteAllBytes(clsDateClass.appStartupPath + "\\" + fileName, file);
            report.LoadLayoutFromXml(clsDateClass.appStartupPath + "\\" + fileName);

            ds.Tables.Add(header);
            ds.Tables.Add(detail);

            report.DataSource = ds;
            report.RightToLeft = RightToLeft.Yes;


            if (PrintSettingClass.setting.PreviewBeforePrint)
            {
                for (int i = 1; i <= PrintSettingClass.setting.PrintQty; i++)
                {
                    winPreview preview = new winPreview();
                    preview.report = report;
                    preview.ShowDialog();
                }
            }
            else
            {
                for (int i = 1; i <= PrintSettingClass.setting.PrintQty; i++)
                {
                    DevExpress.XtraReports.UI.ReportDesignTool designTool = new DevExpress.XtraReports.UI.ReportDesignTool(report);
                    designTool.Report.Print();
                }
            }
        }

        void Clear()
        {
            txtID.Text = contract.getLastID().ToString();

            txtOrderID.Text = "";
            radAuto.IsChecked = true;
            radMix.IsChecked = true;
            txtPaymentText.Text = "";
            txtCach.Text = "";
            txtChequeQty.Text = "";
            lblOrderInfo.Text = "";

            if (!AppSettingClass.setting.AutoDefaultPaymentText)
                radManual.IsChecked = true;

            OnEdit = false;
            btnCancel.Content = "خروج";

            btnPrint.IsEnabled = false;
            btnDesign.IsEnabled = false;

            btnSave.Visibility = (user.GetInsertAccessSection("Contract", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            btnDelete.Visibility = (user.GetDeleteAccessSection("Contract", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = user.GetDeleteAccessSection("Contract", BLUserClass.UserID);
        }

        private void InsertCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSave_Click(sender, e);
        }

        private void InsertCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (OnEdit)
                e.CanExecute = (user.GetUpdateAccessSection("Contract", BLUserClass.UserID));
            else
                e.CanExecute = (user.GetInsertAccessSection("Contract", BLUserClass.UserID));
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool auto = radAuto.IsChecked.Value;
                bool mix = radMix.IsChecked.Value;

                string paymentText = "";

                if (auto)
                {
                    if (!mix)
                    {
                        paymentText = AppSettingClass.setting.CashPaymentText.Replace("[مبلغ فاکتور]", basePrice.ToString());
                        paymentText = paymentText.Replace("[مبلغ تخفیف]", discount.ToString());
                        paymentText = paymentText.Replace("[مبلغ کل]", orderPrice.ToString());
                        paymentText = paymentText.Replace("[مبلغ به حروف]", StringClass.GetNumberToCharachter(orderPrice));
                    }
                    else
                    {
                        paymentText = AppSettingClass.setting.MixPaymentText.Replace("[مبلغ فاکتور]", basePrice.ToString());
                        paymentText = paymentText.Replace("[مبلغ تخفیف]", discount.ToString());
                        paymentText = paymentText.Replace("[مبلغ کل]", orderPrice.ToString());
                        paymentText = paymentText.Replace("[نقدی]", txtCach.Text);
                        paymentText = paymentText.Replace("[تعداد چک]", txtChequeQty.Text);
                        paymentText = paymentText.Replace("[مبلغ به حروف]", StringClass.GetNumberToCharachter(orderPrice));
                    }
                }
                else
                {
                    paymentText = txtPaymentText.Text;
                }


                if (!OnEdit)
                {
                    contract.Insert(txtID.Text, txtOrderID.Text, paymentText, BLUserClass.UserID, dpDate.SelectedDate, null);
                }
                else
                {
                    contract.Update(txtID.Text, txtOrderID.Text, paymentText, BLUserClass.UserID, dpDate.SelectedDate, null);
                }

                if (IsAutoPrint)
                    PrintContract();

                if (isLoaded)
                    Close();
                else
                    Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطا در برنامه", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (BLContract.contract != null)
            {
                contract.Delete(BLContract.contract.ID);
                Clear();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
                Clear();
            else
                Close();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            BLContract.contract = null;

            winDisplay display = new winDisplay();
            display.tableName = "Contract";
            display.ShowDialog();


            if (BLContract.contract != null)
            {
                orderID = BLContract.contract.ID;
                txtOrderID.Text = BLContract.contract.order_ID.ToString();
                txtID.Text = BLContract.contract.ID.ToString();
                dpDate.SelectedDate = PersianDate.Parse(BLContract.contract.sh_Date);
                txtPaymentText.Text = BLContract.contract.payment_Text;

                lblOrderInfo.Text = "مبلغ فاکتور : " + BLContract.contract.tbl_OrderHeader.order_Price + " تاریخ صدور : " + BLContract.contract.tbl_OrderHeader.sh_Date + " طرف قرارداد : " + BLContract.contract.tbl_OrderHeader.tbl_Customer.cu_Name;

                radManual.IsChecked = true;

                OnEdit = true;
                btnCancel.Content = "انصراف";

                btnPrint.IsEnabled = true;
                btnDesign.IsEnabled = true;

                btnSave.Visibility = (user.GetUpdateAccessSection("Contract", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                chkPrint.IsChecked = bool.Parse(ConfigClass.getSetting(section, "AutoPrint"));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));
            }
            catch (Exception)
            {

            }

            if (isLoaded)
            {
                btnOrderSearch_Click(null, null);
                txtID.Text = contract.getLastID().ToString();
            }
            else
                Clear();

            btnSave.Visibility = (user.GetInsertAccessSection("Contract", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
        }

        private void btnOrderSearch_Click(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
            {
                BLOrder order = new BLOrder(orderID);
            }
            else
            {
                BLOrder.order = null;
            }

            if (BLOrder.order == null)
            {
                winDisplay display = new winDisplay();
                display.tableName = "OrderHeader";
                display.kind = 1;
                display.ShowDialog();
            }

            if (BLOrder.order != null)
            {
                txtOrderID.Text = BLOrder.order.ID.ToString();
                basePrice = BLOrder.order.order_BasePrice.Value;
                discount = BLOrder.order.order_Dsicount.Value;
                orderPrice = BLOrder.order.order_Price.Value;
                orderDate = BLOrder.order.sh_Date;

                lblOrderInfo.Text = "مبلغ فاکتور : " + orderPrice + " تاریخ صدور : " + orderDate + " طرف قرارداد : " + BLOrder.order.tbl_Customer.cu_Name;
            }
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = OnEdit;
        }

        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintContract();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintContract();
        }

        private async void btnDesign_Click(object sender, RoutedEventArgs e)
        {
            DevReport report = new DevReport();
            PrintSettingClass printSetting = new PrintSettingClass();

            int userDefaultDesign = 0;

            DataSet ds = new DataSet();
            DataTable detail;
            DataTable header;

            SqlAdoClass.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);

            SqlAdoClass.Command("sp_OrderHeaderPrint", CommandType.StoredProcedure, orderID, "@id");
            header = SqlAdoClass.ExecuteQuery("Header");

            SqlAdoClass.Command("sp_OrderDetailsPrint", CommandType.StoredProcedure, orderID, "@id");
            detail = SqlAdoClass.ExecuteQuery("Detail");

            designID = printSetting.GetDefaultDesign(BLUserClass.UserID).Value;

            byte[] file = printSetting.GetDesign(designID);

            File.WriteAllBytes(clsDateClass.appStartupPath + "\\" + fileName, file);
            report.LoadLayoutFromXml(clsDateClass.appStartupPath + "\\" + fileName);

            ds.Tables.Add(header);
            ds.Tables.Add(detail);

            report.DataSource = ds;
            report.RightToLeft = RightToLeft.Yes;

            DevExpress.XtraReports.UI.ReportDesignTool designTool = new DevExpress.XtraReports.UI.ReportDesignTool(report);
            designTool.ShowDesignerDialog();

            MetroDialogSettings config = new MetroDialogSettings();

            config.AffirmativeButtonText = "بله";
            config.AnimateHide = true;
            config.AnimateShow = true;
            config.ColorScheme = MetroDialogColorScheme.Theme;
            config.DefaultButtonFocus = MessageDialogResult.Affirmative;
            config.DialogResultOnCancel = MessageDialogResult.Canceled;
            config.NegativeButtonText = "خیر";
            config.FirstAuxiliaryButtonText = "انصراف";

            var res = await main.ShowMessageAsync("", "آیا مایل به ذخیره طرح می باشید؟", MessageDialogStyle.AffirmativeAndNegative, config);

            //MessageBoxResult res = MessageBox.Show("آیا مایل به ذخیره طرح می باشید؟", "ذخیره طرح", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (res == MessageDialogResult.Affirmative)
            {
                report.SaveLayoutToXml(clsDateClass.appStartupPath + "\\" + fileName);
                byte[] reportFile = File.ReadAllBytes(clsDateClass.appStartupPath + "\\" + fileName);
                userDefaultDesign = int.Parse(printSetting.GetDefaultDesign(BLUserClass.UserID).Value.ToString());

                printSetting.SaveDesign(userDefaultDesign, reportFile);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting(section, "Width", this.ActualWidth);
                ConfigClass.setSetting(section, "Height", this.ActualHeight);

                ConfigClass.setSetting(section, "AutoPrint", chkPrint.IsChecked.Value);

                ConfigClass.setSetting(section, "LocationX", this.Left);
                ConfigClass.setSetting(section, "LocationY", this.Top);
            }
        }

        private void chkPrint_Checked(object sender, RoutedEventArgs e)
        {
            IsAutoPrint = chkPrint.IsChecked.Value;
        }
    }
}
