using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Net.NetworkInformation;
using ParsianOffice.Windows;
using System.Reflection;
using System.Diagnostics;
using DA;
using System.IO;
using BL;
using System.Security.Cryptography;
using System.Text;
using ParsianOffice.Classes;

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winSplash.xaml
    /// </summary>
    public partial class winSplash : Window
    {
        public winSplash()
        {
            InitializeComponent();
        }

        string okPic = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\download.png";

        bool IsConnected;
        winLogin userLogin = new winLogin();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(clsDateClass.appStartupPath + "\\Grid Layout"))
                Directory.CreateDirectory(clsDateClass.appStartupPath + "\\Grid Layout");

            Entities db = new Entities();

            clsLanguage.SaveLanguage();

            ConnectionStringClass.GetConnectionString();

            Assembly ver = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVer = FileVersionInfo.GetVersionInfo(ver.Location);
            lblBuild.Text = fileVer.FileVersion;

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync("تست اتصال به شبکه");
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsConnected)
            {
                userLogin.ss = this;
                userLogin.ShowDialog();
            }
            else
            {
                winConnectionError error = new winConnectionError();
                ConnectionStringClass.IsConnected = false;
                error.ShowDialog();

                if (ConnectionStringClass.IsConnected)
                {
                    BackgroundWorker worker = new BackgroundWorker();

                    worker.DoWork += Worker_DoWork;
                    worker.ProgressChanged += Worker_ProgressChanged;
                    worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

                    worker.WorkerReportsProgress = true;
                    worker.RunWorkerAsync("تست اتصال به شبکه");
                }
                else
                    Application.Current.Shutdown();
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblLoading.Text = e.UserState.ToString();
            pgbLoading.Value = e.ProgressPercentage;

            int div = e.ProgressPercentage / 20;

            Uri image = new Uri(okPic);
            BitmapImage bitImage = new BitmapImage(image);

            switch (div)
            {
                case 1:
                    img1.Source = bitImage;
                    break;

                case 2:
                    img2.Source = bitImage;
                    break;

                case 3:
                    img3.Source = bitImage;
                    break;

                case 4:
                    img4.Source = bitImage;
                    break;

                case 5:
                    img5.Source = bitImage;
                    break;
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string state = "";
                int percent = 0;

                string hostname = "";
                int timeout = 1000;

                Ping ping = new Ping();

                if (ConnectionStringClass.ServerName == "." || ConnectionStringClass.ServerName == "NULL" || ConnectionStringClass.ServerName == null)
                    hostname = "127.0.0.1";
                else
                    hostname = ConnectionStringClass.ServerName;

                percent += 20;
                state = "تست اتصال به شبکه";
                (sender as BackgroundWorker).ReportProgress(percent, state);
                System.Threading.Thread.Sleep(100);

                PingReply reply = ping.Send(hostname, timeout);

                if (reply.Status != IPStatus.Success)
                {
                    MessageBox.Show("ارتباط به سرور در شبکه برقرار نیست", "اتصال شبکه", MessageBoxButton.OK, MessageBoxImage.Error);
                    (sender as BackgroundWorker).CancelAsync();
                }

                state = "در حال اتصال به بانک اطلاعاتی......";
                (sender as BackgroundWorker).ReportProgress(percent, state);
                System.Threading.Thread.Sleep(100);
                percent += 20;

                IsConnected = ConnectionStringClass.TestConnection();

                if (!IsConnected)
                    throw new Exception();

                Entities db = new Entities();

                state = "بررسی اطلاعات بانک اطلاعاتی......";
                (sender as BackgroundWorker).ReportProgress(percent, state);
                System.Threading.Thread.Sleep(100);
                percent += 20;

                if (db.tbl_Users.Count() == 0)
                {
                    FirstBuildingClass FirstBuilding = new FirstBuildingClass();
                    FirstBuilding.Build();
                }

                state = "در حال دریافت اطلاعات......";
                (sender as BackgroundWorker).ReportProgress(percent, state);
                System.Threading.Thread.Sleep(100);
                percent += 20;

                BLUserClass.user = db.tbl_Users.Where(p => p.IsActive == true);


                state = "آماده سازی انجام شد";
                (sender as BackgroundWorker).ReportProgress(percent, state);
                System.Threading.Thread.Sleep(100);
                percent += 20;

                (sender as BackgroundWorker).ReportProgress(percent, state);

                System.Threading.Thread.Sleep(100);
            }
            catch (Exception)
            {
                IsConnected = false;
            }
        }
    }
}
