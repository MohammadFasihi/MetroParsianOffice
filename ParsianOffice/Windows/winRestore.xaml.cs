using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winRestore.xaml
    /// </summary>
    public partial class winRestore : Window
    {
        public winRestore()
        {
            InitializeComponent();
        }

        public winMain main;
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Backup Files(*.bak)|*.bak";
            ofd.ShowDialog();

            txtFileAddress.Text = ofd.FileName;

            if (!File.Exists(txtFileAddress.Text))
            {
                btnRestore.IsEnabled = false;
            }
            else
            {
                DateTime createdDate = File.GetCreationTime(txtFileAddress.Text);
                DateTime lastAccess = File.GetLastAccessTime(txtFileAddress.Text);

                lblFileName.Text = ofd.SafeFileName;

                lblCreatedDate.Text = "تاریخ میلادی ایجادی فایل پشتیبان :  " + createdDate.ToShortDateString() +
                    "\n" + "تاریخ شمسی ایجاد فایل پشتیبان : " + clsDateClass.PersianDate(createdDate) +
                    "\n" + "تاریخ میلادی آخرین دسترسی به فایل : " + lastAccess.ToShortDateString() +
                    "\n" + "تاریخ شمسی آخرین دسترسی به فایل : " + clsDateClass.PersianDate(lastAccess);

                btnRestore.IsEnabled = true;
            }

            
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString(120));
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "RESTORE DATABASE " + ConnectionStringClass.DatabaseName + " FROM DISK = N'" + txtFileAddress.Text + "' with replace";
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                System.Windows.MessageBox.Show("Restore opration has been done.", "Successfuly", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
