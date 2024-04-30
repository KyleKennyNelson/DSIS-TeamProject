using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Windows;

namespace AdminMonitor.NVCOBAN
{
    /// <summary>
    /// Interaction logic for EditPhoneNumber.xaml
    /// </summary>
    public partial class EditPhoneNumber : Window
    {
        string _username = string.Empty;
        OracleConnection _conn;
        public EditPhoneNumber(OracleConnection con, string username)
        {
            InitializeComponent();
            _conn = con;
            _username = username;
            UsernameTextBlock.Text = _username;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand query = _conn.CreateCommand();
            query.CommandText = """
                                    UPDATE admin.UV_SDT_NHANSU
                                    SET DT = phonenumber
                                """;

            query.CommandType = CommandType.Text;

            string phonenumber = NewPhoneNumberBox.Text;
            query.Parameters.Add(new OracleParameter("phonenumber", phonenumber));

            try
            {
                query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Changed phone number of {_username}", "Success", MessageBoxButton.OK);
            DialogResult = true;
        }
    }
}
