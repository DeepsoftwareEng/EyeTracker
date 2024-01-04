using DocumentFormat.OpenXml.Bibliography;
using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;


namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for LoginUI.xaml
    /// </summary>
    public partial class LoginUI : Page
    {
        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        private static string binFolderPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        private static string projectFolderPath = Directory.GetParent(binFolderPath).FullName;
        private static string fix = projectFolderPath.Remove(projectFolderPath.Length - 9);
        private static string dataFolderPath = System.IO.Path.Combine(fix, "Log");
        private static string CookieFolderPath = System.IO.Path.Combine(fix, "Cookie");
        private string currentUsername;
        private string currentPassword;
        public LoginUI()
        {
            InitializeComponent();
            if(RemebermeExists() == true)
            {
                StreamReader rd = new StreamReader(CookieFolderPath + $"\\RememberMe.txt", Encoding.ASCII);
                string test = rd.ReadToEnd();
                rd.Close();
                string[] currentdata = test.Split('-');
                currentUsername = currentdata[0];
                currentPassword = currentdata[1];
                UsernameTxb.Text = currentUsername;
                PassTxb.Password = currentPassword;
            }
        }
        private bool RemebermeExists()
        {
            if(!File.Exists(CookieFolderPath + $"\\RememberMe.txt"))
            return false;
            return true;
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            string query = "Select Count(1) from TaiKhoan where TenTaiKhoan = @tentk and MatKhau = @mk";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            try
            {
                cmd = new SqlCommand(query, dc.GetConnection());
                cmd.Parameters.AddWithValue("@tentk", UsernameTxb.Text);
                cmd.Parameters.AddWithValue("@mk", PassTxb.Password);
                temp = Int32.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            if (temp == 1)
            {
                query = "Select ChucVu from TaiKhoan where TenTaiKhoan = @tentk";
                cmd = new SqlCommand(query, dc.GetConnection());
                string role = "";
                string tengv = "";
                string magv = "";
                try
                {
                    cmd.Parameters.AddWithValue("@tentk", UsernameTxb.Text);
                    role = cmd.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (role.ToUpper() == "ADMIN")
                {
                    string filepath = $"{dataFolderPath}\\admin-{UsernameTxb.Text}.txt";
                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Close();
                        StreamWriter writer = new StreamWriter(filepath);
                        writer.Write($"{UsernameTxb.Text}-{DateTime.Now}: Login as Admin");
                        writer.Close();
                    }
                    else
                    {
                        string content = $"{UsernameTxb.Text}-{DateTime.Now}: Login as Admin";
                        File.AppendAllText(filepath, content);
                    }
                    if(RememberCkb.IsChecked == true)
                    {
                        if (!File.Exists(CookieFolderPath + $"\\RememberMe.txt"))
                            File.Create(CookieFolderPath + $"\\RememberMe.txt").Close();
                        StreamWriter writer = new StreamWriter(filepath);
                        writer.Write($"{UsernameTxb.Text}-{PassTxb.Password}-admin");
                        writer.Close();
                    }
                    else
                    {
                        File.Delete(CookieFolderPath + $"\\RememberMe.txt");
                    }
                    this.NavigationService.Navigate(new AdminUI(UsernameTxb.Text));
                }
                else
                {
                    query = "Select MaGV, TenGV from GiaoVien where TenTaiKhoan = @tentk";
                    cmd = new SqlCommand(query, dc.GetConnection());
                    try
                    {
                        cmd.Parameters.AddWithValue("@tentk", UsernameTxb.Text);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            magv = reader.GetString(0);
                            tengv = reader.GetString(1);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    string filepath = $"{dataFolderPath}\\{magv}.txt";
                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Close();
                        StreamWriter writer = new StreamWriter(filepath);
                        writer.Write($"{magv}-{DateTime.Now}: Login as Teacher");
                        writer.Close();
                    }
                    else
                    {
                        string content = $"{magv}-{DateTime.Now}: Login as Teacher";
                        File.AppendAllText(filepath, content);
                    }
                    if (RememberCkb.IsChecked == true)
                    {
                        if (!File.Exists(CookieFolderPath + $"\\RememberMe.txt"))
                            File.Create(CookieFolderPath + $"\\RememberMe.txt").Close();
                        if((PassTxb.Password != currentPassword || UsernameTxb.Text != currentUsername) && currentPassword != string.Empty && currentUsername!= string.Empty) {
                            File.WriteAllText(CookieFolderPath + $"\\RememberMe.txt", string.Empty);    
                        }
                        StreamWriter writer = new StreamWriter(CookieFolderPath + $"\\RememberMe.txt");
                        writer.Write($"{UsernameTxb.Text}-{PassTxb.Password}-teacher");
                        writer.Close();
                    }
                    else
                    {
                        File.Delete(CookieFolderPath + $"\\RememberMe.txt");
                    }
                    this.NavigationService.Navigate(new MainMenuUI(magv, tengv));
                }

            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
            dc.GetConnection().Close();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new Recovery());
        }
    }
}
