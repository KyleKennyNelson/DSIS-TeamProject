using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AdminMonitor.SINHVIEN
{
    /// <summary>
    /// Interaction logic for ManHinhSinhVien.xaml
    /// </summary>
    public partial class ManHinhSinhVien : Window
    {
        OracleConnection Conn;
        public ManHinhSinhVien(OracleConnection connection, string MaSV)
        {
            InitializeComponent();
            Conn = connection;

            var tabItems = new ObservableCollection<TabItem>() { 
                new TabItem(){Header = "Thông tin SV", Content = new TTSinhVienUserControl(Conn,MaSV) },
                new TabItem(){Header = "DS HP đăng kí"},
                new TabItem(){Header = "DS Học phần"},
                new TabItem(){Header = "Kế hoạch mở môn"},
            };

            MainTabControl.ItemsSource = tabItems;
        }
    }
}
