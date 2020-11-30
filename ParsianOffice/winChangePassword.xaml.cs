using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for winChangePassword.xaml
    /// </summary>
    public partial class winChangePassword : Window
    {
        public winChangePassword()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLUserClass user = new BLUserClass();
                user.ChangePassword(lblUsername.Text, txtCurrPassword.Password, txtNewPassword.Password);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblUsername.Text = BLUserClass.Username;
            lblKind.Text = (BLUserClass.Kind == "Admin") ? "مدیر" : "کاربر عادی";
        }
    }
}
