using EyeTracker.CustomComponents;
using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        private HocSinh choosenStudent = new HocSinh();
        private string  maGV;
        private string choosenImage;
        private static string binFolderPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        private static string projectFolderPath = Directory.GetParent(binFolderPath).FullName;
        private static string fix = projectFolderPath.Remove(projectFolderPath.Length - 9);
        private static string dataFolderPath = System.IO.Path.Combine(fix, "UserData");
        private static string LogFolderPath = System.IO.Path.Combine(fix, "Log");
        private string filepath;
        public StudentManageUI(string maGV)
        {
            InitializeComponent();
            this.maGV = maGV;
            filepath = LogFolderPath + $"{maGV}.txt";
            LoadStudent();
            LoadFunction();
        }
        private void LoadFunction()
        {
            StudentInfo.AddBtn.MouseLeftButtonDown += AddStudent;
            StudentInfo.EditBtn.MouseLeftButtonDown += EditStudent;
            StudentInfo.DelBtn.MouseLeftButtonDown += DelStudent;
            StudentChange.BackBtn.MouseLeftButtonDown += Back;
            StudentChange.AddImgBtn.MouseLeftButtonDown += AddImageClick;
        }
        private void NullWrpImage()
        {
            foreach(var i in StudentWrp.Children)
            {
                if(i is StudentTab temp && Int32.Parse(temp.Tag.ToString()) == choosenStudent.MaHocSinh)
                {
                    temp.RemoveImage();
                }
            }
        }
        private void AddImageClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp, *.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (fileDialog.ShowDialog() == true)
            {
                choosenImage = fileDialog.FileName;
                Stream fs = File.Open(fileDialog.FileName, FileMode.Open);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad; 
                bmp.StreamSource = fs;
                bmp.EndInit();
                StudentChange.StudentImg.Source = bmp;
                fs.Close();
            }
        }

        private void Back(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StudentChange.Clear();
            StudentChange.IsEnabled = false;
            StudentChange.Visibility = Visibility.Hidden;
        }

        private void DelStudent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn xóa ?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                string querry = "delete from HocSinh where MaHocSinh = @mahocsinh";
                if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                    dc.GetConnection().Open();
                cmd = new SqlCommand(querry, dc.GetConnection());
                try
                {
                    cmd.Parameters.AddWithValue("@mahocsinh", choosenStudent.MaHocSinh);
                    cmd.ExecuteNonQuery();
                    NullWrpImage();
                    StudentInfo.StudentImg.Source = null;
                    File.Delete(dataFolderPath + $"\\StudentImage\\{choosenStudent.MaHocSinh}.png");
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine($"{maGV} - {DateTime.Now}: Xoa hoc sinh: {choosenStudent.MaHocSinh}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                StudentInfo.Clear();
                StudentWrp.Children.RemoveAt(choosenStudent.MaHocSinh);
                choosenStudent = new HocSinh();
                MessageBox.Show("Xóa thành công");
            }
        }

        private void EditStudent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StudentChange.IsEnabled = true;
            StudentChange.Visibility = Visibility.Visible;
            StudentChange.SaveBtn.MouseLeftButtonDown += EditSaveBtn;
            StudentChange.addInfo(choosenStudent);
            Stream fs = File.Open(dataFolderPath + $"\\StudentImage\\{choosenStudent.MaHocSinh}.png", FileMode.Open);
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad; // This will allow you to close the stream after EndInit
            bmp.StreamSource = fs;
            bmp.EndInit();
            StudentChange.StudentImg.Source = bmp;
            fs.Close();
        }

        private void EditSaveBtn(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DateOnly dob = DateOnly.Parse(StudentChange.DateTxb.Text);
            DateOnly enroll = DateOnly.Parse(StudentChange.EnrollTxb.Text);
            string querry = "Update HocSinh set HoTen = @hoten, NgaySinh = @ngaysinh, NamNhapHoc = @namnhaphoc, @DiaChi=@diachi, MaLop =@malop, @MaGV = @magv where MaHocSinh = @mahocsinh";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(querry, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@hoten", StudentChange.NameTxb.Text);
                cmd.Parameters.AddWithValue("@ngaysinh", dob.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@namnhaphoc", enroll.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@diachi", StudentChange.AddressTxb.Text);
                cmd.Parameters.AddWithValue("@docanthi", double.Parse(StudentChange.MyopiaTxb.Text));
                cmd.Parameters.AddWithValue("@malop", StudentChange.ClassTxb.Text);
                cmd.Parameters.AddWithValue("@magv", StudentChange.TeacherTxb.Text);
                cmd.Parameters.AddWithValue("@mahocsinh", choosenStudent.MaHocSinh);
                cmd.ExecuteNonQuery();
                if(choosenImage != null && choosenImage != dataFolderPath + $"\\StudentImage\\{choosenStudent.MaHocSinh}.png" )
                {
                    StudentInfo.StudentImg.Source = null;
                    StudentChange.StudentImg.Source = null;
                    NullWrpImage();
                    File.Delete(dataFolderPath + $"\\StudentImage\\{choosenStudent.MaHocSinh}.png");
                    File.Copy(choosenImage, dataFolderPath + $"\\StudentImage\\{choosenStudent.MaHocSinh}.png");
                    Stream fs = File.Open(dataFolderPath + $"\\StudentImage\\{choosenStudent.MaHocSinh}.png", FileMode.Open);
                    BitmapImage bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = fs;
                    bmp.EndInit();
                    StudentInfo.StudentImg.Source = bmp;
                    fs.Close();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine($"{maGV} - {DateTime.Now}: Sua hoc sinh: {choosenStudent.MaHocSinh}");
                }
                dc.GetConnection().Close();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Thành công!");
            StudentWrp.Children.Clear();
            LoadStudent();
            StudentChange.Clear();
            StudentChange.IsEnabled = false;
            StudentChange.Visibility = Visibility.Hidden;
           
        }

        private void AddStudent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StudentChange.IsEnabled = true;
            StudentChange.Visibility = Visibility.Visible;
            StudentChange.SaveBtn.MouseLeftButtonDown += AddSaveBtn;
        }
        private int MaxID()
        {
            int t;
            string query = "Select Max(MaHocSinh) from HocSinh";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            try
            {
                t = Int32.Parse(cmd.ExecuteScalar().ToString());
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            dc.GetConnection().Close();
            return t;
        }
        private void AddSaveBtn(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int t = MaxID();
            DateOnly date = DateOnly.Parse(StudentChange.DateTxb.Text);
            DateOnly date2 = DateOnly.Parse(StudentChange.EnrollTxb.Text);
            string query = "insert into HocSinh value(@hoten,@ngaysinh,@namnhaphoc,@diachi,@docanthi,@malop,@magv)";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@hoten",StudentChange.NameTxb.Text);
                cmd.Parameters.AddWithValue("@ngaysinh",date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@namnhaphoc",date2.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@diachi",StudentChange.AddressTxb.Text);
                cmd.Parameters.AddWithValue("@docanthi",StudentChange.MyopiaTxb.Text);
                cmd.Parameters.AddWithValue("@malop",StudentChange.ClassTxb.Text);
                cmd.Parameters.AddWithValue("@magv",StudentChange.TeacherTxb.Text);
                cmd.ExecuteNonQuery();
                if(choosenImage != null)
                {
                    File.Copy(choosenImage, dataFolderPath + $"\\StudentImage\\{t + 1}.png");
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine($"{maGV} - {DateTime.Now}: Them hoc sinh: {t+1}");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
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
            choosenStudent = temp;
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
                dc.GetConnection().Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
