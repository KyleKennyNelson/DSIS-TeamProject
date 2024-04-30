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
using AdminMonitor.SINHVIEN;

namespace AdminMonitor.GIAOVU
{
    /// <summary>
    /// Interaction logic for GiaoVuMainWindow.xaml
    /// </summary>
    public partial class GiaoVuMainWindow : Window
    {
        OracleConnection Conn;
        public GiaoVuMainWindow(OracleConnection connection, string MaNV)
        {
            InitializeComponent();
            Conn = connection;

            var tabItems = new ObservableCollection<TabItem>() {
                new TabItem(){Header = "Thông tin cá nhân", Content = new TTCaNhanNVUserControl(Conn,MaNV)},
                new TabItem(){Header = "DS sinh viên", Content = new DSSinhVienUserControl(Conn)},
                new TabItem(){Header = "DS đơn vị", Content = new DSDonViUserControl(Conn)},
                new TabItem(){Header = "DS học phần", Content = new DSHocPhanUserControl(Conn)},
                new TabItem(){Header = "DS kế hoạch mở môn", Content = new GIAOVUDSKHMOUserControl(Conn)},
                new TabItem(){Header = "DS phân công", Content = new GiaoVu_DSPhanCongUserControl(Conn)},
                new TabItem(){Header = "DS đăng kí", Content = new ManHinhDangKi(Conn)},
                new TabItem(){Header = "Thông báo", Content = new ManHinhThongBaoUserControl(Conn)},
            };

            MainTabControl.ItemsSource = tabItems;
        }
    }
}
