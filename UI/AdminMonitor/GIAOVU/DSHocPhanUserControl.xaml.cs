using Oracle.ManagedDataAccess.Client;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminMonitor.GIAOVU
{
    /// <summary>
    /// Interaction logic for DSHocPhanUserControl.xaml
    /// </summary>
    public partial class DSHocPhanUserControl : UserControl
    {
        OracleConnection Conn;
        List<HocPhan>? list = null;
        public DSHocPhanUserControl(OracleConnection connection)
        {
            InitializeComponent();
            Conn = connection;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ThemButton.IsEnabled = false;
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            List<HocPhan>? data = null;
            await Task.Run(() => data = Controller_HocPhan.getAllHocPhan(Conn));
            list = data;
            MainDataGrid.ItemsSource = list;

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));

            loadingProgressBar.IsIndeterminate = true;
            ThemButton.IsEnabled = true;
        }

        private void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            HocPhan? selected = (HocPhan?)MainDataGrid.SelectedItem;
            if (selected != null)
            {
                var screen = new ThemSuaHocPhanWindow(Conn, selected);
                screen.ShowDialog();
                UserControl_Loaded(sender, e);
            }
        }

        private void ThemButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new ThemSuaHocPhanWindow(Conn);
            screen.ShowDialog();
            UserControl_Loaded(sender, e);
        }
    }
}
