using Arash;
using BL;
using DA;
using ParsianOffice.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winReports.xaml
    /// </summary>
    public partial class winReports : Window
    {
        public winReports()
        {
            InitializeComponent();
        }

        string section = "Reports";

        BackgroundWorker worker;
        BackgroundWorker followWorker;
        BackgroundWorker vacationWorker;


        DataTable apointmentDataTable = new DataTable();
        DataTable followDataTable = new DataTable();
        DataTable vacationDataTable = new DataTable();

        void LoadApointmentData()
        {
            worker = new BackgroundWorker();

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();

            grdApointment.ShowLoadingPanel = true;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            grdApointment.ShowLoadingPanel = false;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            grdApointment.ItemsSource = apointmentDataTable;

            if (File.Exists(clsDateClass.appStartupPath + "\\Grid Layout\\ApointmentReport.xml"))
                grdApointment.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\ApointmentReport.xml");
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            SqlClass sql = new SqlClass();
            sql.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);

            sql.OpenConnection();

            string ToDay = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(PersianDate.Today.ToString()));
            sql.Command("sp_ApointmentReportDate", CommandType.StoredProcedure,ToDay);

            DataTable dt = sql.ExecuteQuery();

            apointmentDataTable = dt;

            sql.CloseConnection();

            worker.ReportProgress(100);
        }

        void LoadFollowData()
        {
            followWorker = new BackgroundWorker();

            followWorker.DoWork += FollowWorker_DoWork;
            followWorker.ProgressChanged += FollowWorker_ProgressChanged;
            followWorker.RunWorkerCompleted += FollowWorker_RunWorkerCompleted;

            followWorker.WorkerReportsProgress = true;
            followWorker.RunWorkerAsync();

            grdFollow.ShowLoadingPanel = true;
        }

        private void FollowWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            grdFollow.ShowLoadingPanel = false;
        }

        private void FollowWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            grdFollow.ItemsSource = followDataTable;

            if (File.Exists(clsDateClass.appStartupPath + "\\Grid Layout\\FollowUpReport.xml"))
                grdFollow.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\FollowUpReport.xml");
        }

        private void FollowWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SqlClass sql = new SqlClass();
            sql.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);
            sql.OpenConnection();
            string ToDay = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(PersianDate.Today.ToString()));
            sql.Command("sp_FollowUpReportDate", CommandType.StoredProcedure, ToDay);

            DataTable dt = sql.ExecuteQuery();

            followDataTable = dt;
            sql.CloseConnection();
            followWorker.ReportProgress(100);
        }

        void LoadVacationData()
        {
            vacationWorker = new BackgroundWorker();

            vacationWorker.DoWork += VacationWorker_DoWork ;
            vacationWorker.ProgressChanged += VacationWorker_ProgressChanged;
            vacationWorker.RunWorkerCompleted += VacationWorker_RunWorkerCompleted ;

            vacationWorker.WorkerReportsProgress = true;
            vacationWorker.RunWorkerAsync();

            grdVacation.ShowLoadingPanel = true;
        }

        private void VacationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            grdVacation.ShowLoadingPanel = false;
        }

        private void VacationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            grdVacation.ItemsSource = vacationDataTable;

            if (File.Exists(clsDateClass.appStartupPath + "\\Grid Layout\\VacationReport.xml"))
                grdVacation.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\VacationReport.xml");
        }

        private void VacationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SqlClass sql = new SqlClass();
            sql.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);
            sql.OpenConnection();
            string ToDay = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(PersianDate.Today.ToString()));
            sql.Command("sp_VacationsDate", CommandType.StoredProcedure,ToDay);
            DataTable dt = sql.ExecuteQuery();

            vacationDataTable = dt;
            sql.CloseConnection();
            vacationWorker.ReportProgress(100);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tabApointment.Visibility = (AppSettingClass.setting.ShowApointmentInStart) ? Visibility.Visible : Visibility.Collapsed;
            tabFollow.Visibility = (AppSettingClass.setting.ShowFollowInStart) ? Visibility.Visible : Visibility.Collapsed;
            tabRest.Visibility = (AppSettingClass.setting.ShowRestInStart) ? Visibility.Visible : Visibility.Collapsed;

            try
            {
                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));
            }
            catch (Exception)
            {

            }

            LoadApointmentData();
            LoadFollowData();
            LoadVacationData();

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if(AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting(section, "Width", this.ActualWidth);
                ConfigClass.setSetting(section, "Height", this.ActualHeight);

                ConfigClass.setSetting(section, "LocationX", this.Left);
                ConfigClass.setSetting(section, "LocationY", this.Top);
            }
        }
    }
}
