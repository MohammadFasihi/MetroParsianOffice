using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winCancelReson.xaml
    /// </summary>
    public partial class winCancelReson : Window
    {
        public winCancelReson()
        {
            InitializeComponent();
        }

        bool isFill = false;

        public int ap_ID;
        string side = "";
        private void Checked_Changed(object sender, RoutedEventArgs e)
        {
            RadioButton rad = (RadioButton)sender;
            side = rad.Content.ToString();
        }

        private async void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DA.Entities db = new DA.Entities();
            DA.tbl_Reason res = new DA.tbl_Reason();

            isFill = ((!String.IsNullOrEmpty(txtText.Text) || String.IsNullOrWhiteSpace(txtText.Text)) && (radCompany.IsChecked.Value || radCustomer.IsChecked.Value || radExpert.IsChecked.Value));

            if (isFill)
            {
                res.ap_ID = ap_ID;
                res.res_Text = txtText.Text;
                res.res_Side = side;

                db.tbl_Reason.Add(res);

                db.SaveChanges();
                this.DialogResult = isFill;
                Close();
            }
            else
            {
                MessageBox.Show("طرف لغو کننده و علت لغو را باید وارد کنید", "خطا در سیستم", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = isFill;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = isFill;
            Close();
        }
    }
}
