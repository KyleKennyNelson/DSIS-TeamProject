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
    /// Interaction logic for XemDSKHMOUserControl.xaml
    /// </summary>
    public partial class XemDSKHMOUserControl : UserControl
    {
        OracleConnection Conn;
        public XemDSKHMOUserControl(OracleConnection connection)
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

            List<KHMO>? data = null;
            await Task.Run(() => {
                data = Controller_KHMO.GetKHMO(Conn);
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
    public class KHMO
    {
        public string? MaHP { get; set; }
        public int? HK { get; set; }
        public int? Nam { get; set; }
        public string? MaCT { get; set; }
    }
    public class Controller_KHMO
    {
        public static List<KHMO> GetKHMO(OracleConnection Conn)
        {
            var result = new List<KHMO>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    select * from ADMIN.PROJECT_KHMO
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {
                   
                    var newKHMO = new KHMO()
                    {
                        MaHP = (string)row["MAHP"],
                        HK = (int)(decimal)row["HK"],
                        Nam = (int)(decimal)row["NAM"],
                        MaCT = (string)row["MACT"]
                    };
                    result.Add(newKHMO);
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
