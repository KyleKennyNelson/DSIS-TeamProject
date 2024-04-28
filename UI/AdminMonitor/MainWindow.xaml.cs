using AdminMonitor.NVCOBAN;
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
            LoginScreen loginScreen = new LoginScreen();
            this.Hide();
            var result = loginScreen.ShowDialog();
            var con = loginScreen._connection;
            var role = loginScreen._role;
            if (role == "DBA")
            {
                AdminScreen AdminScreen = new AdminScreen(con);
                if (result == true)
                {
                    AdminScreen.Show();
                }
                else
                {
                    AdminScreen.Close();
                    return;
                }
            }
            else if (role == "NVCOBAN")
            {
                NVCoBanScreen nVCoBanScreen = new NVCoBanScreen(con);
                if (result == true)
                {
                    nVCoBanScreen.Show();
                }
                else
                {
                    nVCoBanScreen.Close();
                    return;
                }
            }
            else if (role == "GIANGVIEN")
            {
                //NVCoBanScreen nVCoBanScreen = new NVCoBanScreen(con);
                //if (result == true)
                //{
                //    nVCoBanScreen.Show();
                //}
                //else
                //{
                //    nVCoBanScreen.Close();
                //    return;
                //}
            }
        }
    }
}