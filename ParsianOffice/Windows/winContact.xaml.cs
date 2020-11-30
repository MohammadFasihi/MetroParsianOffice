using BL;
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

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winContact.xaml
    /// </summary>
    public partial class winContact : Window
    {
        public winContact()
        {
            InitializeComponent();
        }

        public string tableName;
        public int ID;

        public void HeaderText()
        {
            grdMain.Columns[2].Header = "نوع فیلد";
            grdMain.Columns[3].Header = "اطلاعات";

            grdMain.Columns["ID"].IsVisible = false;
            grdMain.Columns[1].IsVisible = false;
            grdMain.Columns[4].IsVisible = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (tableName)
            {
                case "Visitor":
                    BLVisitor visitor = new BLVisitor();
                    grdMain.ItemsSource = visitor.getContact(ID);
                    HeaderText();
                    break;
                case "Customer":
                    BLCustomer customer = new BLCustomer();
                    grdMain.ItemsSource = customer.getContact(ID);
                    HeaderText();
                    break;
                case "Expert":
                    BLExpert expert = new BLExpert();
                    grdMain.ItemsSource = expert.getContact(ID);
                    HeaderText();
                    break;
            }

            grdMain.Items.MoveCurrentToFirst();

            if (grdMain.Items.Count != 0)
                grdMain.SelectedItem = grdMain.Items[0];
        }
    }
}
