using ParsianOffice.Classes;
using System;
using System.Collections.Generic;
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
using BL;
using System.Data;
using System.ComponentModel;
using Arash;
using System.IO;
using DA;
using System.Threading;
using Arash.PersianDateControls;
using MahApps.Metro.Controls.Dialogs;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winApointmentReport.xaml
    /// </summary>
    public partial class winApointmentReport : Window
    {
        public winApointmentReport()
        {

            InitializeComponent();
        }

        string section = "ApointmentReport";
        public winMain main;
        DataTable dataTable;
        BackgroundWorker worker = new BackgroundWorker();

        public string status;

        void LoadData()
        {
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();

            grd.ShowLoadingPanel = true;

        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            grd.ItemsSource = dataTable;
            
            if (File.Exists(clsDateClass.appStartupPath + "\\Grid Layout\\ApointmentReport.xml"))
                grd.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\ApointmentReport.xml");
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (viw.AllowPaging)
            {
                int rowCount = grd.VisibleRowCount;
                int page = rowCount / AppSettingClass.setting.PagingQty.Value;

                if (rowCount % AppSettingClass.setting.PagingQty == 0)
                    viw.CurrentPageIndex = page - 1;
                else
                    viw.CurrentPageIndex = page;
            }
            viw.MoveLastRow();

            grd.ShowLoadingPanel = false;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

            SqlClass ado = new SqlClass();
            ado.ConnectionString = ConnectionStringClass.GetADOConnectionString();
            ado.OpenConnection();

            switch (status)
            {
                case "All":
                    ado.Command("sp_ApointmentReport", CommandType.StoredProcedure);
                    break;
                case "ToDay":
                    PersianDate.Today.ToString();
                    string Date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(PersianDate.Today.ToString()));
                    ado.Command("sp_ApointmentReportDate", CommandType.StoredProcedure, Date);
                    break;
                case "NextDay":
                    string NextDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(PersianDate.Today.AddDays(1).ToString()));
                    ado.Command("sp_ApointmentReportDate", CommandType.StoredProcedure, NextDate);
                    break;
                default:
                    break;
            }


            DataTable dt = ado.ExecuteQuery();

            ado.CloseConnection();

            dataTable = dt;

            worker.ReportProgress(100);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppSettingClass.setting.GridPaging)
                {
                    viw.AllowPaging = true;
                    viw.PageSize = AppSettingClass.setting.PagingQty.Value;
                }

                BLUserClass user = new BLUserClass();

                itmDeleteApointment.Visibility = (user.GetDeleteAccessSection("Apointment", BL.BLUserClass.UserID)) ? Visibility.Visible : Visibility.Collapsed;
                itmDeleteApointment.Visibility = (user.GetUpdateAccessSection("Apointment", BL.BLUserClass.UserID)) ? Visibility.Visible : Visibility.Collapsed;

                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                mainGrid.ColumnDefinitions[0].Width = new GridLength(double.Parse(ConfigClass.getSetting(section, "GridProperties")));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));

                LoadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting(section, "Width", this.ActualWidth);
                ConfigClass.setSetting(section, "Height", this.ActualHeight);

                ConfigClass.setSetting(section, "GridProperties", mainGrid.ColumnDefinitions[0].Width);

                ConfigClass.setSetting(section, "LocationX", this.Left);
                ConfigClass.setSetting(section, "LocationY", this.Top);
            }

            if (AppSettingClass.setting.SaveGridLayout)
            {
                for (int i = 0; i < grd.Columns.Count - 1; i++)
                {
                    string fieldName = grd.Columns[i].FieldName;
                    grd.ClearColumnFilter(fieldName);
                }

                grd.SaveLayoutToXml(clsDateClass.appStartupPath + "\\Grid Layout\\ApointmentReport.xml");
            }
        }

        private void grd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                winDisplayApointment apointment = new winDisplayApointment();
                DataRowView dr = (DataRowView)grd.SelectedItem;
                apointment.ID = int.Parse(dr["ID"].ToString());
                apointment.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void itmShowApointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                winDisplayApointment apointment = new winDisplayApointment();
                DataRowView dr = (DataRowView)grd.SelectedItem;
                apointment.ID = int.Parse(dr["ID"].ToString());
                apointment.ShowDialog();
            }
            catch (Exception)
            {

            }            
        }

        private void itmEditApointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                winApointment apointment = new winApointment();
                apointment.doEdit = true;
                apointment.OnEdit = true;
                apointment.id = int.Parse(dr["ID"].ToString());
                apointment.ShowDialog();

                LoadData();
            }
            catch (Exception)
            {

            }
        }

        private async void itmDeleteApointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                MetroDialogSettings config = new MetroDialogSettings();

                config.AffirmativeButtonText = "بله";
                config.AnimateHide = true;
                config.AnimateShow = true;
                config.ColorScheme = MetroDialogColorScheme.Theme;
                config.DefaultButtonFocus = MessageDialogResult.Affirmative;
                config.DialogResultOnCancel = MessageDialogResult.Negative;
                config.NegativeButtonText = "خیر";

                var res = await main.ShowMessageAsync("", "آیا مایل به حذف قرار ملاقات به شماره " + id.ToString() + " می باشید؟", MessageDialogStyle.AffirmativeAndNegative, config);

                //var res = MessageBox.Show("آیا مایل به حذف قرار ملاقات به شماره " + id.ToString() + " می باشید؟", "تاییدیه", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageDialogResult.Affirmative)
                {
                    BLApointment ap = new BLApointment();
                    ap.Delete(id);
                    LoadData();
                }
            }
            catch (Exception)
            {

            }
        }

        private void itmBargharar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                BLApointment ap = new BLApointment();
                ap.ChangeSituation(id, 1);
                LoadData();
            }
            catch (Exception)
            {

            }
        }

        private void itmCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                winCancelReson cancel = new winCancelReson();
                cancel.ap_ID = id;
                bool res = cancel.ShowDialog().Value;

                if (res)
                {
                    BLApointment ap = new BLApointment();
                    ap.ChangeSituation(id, 2);
                    LoadData();
                }
            }
            catch (Exception)
            {

            }
        }

        private void itmTaligh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                BLApointment ap = new BLApointment();
                ap.ChangeSituation(id, 3);
                LoadData();
            }
            catch (Exception)
            {

            }
        }

        private void itmDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                BLApointment ap = new BLApointment();
                ap.ChangeSituation(id, 4);
                LoadData();
            }
            catch (Exception)
            {

            }
        }

        private void itmPresent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                BLApointment ap = new BLApointment();
                ap.ChangeKind(id, 1);
                LoadData();
            }
            catch (Exception)
            {

            }
        }

        private void itmAmozesh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                BLApointment ap = new BLApointment();
                ap.ChangeKind(id, 3);
                LoadData();
            }
            catch (Exception)
            {

            }
        }

        private void itmNasb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                BLApointment ap = new BLApointment();
                ap.ChangeKind(id, 2);
                LoadData();
            }
            catch (Exception)
            {

            }
        }
        private void itmNasbVaAmozesh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                BLApointment ap = new BLApointment();
                ap.ChangeKind(id, 4);
                LoadData();
            }
            catch (Exception)
            {

            }
        }

        private void itmKhadamat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grd.SelectedItem;
                int id = int.Parse(dr["ID"].ToString());

                BLApointment ap = new BLApointment();
                ap.ChangeKind(id, 5);
                LoadData();
            }
            catch (Exception)
            {

            }
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            grd.ClearColumnFilter("Date");
        }

        private void PersianCalendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PersianCalendar per = (PersianCalendar)sender;
            PersianDate perDate = per.SelectedDate;
            string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(perDate.ToString()));

            grd.Columns["Date"].AutoFilterValue = date;
        }

        private void itmSetting_Click(object sender, RoutedEventArgs e)
        {
            winGridColumnSetting setting = new winGridColumnSetting();
            setting.grd = grd;
            setting.ShowDialog();
        }
    }
}
