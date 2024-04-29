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
    /// Interaction logic for XemDSHocPhanUserControl.xaml
    /// </summary>
    public partial class XemDSHocPhanUserControl : UserControl
    {
        OracleConnection Conn;
        public XemDSHocPhanUserControl(OracleConnection connection)
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

            List<HocPhan>? hocPhans = null;
            await Task.Run(() => {
                hocPhans = Controller_HocPhan.GetHocPhans(Conn);
            });
            MainDataGrid.ItemsSource = hocPhans;

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));

            loadingProgressBar.IsIndeterminate = true;
        }
    }

    public class HocPhan
    {
        public string? MaHP { get; set; }
        public string? TenHP { get; set; }
        public int? SOTC { get; set; }
        public int? STLT { get; set; }
        public int? STTH { get; set; }
        public int? SOSVTD { get; set; }
        public string? MaDV { get; set; }
    }

    public class Controller_HocPhan
    {
        public static List<HocPhan>? GetHocPhans(OracleConnection Conn)
        {
            List<HocPhan> result = new List<HocPhan>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    select * from ADMIN.PROJECT_HOCPHAN
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {
                    result.Add(new HocPhan
                    {
                        MaHP = (string)row["MAHP"],
                        TenHP = (string)row["TENHP"],
                        SOTC = (int)(decimal)row["SOTC"],
                        STLT = (int)(decimal)row["STLT"],
                        STTH = (int)(decimal)row["STTH"],
                        SOSVTD = (int)(decimal)row["SOSVTD"],
                    });
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
