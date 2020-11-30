using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Data.SqlClient;
using System.IO;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winConnectionError.xaml
    /// </summary>
    public partial class winConnectionError : Window
    {
        public winConnectionError()
        {
            InitializeComponent();
        }

        public bool IsCanceled = false;

        const string errorPic = "/Pics/red-error-png-image-100919.png";
        const string okPic = "/Pics/ok.png";
        const string errorColor = "#FFE84B4B";
        const string okColor = "#FF4BE868";

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!ConnectionStringClass.IsConnected)
            {
                if (radWin.IsChecked.Value)
                {
                    ConnectionStringClass.TestConnection(txtConnectionPoint.Text, txtInstance.Text, txtDatabase.Text, "Windows Authentication", "NULL", "NULL");

                    if (ConnectionStringClass.IsConnected)
                        ConnectionStringClass.SetConnectionString("Windows Authentication",txtConnectionPoint.Text, txtInstance.Text, txtDatabase.Text, "NULL", "NULL");
                }
                else
                {
                    ConnectionStringClass.TestConnection(txtConnectionPoint.Text, txtInstance.Text, txtDatabase.Text, "SQL Authentication", txtUsername.Text, txtPassword.Password);
                    if (ConnectionStringClass.IsConnected)
                        ConnectionStringClass.SetConnectionString("SQL Authentication",txtConnectionPoint.Text, txtInstance.Text, txtDatabase.Text,  txtUsername.Text, txtPassword.Password);
                }

                Window_Loaded(null, null);
            }
            else
                Close();
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            string appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            Process proc = Process.Start(appStartPath + "\\ConfigApp.exe");
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.WaitForExit();

            ConnectionStringClass.TestConnection();

            Window_Loaded(null, null);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectionStringClass.GetConnectionString();

            if (ConnectionStringClass.IsConnected)
            {
                grdMain.Background = (Brush)(new BrushConverter().ConvertFromString(okColor));
                string str = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\ok.png";

                Uri uri = new Uri(str);
                BitmapImage image = new BitmapImage(uri);

                img.Source = image;

                lblTilte.Text = "اتصال برقرار است";

                btnConnect.Content = "تایید";
            }
            else
            {
                grdMain.Background = (Brush)(new BrushConverter().ConvertFromString(errorColor));
                string str = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\red-error-png-image-100919.png";

                Uri uri = new Uri(str);
                BitmapImage image = new BitmapImage(uri);

                img.Source = image;

                lblTilte.Text = "اتصال برقرار نیست";

                btnConnect.Content = "اتصال";
            }

            string appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            if (!File.Exists(appStartPath + "\\Connection Configs.ini"))
                ConnectionStringClass.SetConnectionString("NULL", "NULL", "NULL", "NULL", "NULL", "NULL");

            string auth = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "Auth").ToString();
            string serverName = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "serverName").ToString();
            string inctancName = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "inctancName").ToString();
            string databaseName = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "databaseName").ToString();

            if (auth == "SQL Authentication")
            {
                radSQL.IsChecked = true;

                string user = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "User").ToString();
                string pass = new AMS.Profile.Ini(appStartPath + "\\Connection Configs.ini").GetValue("Database", "Password").ToString();

                txtUsername.Text = user;
                txtPassword.Password = pass;
            }

            if (auth == "Windows Authentication")
            {
                radWin.IsChecked = true;
            }

            txtConnectionPoint.Text = (serverName == "NULL") ? "." : serverName;
            txtInstance.Text = inctancName;
            txtDatabase.Text = databaseName;

        }
    }
}
