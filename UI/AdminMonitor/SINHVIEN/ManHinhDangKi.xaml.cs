using FastMember;
using Oracle.ManagedDataAccess.Client;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminMonitor.SINHVIEN
{
    /// <summary>
    /// Interaction logic for ManHinhDangKi.xaml
    /// </summary>
    public partial class ManHinhDangKi : UserControl
    {
        SinhVien? TTSinhVien;
        OracleConnection Conn;
        public ManHinhDangKi(OracleConnection connection, string masv)
        {
            InitializeComponent();
            Conn = connection;
            TTSinhVien = new SinhVien() { MASV  = masv };
        }
        public ManHinhDangKi(OracleConnection connection)
        {
            InitializeComponent();
            Conn = connection;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            List<DangKi>? data = null;
            await Task.Run(() => data = Controller_DangKi.GetDangKi(Conn));
            DangKiDataGrid.ItemsSource = data;

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.IsIndeterminate = true;
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            var item = (DangKi?)DangKiDataGrid.SelectedItems[0];
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            if (item != null && item.MaSV != null && item.MaHP != null && item.HK != null && item.Nam != null && item.MaCT != null)
            {
                result = Controller_DangKi.DeleteDangKi(Conn, item.MaSV, item.MaHP, (int)item.HK, (int)item.Nam, item.MaCT);
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin đăng kí!", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.IsIndeterminate = true;
            if(result)
            {
                MessageBox.Show($"Xoá thông tin đăng kí thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Không thể xoá thông tin đăng kí!", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            List<DangKi>? data = null;
            await Task.Run(() => data = Controller_DangKi.GetDangKi(Conn));
            DangKiDataGrid.ItemsSource = data;

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.IsIndeterminate = true;
        }

        private void DangKiButton_Click(object sender, RoutedEventArgs e)
        {
            if(TTSinhVien != null && TTSinhVien.MASV != null)
            {
                var screen = new ThemDangKiWindow(Conn, TTSinhVien.MASV);
                screen.ShowDialog();
                UserControl_Loaded(sender, e);
            }
            else
            {
                var screen = new ThemDangKiWindow(Conn);
                screen.ShowDialog();
                UserControl_Loaded(sender, e);
            }
        }
    }
}
