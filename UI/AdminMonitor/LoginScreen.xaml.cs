using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AdminMonitor
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
        public OracleConnection _connection;

        List<string> privileges = new List<string>() { 
            "","SYSDBA","SYSOPER"
        };
        public string _role;
        void Encrypt(string password, string username,string server, string privilege)
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["username"].Value = username;
            config.AppSettings.Settings["DABPRIVILEGE"].Value = (string)DBAPrivilegeComboBox.SelectedItem;
            config.AppSettings.Settings["DATASOURCE"].Value = DataSourceTextBox.Text;
            config.AppSettings.Settings["password"].Value = Convert.ToBase64String(cypherText);
            config.AppSettings.Settings["entropy"].Value = Convert.ToBase64String(entropy);
            config.AppSettings.Settings["isPasswordRemmembered"].Value = "1";
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string password = PasswordBox.Password;
            string username = UsernameTextBox.Text;
            string server = DataSourceTextBox.Text;
            string privilege = DBAPrivilegeComboBox.Text;

            string conStr = $"""
                                    DATA SOURCE={server};
                                    DBA PRIVILEGE={privilege};
                                    PERSIST SECURITY INFO=True;
                                    USER ID={username};
                                    PASSWORD={password}
                                    """;
            
            try
            {
                _connection = new OracleConnection(conStr);
                _connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (_connection.State == ConnectionState.Open)
            {
                MessageBox.Show( "Logged in successfully!", "Success", MessageBoxButton.OK);

                if (username == "sys" || username.ToUpper() == "SYS")
                {
                    _role = "DBA";
                }
                else
                {
                    //_role = "NVCOBAN";
                    var con = _connection;
                    OracleCommand query = con.CreateCommand();

                    query.CommandText = $"""
                                            select GRANTED_ROLE 
                                            from USER_ROLE_PRIVS 
                                            where USERNAME='{username}'
                                                and GRANTED_ROLE != 'CONNECT'
                                         """;

                    query.CommandType = CommandType.Text;
                    OracleDataReader dr = query.ExecuteReader();

                    try
                    {
                        while (dr.Read())
                        {
                            string TablegrantedRole = (string)dr["GRANTED_ROLE"];
                            _role = TablegrantedRole;
                        }
                    }
                    finally
                    {
                        dr.Close();
                    }

                    //var grantedRole = new DataTable();
                    //grantedRole.Load(datareader);
                    //_role = grantedRole.Rows[0]["GRANTED_ROLE"].ToString();
                }

                if (RemembermeCheckBox.IsChecked == true)
                {
                    Encrypt(password, username,server,privilege);
                }
                else
                {
                    config.AppSettings.Settings["isPasswordRemmembered"].Value = "0";
                    config.AppSettings.Settings["username"].Value = " ";
                    config.AppSettings.Settings["password"].Value = " ";
                    config.AppSettings.Settings["entropy"].Value = " ";
                    config.AppSettings.Settings["DABPRIVILEGE"].Value = (string)DBAPrivilegeComboBox.SelectedItem;
                    config.AppSettings.Settings["DATASOURCE"].Value = DataSourceTextBox.Text;
                    config.Save(ConfigurationSaveMode.Minimal);

                }
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Wrong credential!", "Log in failed", MessageBoxButton.OK);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["isPasswordRemmembered"].Value == "1")
            {
                UsernameTextBox.Text = config.AppSettings.Settings["username"].Value;
                var cypherText = Convert.FromBase64String(ConfigurationManager.AppSettings["password"]);
                var entropy = Convert.FromBase64String(ConfigurationManager.AppSettings["entropy"]);
                var decryptedPassword = ProtectedData.Unprotect(cypherText, entropy, DataProtectionScope.CurrentUser);
                var realPassword = Encoding.UTF8.GetString(decryptedPassword);


                PasswordBox.Password = realPassword;
                RemembermeCheckBox.IsChecked = true;
            }

            DataSourceTextBox.Text = config.AppSettings.Settings["DATASOURCE"].Value;
            DBAPrivilegeComboBox.ItemsSource = privileges;
            DBAPrivilegeComboBox.SelectedItem = privileges.Single(x => x == config.AppSettings.Settings["DABPRIVILEGE"].Value);
        }
    }
}
