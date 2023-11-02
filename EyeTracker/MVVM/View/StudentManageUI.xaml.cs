using EyeTracker.CustomComponents;
using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for StudentManageUI.xaml
    /// </summary>
    public partial class StudentManageUI : Page
    {
        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        private List<HocSinh> Students = new List<HocSinh>();
        private string  maGV;
        private static string binFolderPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        private static string projectFolderPath = Directory.GetParent(binFolderPath).FullName;
        private static string fix = projectFolderPath.Remove(projectFolderPath.Length - 9);
        private static string dataFolderPath = System.IO.Path.Combine(fix, "UserData");
        public StudentManageUI(string maGV)
        {
            InitializeComponent();
            this.maGV = maGV;
            LoadStudent();
        }
        private StudentTab LoadTab(int id, string name)
        {
            StudentTab studentTab = new StudentTab();
            studentTab.Height = 60;
            studentTab.Width = 150;
            studentTab.Tag = id;
            studentTab.StudentNameTxb.Text = name;
            Stream fs = File.Open(dataFolderPath + $"\\StudentImage\\{id}.png", FileMode.Open);
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad; // This will allow you to close the stream after EndInit
            bmp.StreamSource = fs;
            bmp.EndInit();
            studentTab.StudentImg.Source = bmp;
            fs.Close();
            studentTab.MouseLeftButtonDown += ChooseStudent;
            return studentTab;
        }

        private void ChooseStudent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int id = Int32.Parse((sender as StudentTab).Tag.ToString());
            var result = from HocSinh in Students where HocSinh.MaHocSinh == id select HocSinh;
            HocSinh temp = result.FirstOrDefault();
            StudentInfo.addInfo(temp.HoTen, temp.NgaySinh, temp.NamNhapHoc, temp.DiaChi, temp.DoCanThi, temp.MaLop, temp.MaGV);
            Stream fs = File.Open(dataFolderPath + $"\\StudentImage\\{id}.png", FileMode.Open);
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad; // This will allow you to close the stream after EndInit
            bmp.StreamSource = fs;
            bmp.EndInit();
            fs.Close();
            StudentInfo.StudentImg.Source = bmp;
        }

        private void LoadStudent()
        {
            string query = "Select * from HocSinh where MaGV = @MaGV";
            if(dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open(); 
            try
            {
                cmd = new SqlCommand(query, dc.GetConnection());
                cmd.Parameters.AddWithValue("@MaGV", maGV);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TextBlock space = new TextBlock();
                    space.Width = 5;
                    HocSinh temp = new HocSinh();
                    temp.MaHocSinh = reader.GetInt32(0);
                    temp.HoTen = reader.GetString(1);
                    temp.NgaySinh = DateOnly.FromDateTime(reader.GetDateTime(2));
                    temp.NamNhapHoc = DateOnly.FromDateTime(reader.GetDateTime(3));
                    temp.DiaChi = reader.GetString(4);
                    temp.DoCanThi = reader.GetDouble(5);
                    temp.MaLop = reader.GetString(6);
                    temp.MaGV = reader.GetString(7);
                    Students.Add(temp);
                    StudentWrp.Children.Add(LoadTab(temp.MaHocSinh, temp.HoTen));
                    StudentWrp.Children.Add(space);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
