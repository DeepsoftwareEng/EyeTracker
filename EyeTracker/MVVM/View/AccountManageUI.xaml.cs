using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EyeTracker.CustomComponents;

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

        public AccountManageUI()
        {
            InitializeComponent();
            GetTaiKhoans();
        }
        private void GetTaiKhoans()
        {
            string query = "Select * from TaiKhoan";
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
                    AccountWrp.Children.Add(new AccountTab(temp.Chucvu, temp.TenTaiKhoan));
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
        }
    }
}
