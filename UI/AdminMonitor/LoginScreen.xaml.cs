using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AdminMonitor
{
    internal class Role
    {
        public required string RoleName { get; set; }
        public required string ConnectAs { get; set; }
    }
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
        public OracleConnection? _connection;

        readonly List<Role> roleOptions = [
            new Role(){ RoleName = "NVCOBAN", ConnectAs = ""},
            new Role(){ RoleName = "GIAOVU", ConnectAs = ""},
            new Role(){ RoleName = "GIANGVIEN", ConnectAs = ""},
            new Role(){ RoleName = "TRGDONVI", ConnectAs = ""},
            new Role(){ RoleName = "TRGKHOA", ConnectAs = ""},
            new Role(){ RoleName = "SINHVIEN", ConnectAs = ""},
            new Role(){ RoleName = "SYSDBA", ConnectAs = "SYSDBA"},
            new Role(){ RoleName = "SYSOPER", ConnectAs = "SYSOPER"},
        ];

        public string? _role;
        void Encrypt(string password, string username)
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }

            var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var selectedRole = (Role)DBAPrivilegeComboBox.SelectedItem;

            config.AppSettings.Settings["username"].Value = username;
            config.AppSettings.Settings["DABPRIVILEGE"].Value = selectedRole.RoleName;
            config.AppSettings.Settings["DATASOURCE"].Value = DataSourceTextBox.Text;
            config.AppSettings.Settings["password"].Value = Convert.ToBase64String(cypherText);
            config.AppSettings.Settings["entropy"].Value = Convert.ToBase64String(entropy);
            config.AppSettings.Settings["isPasswordRemmembered"].Value = "1";

            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRole = (Role)DBAPrivilegeComboBox.SelectedItem;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string password = PasswordBox.Password;
            string username = UsernameTextBox.Text;
            string server = DataSourceTextBox.Text;
            string privilege = selectedRole.ConnectAs;

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

            if (_connection?.State == ConnectionState.Open)
            {
                MessageBox.Show( "Logged in successfully!", "Success", MessageBoxButton.OK);

                _role = selectedRole.RoleName;

                if (RemembermeCheckBox.IsChecked == true)
                {
                    Encrypt(password, username);
                }
                else
                {
                    config.AppSettings.Settings["isPasswordRemmembered"].Value = "0";
                    config.AppSettings.Settings["username"].Value = " ";
                    config.AppSettings.Settings["password"].Value = " ";
                    config.AppSettings.Settings["entropy"].Value = " ";
                    config.AppSettings.Settings["DABPRIVILEGE"].Value = selectedRole.RoleName;
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

                byte[]? cypherText = null;
                byte[]? entropy = null;
                byte[]? decryptedPassword = null;
                string? realPassword = null;

                string? cypherTextStr = ConfigurationManager.AppSettings["password"];
                if(cypherTextStr != null)
                {
                    cypherText = Convert.FromBase64String(cypherTextStr);
                }

                string? entropyStr = ConfigurationManager.AppSettings["entropy"];
                if(entropyStr != null)
                {
                    entropy = Convert.FromBase64String(entropyStr);
                }

                if (cypherText != null && entropy != null) {
                    decryptedPassword = ProtectedData.Unprotect(cypherText, entropy, DataProtectionScope.CurrentUser);
                }
                
                if(decryptedPassword != null)
                {
                    realPassword = Encoding.UTF8.GetString(decryptedPassword);
                }
                
                PasswordBox.Password = realPassword;
                RemembermeCheckBox.IsChecked = true;
            }

            DataSourceTextBox.Text = config.AppSettings.Settings["DATASOURCE"].Value;
            DBAPrivilegeComboBox.ItemsSource = roleOptions;
            DBAPrivilegeComboBox.SelectedItem = roleOptions.Single(x => x.RoleName == config.AppSettings.Settings["DABPRIVILEGE"].Value);
        }
    }
}
