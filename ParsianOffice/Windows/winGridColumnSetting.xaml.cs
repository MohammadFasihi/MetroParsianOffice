using DevExpress.Xpf.Grid;
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
    /// Interaction logic for winGridColumnSetting.xaml
    /// </summary>
    public partial class winGridColumnSetting : Window
    {
        public winGridColumnSetting()
        {
            InitializeComponent();
        }

        public GridControl grd;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstColumnName.ItemsSource = grd.Columns;
            lstColumnName.DisplayMemberPath = "Header";
            lstColumnName.SelectedValuePath = "FieldName";
        }

        private void btnAddColumn_Click(object sender, RoutedEventArgs e)
        {
            GridColumn column = new GridColumn();
            column.Header = "NewColumn";
            column.FieldName = "NewColumn";
            column.AutoFilterCriteria = DevExpress.Data.Filtering.Helpers.ClauseType.Contains;
            grd.Columns.Add(column);
        }

        private void btnDelColumn_Click(object sender, RoutedEventArgs e)
        {
            string columnName = lstColumnName.SelectedValue.ToString();
            GridColumn column = grd.Columns[columnName];
            grd.Columns.Remove(column);
        }

        private void lstColumnName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string columnName = lstColumnName.SelectedValue.ToString();
            GridColumn column = grd.Columns[columnName];
            propColumn.SelectedObject = column;
        }
    }
}
