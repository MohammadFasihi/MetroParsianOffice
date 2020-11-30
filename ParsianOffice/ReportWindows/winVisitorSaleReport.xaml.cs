using Arash;
using BL;
using DA;
using ParsianOffice.Classes;
using ParsianOffice.Windows;
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

namespace ParsianOffice.ReportWindows
{
    /// <summary>
    /// Interaction logic for winVisitorSaleReport.xaml
    /// </summary>
    public partial class winVisitorSaleReport : Window
    {
        public winVisitorSaleReport()
        {
            InitializeComponent();
        }

        string section = "FollowUpReport";
        public winMain main;
        DataTable dataTable;
        BackgroundWorker worker = new BackgroundWorker();
        void LoadData()
        {

            try
            {
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

                worker.WorkerReportsProgress = true;
                worker.RunWorkerAsync();

                grd.ShowLoadingPanel = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            grd.ItemsSource = dataTable;

            if (File.Exists(clsDateClass.appStartupPath + "\\Grid Layout\\VisitorSaleReport.xml"))
                grd.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\VisitorSaleReport.xml");
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                SqlClass ado = new SqlClass();
                ado.ConnectionString = ConnectionStringClass.GetADOConnectionString();
                ado.OpenConnection();

                if (BLUserClass.Kind == "Admin")
                    ado.Command("sp_VisitorSale", CommandType.StoredProcedure);
                else
                    ado.Command("sp_OwnVisitorSale", CommandType.StoredProcedure, BLUserClass.VisitorID, "@vt_ID");

                DataTable dt = ado.ExecuteQuery();

                dataTable = dt;

                worker.ReportProgress(100);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                mainGrid.ColumnDefinitions[0].Width = new GridLength(double.Parse(ConfigClass.getSetting(section, "GridProperties")));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));

            }
            catch (Exception)
            {

            }

            LoadData();

            viw.MoveLastRow();

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

                grd.SaveLayoutToXml(clsDateClass.appStartupPath + "\\Grid Layout\\VisitorSaleReport.xml");
            }
        }

        private void itmSetting_Click(object sender, RoutedEventArgs e)
        {
            winGridColumnSetting setting = new winGridColumnSetting();
            setting.grd = grd;
            setting.ShowDialog();
        }
    }
}
