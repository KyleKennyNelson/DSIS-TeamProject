using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
    /// Interaction logic for TTSinhVienUserControl.xaml
    /// </summary>
    public partial class TTSinhVienUserControl : UserControl
    {
        OracleConnection Conn;
        SinhVien? _TTSinhVien;
        public TTSinhVienUserControl(OracleConnection connection,string masv)
        {
            InitializeComponent();
            Conn = connection;
            _TTSinhVien = new SinhVien() { MASV = masv };
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            loadingBar.IsIndeterminate = false;
            loadingBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingBar.Value = 40;
            if(_TTSinhVien != null && _TTSinhVien.MASV != null && _TTSinhVien.SDT != null && _TTSinhVien.DiaChi != null)
            {
                await Task.Run(() => Controler_SinhVien.UpdateSinhVien(Conn, _TTSinhVien.MASV, _TTSinhVien.SDT, _TTSinhVien.DiaChi));
            }
            await Task.Run(() => Thread.Sleep(25));
            loadingBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thành công", MessageBoxButton.OK);
            loadingBar.IsIndeterminate = true;
            SaveButton.IsEnabled = true;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            loadingBar.IsIndeterminate = false;
            loadingBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingBar.Value = 40;

            if(_TTSinhVien != null && _TTSinhVien.MASV != null)
            {
                SinhVien? data = null;
                await Task.Run(() => data = Controler_SinhVien.GetSinhVien(Conn, _TTSinhVien.MASV));
                _TTSinhVien = data;
            }
            
            DataContext = _TTSinhVien;
            await Task.Run(() => Thread.Sleep(25));
            loadingBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            loadingBar.IsIndeterminate = true;
            SaveButton.IsEnabled = true;
            
        }
    }
}
