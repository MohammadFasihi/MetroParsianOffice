using DA;
using ParsianOffice.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for winAddContactKind.xaml
    /// </summary>
    public partial class winAddContactKind : Window
    {
        public winAddContactKind()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Entities db = new Entities();

            grdMain.ItemsSource = db.tbl_ContactKind.ToList();
        }


        private void grdMain_Deleting(object sender, Telerik.Windows.Controls.GridViewDeletingEventArgs e)
        {
            Entities db = new Entities();
            tbl_ContactKind kind = (tbl_ContactKind)e.DataControl.SelectedItem;
            var query = (from q in db.tbl_ContactKind where q.ID == kind.ID select q).Single();
            db.tbl_ContactKind.Remove(query);
            db.SaveChanges();
        }

        private void grdMain_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            Entities db = new Entities();
            tbl_ContactKind kind = (tbl_ContactKind)grdMain.SelectedItem;

            if (kind == null)
            {
                kind = (tbl_ContactKind)e.NewData;
                db.tbl_ContactKind.Add(kind);
            }
            else
            {
                var query = (from q in db.tbl_ContactKind where q.ID == kind.ID select q);

                query.Single().KindName = kind.KindName;

            }


            db.SaveChanges();
        }
    }
}
