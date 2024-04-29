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

namespace AdminMonitor
{
    /// <summary>
    /// Interaction logic for ManHinhThongBaoUserControl.xaml
    /// </summary>
    public partial class ManHinhThongBaoUserControl : UserControl
    {
        OracleConnection Conn;
        public ManHinhThongBaoUserControl(OracleConnection connection)
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

            List<ThongBao>? data = null;
            await Task.Run(() => {
                data = Controller_ThongBao.GetThongBao(Conn);
            });
            MainDataGrid.ItemsSource = data;

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));

            loadingProgressBar.IsIndeterminate = true;
        }
    }
    public class ThongBao
    {
        public string? MaTB { get; set; }
        public string? NoiDung { get; set; }
    }

    public class Controller_ThongBao
    {
        public static List<ThongBao> GetThongBao(OracleConnection Conn)
        {
            var result = new List<ThongBao>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    select * from ADMIN.PROJECT_THONGBAO
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {

                    var newThongBao = new ThongBao()
                    {
                        MaTB = (string)row["ID_TB"],
                        NoiDung = (string)row["NOIDUNG"]
                    };
                    result.Add(newThongBao);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return result;
        }
    }
}
