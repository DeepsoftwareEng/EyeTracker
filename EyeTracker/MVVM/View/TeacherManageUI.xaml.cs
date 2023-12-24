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
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for TeacherManageUI.xaml
    /// </summary>
    public partial class TeacherManageUI : Page
    {
        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        private List<GiaoVien> giaoViens = new List<GiaoVien>();
        private List<Lop> lops = new List<Lop>();
        //private List<GiaoVienLop> giaoVienLops = new();
        private static string binFolderPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        private static string projectFolderPath = Directory.GetParent(binFolderPath).FullName;
        private static string fix = projectFolderPath.Remove(projectFolderPath.Length - 9);
        private static string dataFolderPath = System.IO.Path.Combine(fix, "UserData");
        private static string LogFolderPath = System.IO.Path.Combine(fix, "Log");
        private string choosenImage;
        private string choosenTeacherId;
        private string filepath;
        private readonly string tentk;

        public TeacherManageUI(string tentk)
        {
            InitializeComponent();
            Loadfunction();
            LoadData();
            this.tentk = tentk;
            filepath = LogFolderPath + $"\\admin-{tentk}.txt";
        }
        private void Loadfunction()
        {
            TeacherInfo.AddBtn.MouseLeftButtonDown += AddTeacher;
            TeacherInfo.EditBtn.MouseLeftButtonDown += EditTeacher;
            TeacherInfo.DelBtn.MouseLeftButtonDown += DeleteTeacher;
            TeacherChange.AddImgBtn.MouseLeftButtonDown += AddImage;
            TeacherChange.BackBtn.MouseLeftButtonDown += GoBack;
        }
        private void LoadData()
        {
            string query = "Select * from GiaoVien where MaGv != 'GV00'";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                GiaoVien t = new();
                t.MaGV = reader.GetString(0);
                t.TenGV = reader.GetString(1);
                t.NgaySinh = DateOnly.FromDateTime(reader.GetDateTime(2));
                t.TenTaiKhoan = reader.GetString(3);
                TeacherWrp.Children.Add(teacherTabs(t));
                giaoViens.Add(t);
            }
            dc.GetConnection().Close();
            query = "Select * from Lop";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Lop t = new();
                t.MaLop = reader.GetString(0);
                t.TenLop = reader.GetString(1);
                t.MaGV = reader.GetString(2);
                lops.Add(t);
            }
            dc.GetConnection().Close();
        }
        private TeacherTab teacherTabs(GiaoVien gv)
        {
            TeacherTab temp = new TeacherTab();
            temp.TeacherNameTxb.Text = gv.TenGV;
            Stream fs = File.Open(dataFolderPath + $"\\TeacherImage\\{gv.MaGV}.png", FileMode.Open);
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad; // This will allow you to close the stream after EndInit
            bmp.StreamSource = fs;
            bmp.EndInit();
            temp.TeacherImg.Source = bmp;
            fs.Close();
            temp.MouseLeftButtonDown += ChooseTeacher;
            temp.Tag = gv.MaGV;
            return temp;
        }

        private void ChooseTeacher(object sender, MouseButtonEventArgs e)
        {
            string teacherId = (sender as TeacherTab).Tag.ToString();
            var lopCN = lops.Where(c => c.MaGV == teacherId).ToList();
            var gv = giaoViens.Where(t => t.MaGV == teacherId).FirstOrDefault();
            TeacherInfo.addInfo(gv,lopCN);
            Stream fs = File.Open(dataFolderPath + $"\\TeacherImage\\{gv.MaGV}.png", FileMode.Open);
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad; // This will allow you to close the stream after EndInit
            bmp.StreamSource = fs;
            bmp.EndInit();
            TeacherInfo.TeacherImg.Source = bmp;
            fs.Close();
            choosenTeacherId = teacherId;
            TeacherInfo.AddBtn.Tag = gv.MaGV;
            TeacherInfo.EditBtn.Tag = gv.MaGV;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            TeacherChange.RemoveData();
            TeacherChange.Visibility = Visibility.Hidden;
        }

        private void AddImage(object sender, MouseButtonEventArgs e)
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
                TeacherChange.TeacherImg.Source = bmp;
                fs.Close();
            }
        }
        private int DeletedTeacherClass()
        {
            string query = "Update Lop set Magv = GV00 where magv = @magv";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@magv", choosenTeacherId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            dc.GetConnection().Close();
            return 1;
        }
        private int DeletedTeacherStudent()
        {
            string query = "Update HocSinh set Magv = GV00 where magv = @magv";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@magv", choosenTeacherId);
                cmd.ExecuteNonQuery();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            dc.GetConnection().Close();
            return 1;
        }
        private void DeleteTeacher(object sender, MouseButtonEventArgs e)
        {
            if (DeletedTeacherStudent() == 1 && DeletedTeacherClass()==1)
            {
                string query = "Delete from GiaoVien where Magv = @magv";
                if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                    dc.GetConnection().Open();
                cmd = new SqlCommand(query, dc.GetConnection());
                try
                {
                    cmd.Parameters.AddWithValue("@magv", choosenTeacherId);
                    cmd.ExecuteNonQuery();
                    File.Delete($"\\TeacherImage\\{choosenTeacherId}.png");
                    string content = $"{tentk} - {DateTime.Now}: Xoa giao vien: {choosenTeacherId}";
                    File.AppendAllText(filepath, content);
                    MessageBox.Show("Thanh cong");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dc.GetConnection().Close();
            }
            else
            {
                MessageBox.Show("khong the chuyen hoc sinh sang giao vien khac");
            }
        }

        private void EditTeacher(object sender, MouseButtonEventArgs e)
        {
            GiaoVien temp = new();
            temp = giaoViens.Where(t => t.MaGV == choosenTeacherId).FirstOrDefault();
            var lopCN = lops.Where(c => c.MaGV == choosenTeacherId).ToList();
            TeacherChange.SetData(temp, lopCN,lops);
            TeacherChange.Visibility = Visibility.Visible;
            TeacherChange.IsEnabled = true;
            TeacherChange.SaveBtn.MouseLeftButtonDown += SaveEditTeacher;
            TeacherChange.SaveBtn.Tag = (sender as Border).Tag.ToString();
        }
        private void SaveEditClass(string magv, List<Lop> ds)
        {
            string query = "Update Lop Set GVCN = @gvcn where MaLop = @malop";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query,dc.GetConnection());
            cmd.Parameters.AddWithValue("@gvcn", magv);
            foreach(var i in ds)
            {
                cmd.Parameters.AddWithValue("@malop", i.MaLop);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            dc.GetConnection().Close();
        }
        private void SaveEditTeacher(object sender, MouseButtonEventArgs e)
        {
            DateOnly dob = DateOnly.ParseExact(TeacherChange.DateTxb.Text, "dd/MM/yyyy", null);
            string query = "Update GiaoVien set TenGV =@tengv, NgaySinh = @ngaysinh where MaGV= @magv";
            List<Lop> ds = new();
            foreach (var i in TeacherChange.ClassLvw.Items)
            {
                var result = lops.Where(c => c.TenLop == i.ToString()).FirstOrDefault();
                ds.Add(result);
            }
            SaveEditClass((sender as Border).Tag.ToString(), ds);
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            cmd.Parameters.AddWithValue("@tengv", TeacherChange.NameTxb.Text);
            cmd.Parameters.AddWithValue("@ngaysinh", dob.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@magv", (sender as Border).Tag.ToString());
            try
            {
                cmd.ExecuteNonQuery();
                if(choosenImage != null && choosenImage != dataFolderPath+$"\\TeacherImage\\{(sender as Border).Tag.ToString()}.png")
                {
                    TeacherChange.TeacherImg.Source = null;
                    NullWrpImage((sender as Border).Tag.ToString());
                    File.Delete($"\\TeacherImage\\{(sender as Border).Tag.ToString()}.png");
                    File.Copy(choosenImage, $"\\TeacherImage\\{(sender as Border).Tag.ToString()}.png");
                    Stream fs = File.Open(dataFolderPath + $"\\TeacherImage\\{(sender as Border).Tag.ToString()}.png", FileMode.Open);
                    BitmapImage bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = fs;
                    bmp.EndInit();
                    TeacherInfo.TeacherImg.Source = bmp;
                    fs.Close();
                }
                string content = $"{tentk} - {DateTime.Now}: Sua giao vien: {choosenTeacherId}";
                File.AppendAllText(filepath, content);
                MessageBox.Show("Thanh cong");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            dc.GetConnection().Close();
            TeacherChange.RemoveData();
            TeacherChange.IsEnabled = false;
            TeacherChange.Visibility = Visibility.Hidden;
            TeacherWrp.Children.Clear();
            giaoViens.Clear();
            LoadData();
        }

        private void AddTeacher(object sender, MouseButtonEventArgs e)
        {
            TeacherChange.Visibility = Visibility.Visible;
            TeacherChange.IsEnabled = true;
            TeacherChange.SetClasses(lops);
            TeacherChange.SaveBtn.MouseLeftButtonDown += SaveNewTeacher;
        }
        private string NewId()
        {
            string t = "";
            string query = "select Top 1 * from GiaoVien Order By MaGV DESC";
            if(dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                t = cmd.ExecuteScalar().ToString().Substring(2);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            int temp = Int32.Parse(t);
            temp++;
            if (temp < 10)
                return $"0{temp}";
            return $"{temp}";
        }
        private void SaveNewTeacher(object sender, MouseButtonEventArgs e)
        {
            DateOnly date = DateOnly.ParseExact(TeacherChange.DateTxb.Text.ToString(), "dd/MM/yyyy", null);
            string query = "insert into GiaoVien values(@magv,@tengv,@ngaysinh,@tentk)";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            cmd.Parameters.AddWithValue("@magv", "GV" +NewId);
            cmd.Parameters.AddWithValue("@tengv", TeacherChange.NameTxb.Text);
            cmd.Parameters.AddWithValue("@ngaysinh", date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@tentk", TeacherChange.AccountTxb.Text);
            try
            {
                cmd.ExecuteNonQuery();
                string content = $"{tentk} - {DateTime.Now}: Them giao vien: {"GV" + NewId}";
                File.AppendAllText(filepath, content);
                MessageBox.Show("Thanh cong");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            TeacherWrp.Children.Clear();
            giaoViens.Clear();
            LoadData();
            dc.GetConnection().Close();
            TeacherChange.RemoveData();
            TeacherChange.IsEnabled = false;
            TeacherChange.Visibility = Visibility.Hidden;
        }
        private void NullWrpImage(string id)
        {
            foreach(var i in TeacherWrp.Children)
            {
                if (i is TeacherTab temp && temp.Tag.ToString() == id)
                    temp.RemoveImage();
            }
        }
    }
}
