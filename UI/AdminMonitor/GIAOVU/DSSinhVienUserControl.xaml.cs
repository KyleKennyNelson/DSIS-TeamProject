using AdminMonitor.SINHVIEN;
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
    /// Interaction logic for DSSinhVienUserControl.xaml
    /// </summary>
    public partial class DSSinhVienUserControl : UserControl
    {
        OracleConnection Conn;
        List<SinhVien>? list = null;
        public DSSinhVienUserControl(OracleConnection connection)
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

            List<SinhVien>? data = null;
            await Task.Run(() => data = Controller_SinhVien.GetAllSinhVien(Conn));
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
            SinhVien? selected = (SinhVien?)MainDataGrid.SelectedItem;
            if (selected != null)
            {
                var screen = new ThemSuaTTSinhVien(Conn, selected);
                screen.ShowDialog();
                UserControl_Loaded(sender, e);
            }
        }

        private void ThemButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new ThemSuaTTSinhVien(Conn);
            screen.ShowDialog();
            UserControl_Loaded(sender, e);
        }
    }
}
