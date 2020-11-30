using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Forms;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winBackup.xaml
    /// </summary>
    public partial class winBackup : Window
    {
        public winBackup()
        {
            InitializeComponent();
        }

        string databaseName, serverName;
        public winMain main;
        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString(120));
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "BACKUP DATABASE " + databaseName + " TO DISK = N'" + txtFileAddress.Text + txtFileName.Text + ".bak'";
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                System.Windows.MessageBox.Show("Backup opration has been done.", "Successfuly", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Command Error : " + ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();

            txtFileAddress.Text = fbd.SelectedPath + "\\";

            if (txtFileAddress.Text != "")
                btnBackup.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                serverName = ConnectionStringClass.ServerName + "\\" + ConnectionStringClass.InctancName;
                databaseName = ConnectionStringClass.DatabaseName;

                string fileName = DateTime.Now.Year.ToString("0000") + "-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Day.ToString("00") +
                    "_" + DateTime.Now.Hour.ToString("00") + "-" + DateTime.Now.Minute.ToString("00") + "-" + DateTime.Now.Second.ToString("00");

                txtFileName.Text = fileName;

                txtFileAddress.Focus();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Command Error : " + ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
