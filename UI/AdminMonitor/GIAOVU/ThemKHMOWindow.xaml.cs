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
using System.Windows.Shapes;

namespace AdminMonitor.GIAOVU
{
    /// <summary>
    /// Interaction logic for ThemXoaKHMOWindow.xaml
    /// </summary>
    public partial class ThemKHMOWindow : Window
    {
        OracleConnection Conn;
        KHMO? _TTKHMO;
        public ThemKHMOWindow(OracleConnection connection)
        {
            InitializeComponent();
            Conn = connection;
            _TTKHMO = new KHMO();
            DataContext = _TTKHMO;
            mainTitle.Content = "Thêm kế hoạch mở";
            this.Title = "Thêm kế hoạch mở";
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;

            SaveButton.IsEnabled = false;
            loadingBar.IsIndeterminate = false;
            loadingBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingBar.Value = 40;

            try
            {
                if (_TTKHMO != null)
                {
                    await Task.Run(() => Controller_KHMO.InsertKHMO(Conn, _TTKHMO));
                }
            }
            catch (Exception ex)
            {
                flag = false;
                MessageBox.Show(ex.ToString(), "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await Task.Run(() => Thread.Sleep(25));
            loadingBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            if (flag)
            {
                MessageBox.Show("Thêm kế hoạch mở thành công!", "Thành công", MessageBoxButton.OK);
                loadingBar.IsIndeterminate = true;
                SaveButton.IsEnabled = true;
                DialogResult = true;
            }
        }

        private void CancelButotn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
