﻿using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;


namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for LoginUI.xaml
    /// </summary>
    public partial class LoginUI : Page
    {
        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        public LoginUI()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            int temp = 0; 
            string query = "Select Count(1) from TaiKhoan where TenTaiKhoan = @tentk and MatKhau = @mk";
            if(dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            try
            {
                cmd = new SqlCommand(query, dc.GetConnection());
                cmd.Parameters.AddWithValue("@tentk", UsernameTxb.Text);
                cmd.Parameters.AddWithValue("@mk", PassTxb.Password);
                temp = Int32.Parse(cmd.ExecuteScalar().ToString());
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
            if(temp == 1)
            {
                query = "Select MaGV from GiaoVien where TenTaiKhoan = @tentk";
                cmd = new SqlCommand(query, dc.GetConnection());
                string magv = "";
                try
                {
                    cmd.Parameters.AddWithValue("@tentk", UsernameTxb.Text);
                    magv = cmd.ExecuteScalar().ToString();
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.NavigationService.Navigate(new MainMenuUI(magv));
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
            dc.GetConnection().Close();
        }
    }
}
