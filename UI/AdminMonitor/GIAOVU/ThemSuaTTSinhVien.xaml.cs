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
    /// Interaction logic for ThemSuaTTSinhVien.xaml
    /// </summary>
    public partial class ThemSuaTTSinhVien : Window
    {
        class Modes
        {
            public static int Insert => 0;
            public static int Update => 1;
        }

        OracleConnection Conn;
        SinhVien? _TTSinhVien;
        int mode = Modes.Insert;
        public ThemSuaTTSinhVien(OracleConnection connection, SinhVien data)
        {
            InitializeComponent();
            Conn = connection;
            _TTSinhVien = data;
            mode = Modes.Update;
            DataContext = _TTSinhVien;
            mainTitle.Content = "Chỉnh sửa thông tin sinh viên:";
            this.Title = "Chỉnh sửa thông tin sinh viên";
            DiemTBTextBox.Text = _TTSinhVien.DTBTL.ToString();
            SoTinChiTextBox.Text = _TTSinhVien.SOTCTL.ToString();
            MASVTextBox.IsEnabled = false;
        }
        public ThemSuaTTSinhVien(OracleConnection connection)
        {
            InitializeComponent();
            Conn = connection;
            mode = Modes.Insert;
            _TTSinhVien = new SinhVien() { 
                MASV = "SV000"
            };
            DataContext = _TTSinhVien;
            mainTitle.Content = "Thêm sinh viên:";
            this.Title = "Thêm sinh viên";
            DiemTBTextBox.IsEnabled = false;
            SoTinChiTextBox.IsEnabled = false;
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
                if (_TTSinhVien != null)
                {
                    _TTSinhVien.NgaySinh = DateTime.Parse(NgaySinhDatePicker.Text);
                    _TTSinhVien.SOTCTL = int.Parse(SoTinChiTextBox.Text);
                    _TTSinhVien.DTBTL = double.Parse(DiemTBTextBox.Text);
                    if (mode == Modes.Update)
                    {
                        await Task.Run(() => Controller_SinhVien.UpdateSinhVien(Conn, _TTSinhVien));
                    }
                    else if (mode == Modes.Insert)
                    {
                        await Task.Run(() => Controller_SinhVien.InsertSinhVien(Conn, _TTSinhVien));
                    }
                }
            }catch(Exception ex)
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
                MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thành công", MessageBoxButton.OK);
            }
            loadingBar.IsIndeterminate = true;
            SaveButton.IsEnabled = true;

            DialogResult = true;
        }

        private void CancelButotn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
