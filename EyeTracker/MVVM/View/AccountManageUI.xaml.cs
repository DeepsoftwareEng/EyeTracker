using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EyeTracker.CustomComponents;
using System.Linq;

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for AccountManageUI.xaml
    /// </summary>
    public partial class AccountManageUI : Page
    {
        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        private List<TaiKhoan> taikhoans = new();
        private TaiKhoan choosenAccount = new();

        public AccountManageUI()
        {
            InitializeComponent();
            GetTaiKhoans();
            LoadFunction();
        }
        private void LoadFunction()
        {
            AccountInfo.AddBtn.MouseLeftButtonDown += AddAccount;
            AccountInfo.EditBtn.MouseLeftButtonDown += EditAccount;
            AccountInfo.DelBtn.MouseLeftButtonDown += DeleteAccount;
            AccountChange.BackBtn.MouseLeftButtonDown += GoBack;
        }

        private void GoBack(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AccountChange.ClearData();
            AccountChange.Visibility = Visibility.Hidden;
        }
        private void KeepTeacher()
        {
            string query = "Update GiaoVien set TenTaiKhoan = '' where TenTaiKhoan =@tk";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@tk", AccountInfo.AccountTxb.Text);
                cmd.ExecuteNonQuery();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection();
        }
        private void DeleteAccount(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(MessageBox.Show("Co chac chan muon xoa ?","Canh bao", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                KeepTeacher();
                string query = "Delete from TaiKhoan where tentaikhoan =@tk";
                if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                    dc.GetConnection().Open();
                cmd = new SqlCommand(query, dc.GetConnection());
                try
                {
                    cmd.Parameters.AddWithValue("@tk", AccountInfo.AccountTxb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Da xoa thanh cong");
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dc.GetConnection().Close();
                AccountInfo.ClearData();
                AccountWrp.Children.Clear();
                taikhoans.Clear();
                GetTaiKhoans();
            }
        }

        private void EditAccount(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AccountChange.Visibility = Visibility.Visible;
            AccountChange.AddData(choosenAccount);
            AccountChange.SaveBtn.MouseLeftButtonDown += SaveEditAccount;
            AccountChange.AccountTxb.IsReadOnly = true;
            AccountChange.IsEnabled = true;
        }

        private void SaveEditAccount(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string query = "Update TaiKhoan set MatKhau = @matkhau, Chucvu = @chucvu where TenTaiKhoan = @tk";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@matkhau", AccountChange.PasswordTxb.Text);
                cmd.Parameters.AddWithValue("@chucvu", AccountChange.RoleTxb.SelectedItem);
                cmd.Parameters.AddWithValue("@tk", AccountChange.AccountTxb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thanh cong");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
            AccountChange.Visibility = Visibility.Hidden;
            AccountChange.IsEnabled = false;
            AccountChange.ClearData();
            AccountChange.AccountTxb.IsReadOnly= false;
            AccountWrp.Children.Clear();
            taikhoans.Clear();
            GetTaiKhoans();
        }

        private void AddAccount(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AccountChange.Visibility = Visibility.Visible;
            AccountChange.IsEnabled = true;
            AccountChange.SaveBtn.MouseLeftButtonDown += SaveNewAccount;
        }

        private void SaveNewAccount(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string query = "Insert into TaiKhoan values(@tentk,@matkhau,@role)";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@tentk", AccountChange.AccountTxb.Text);
                cmd.Parameters.AddWithValue("@matkhau", AccountChange.PasswordTxb.Text);
                cmd.Parameters.AddWithValue("@role", AccountChange.RoleTxb.SelectedItem);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Them thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
            AccountChange.Visibility = Visibility.Hidden;
            AccountChange.IsEnabled = false;
            AccountChange.ClearData();
            AccountWrp.Children.Clear();
            taikhoans.Clear();
            GetTaiKhoans();
        }

        private void GetTaiKhoans()
        {
            string query = "Select * from TaiKhoan where tentaikhoan != '' and tentaikhoan != 'gv0'";
            if(dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query,dc.GetConnection());
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TaiKhoan temp = new();
                    temp.TenTaiKhoan = reader.GetString(0);
                    temp.MatKhau = reader.GetString(1);
                    temp.Chucvu = reader.GetString(2);
                    taikhoans.Add(temp);
                    AccountTab at = new AccountTab(temp.Chucvu, temp.TenTaiKhoan);
                    at.MouseLeftButtonDown += ChooseAccount;
                    at.Tag = temp.TenTaiKhoan;
                    AccountWrp.Children.Add(at);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
        }

        private void ChooseAccount(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var i = taikhoans.Where(t => t.TenTaiKhoan == (sender as AccountTab).Tag.ToString()).FirstOrDefault();
            if (i != null)
            {
                AccountInfo.addinfo(i);
                choosenAccount = i;
            }
        }
    }
}
