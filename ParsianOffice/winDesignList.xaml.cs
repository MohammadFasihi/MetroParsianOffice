using DA;
using ParsianOffice.Windows;
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

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winDesignList.xaml
    /// </summary>
    public partial class winDesignList : Window
    {
        public winDesignList()
        {
            InitializeComponent();
        }

        bool IsClosed = true;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DA.Entities db = new DA.Entities();

            lst.ItemsSource = db.tbl_PrintDesigns.ToList();
            lst.DisplayMemberPath = "design_Name";
            lst.SelectedValuePath = "ID";
        }

        private void lst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            winContract.designID = int.Parse(lst.SelectedValue.ToString());
            IsClosed = false;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(IsClosed)
            {
                PrintSettingClass printSetting = new PrintSettingClass();
                winContract.designID = printSetting.GetDefaultDesign(BLUserClass.UserID).Value;
            }
        }
    }
}
