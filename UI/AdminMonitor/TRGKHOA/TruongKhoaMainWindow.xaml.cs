using AdminMonitor.SINHVIEN;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AdminMonitor.TRGKHOA
{
    /// <summary>
    /// Interaction logic for TruongKhoaMainWindow.xaml
    /// </summary>
    public partial class TruongKhoaMainWindow : Window
    {
        OracleConnection Conn;
        public TruongKhoaMainWindow(OracleConnection connection, string MaTrgKhoa)
        {
            InitializeComponent();
            Conn = connection;

            var tabItems = new ObservableCollection<TabItem>() {
                new TabItem(){Header = "Thông tin SV"},
                new TabItem(){Header = "DS HP đăng kí"},
                new TabItem(){Header = "DS Học phần"},
                new TabItem(){Header = "Kế hoạch mở môn"},
                new TabItem(){Header = "Thông báo"},
            };

            MainTabControl.ItemsSource = tabItems;
        }
    }
}
