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
using System.Diagnostics;
using DA;
using System.Security.Cryptography;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winLogin.xaml
    /// </summary>
    public partial class winLogin : Window
    {
        public winLogin()
        {

            InitializeComponent();

        }
        public winSplash ss;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbUsername.ItemsSource = BLUserClass.user.ToList();
            cmbUsername.DisplayMemberPath = "username";
            cmbUsername.SelectedValuePath = "ID";
            cmbUsername.SelectedItem = null;

            txtDatabase.Text = ConnectionStringClass.DatabaseName;
            txtServer.Text = ConnectionStringClass.ServerName + "\\" + ConnectionStringClass.InctancName;

            cmbUsername.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SHA1Cng hash = new SHA1Cng();
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(txtPassword.Password));
                byte[] ResultFinal = hash.ComputeHash(result);
                string pass = Convert.ToBase64String(ResultFinal);

                BLUserClass usr = new BLUserClass();

                var q = usr.SendSearchSignal(cmbUsername.Text, pass);

                BLUserClass.UserID = q.Single().ID;
                BLUserClass.Username = q.Single().username;
                BLUserClass.Password = q.Single().password;
                BLUserClass.VisitorID = q.Single().vt_ID;
                BLUserClass.Kind = q.Single().kind;

                winMain main = new winMain();
                main.login = this;
                txtPassword.Password = "";
                cmbUsername.Focus();

                main.Show();

                ss.Close();
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "احراز هویت", MessageBoxButton.OK, MessageBoxImage.Error);

                cmbUsername.Focus();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            clsLanguage.RestoreLanguage();
            Application.Current.Shutdown();
        }

        private void cmbUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            clsLanguage.Persion();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            clsLanguage.English();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            winConnectionError conError = new winConnectionError();
            conError.ShowDialog();

            txtDatabase.Text = ConnectionStringClass.DatabaseName;
            txtServer.Text = ConnectionStringClass.ServerName + "\\" + ConnectionStringClass.InctancName;
        }
    }
}
