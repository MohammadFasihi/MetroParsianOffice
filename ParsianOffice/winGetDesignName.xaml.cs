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

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winGetDesignName.xaml
    /// </summary>
    public partial class winGetDesignName : Window
    {
        public winGetDesignName()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtName.Text) || !String.IsNullOrWhiteSpace(txtName.Text))
            {
                winPrintingSetting.designName = txtName.Text;
                Close();
            }
            else
                MessageBox.Show("نام را وارد کنید");
        }
    }
}
