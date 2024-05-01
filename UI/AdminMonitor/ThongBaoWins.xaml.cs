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

namespace AdminMonitor
{
    /// <summary>
    /// Interaction logic for ThongBaoWins.xaml
    /// </summary>
    public partial class ThongBaoWins : Window
    {
        public ThongBaoWins(OracleConnection connection)
        {
            InitializeComponent();
            this.Content = new ManHinhThongBaoUserControl(connection);
        }
    }
}
