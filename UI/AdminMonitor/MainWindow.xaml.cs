using AdminMonitor.NVCOBAN;
using AdminMonitor.SINHVIEN;
using System.Windows;


namespace AdminMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginScreen loginScreen = new();
            this.Hide();
            var result = loginScreen.ShowDialog();
            if (result == false) { 
                Close();
            }
            else
            {
                var con = loginScreen._connection;
                var role = loginScreen._role;
                var username = loginScreen.username;
                Window? window = null;
                if (con != null)
                {
                    switch (role)
                    {
                        case "SYSDBA":
                            window = new AdminScreen(con);
                            break;
                        case "NVCOBAN":
                            window = new NVCoBanScreen(con);
                            break;
                        case "GIANGVIEN":
                            break;
                        case "GIAOVU":
                            break;
                        case "TRGKHOA":
                            break;
                        case "TRGDONVI":
                            break;
                        case "SINHVIEN":
                            if(username != null)
                            {
                                window = new ManHinhSinhVien(con, username);
                            }
                            break;
                        default: break;
                    }
                    
                    window?.ShowDialog();
                    Close();
                }
            }
        }
    }
}