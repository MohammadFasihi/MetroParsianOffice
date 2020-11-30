using System.Collections.Generic;
using System.Windows;
using BL;
using System.ComponentModel;
using System.IO;

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winAlert.xaml
    /// </summary>
    public partial class winAlert : Window
    {
        public winAlert()
        {
            InitializeComponent();
        }

        public string ap_Date;
        public string expert;

        BackgroundWorker worker;
        List<DA.viw_ApointmentReport> lst;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            worker = new BackgroundWorker();

            grdMain.ShowLoadingPanel = true;

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();

        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            grdMain.ItemsSource = lst;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            grdMain.ShowLoadingPanel = false;
            if(File.Exists(clsDateClass.appStartupPath + "\\Grid Layout\\Alert.xml"))
                grdMain.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\Alert.xml");
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BLApointment apointment = new BLApointment();
            lst = new List<DA.viw_ApointmentReport>();
            lst = apointment.getApointment(expert, ap_Date);

            worker.ReportProgress(100);
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            grdMain.SaveLayoutToXml(clsDateClass.appStartupPath + "\\Grid Layout\\Alert.xml");
        }
    }
}
