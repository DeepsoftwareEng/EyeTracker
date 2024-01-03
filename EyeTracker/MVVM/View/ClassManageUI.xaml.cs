using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EyeTracker.CustomComponents;

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for ClassManageUI.xaml
    /// </summary>
    public partial class ClassManageUI : Page
    {

        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        List<Lop> lops = new List<Lop>();
        List<GiaoVien> gv = new List<GiaoVien>();
        public ClassManageUI()
        {
            InitializeComponent();
            GetTeacher();
            layDL();
            LoadFuntion();
        }
        private void LoadFuntion()
        {
            ClassInfo.AddBtn.MouseLeftButtonDown += AddClass;
            ClassInfo.EditBtn.MouseLeftButtonDown += EditClass;
            ClassInfo.DelBtn.MouseLeftButtonDown += DeleteClass;
            ClassChange.BackBtn.MouseLeftButtonDown += GoBack;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            ClassChange.ClearData();
            ClassChange.Visibility = Visibility.Hidden;
            ClassChange.IsEnabled = false;
        }
        private void KeepStudent(string malop)
        {
            string query = "UPdate HocSinh set MaLop = 'L00' where malop = @malop";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@malop", malop);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
        }
        private void DeleteClass(object sender, MouseButtonEventArgs e)
        {
            string query = "Delete from Lop where MaLop = @malop";
            KeepStudent(ClassInfo.MaLopTxb.Text);
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@malop", ClassInfo.MaLopTxb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
            ClassInfo.ClearData();
            ClassWrp.Children.Clear();
            lops.Clear();
            layDL();
        }

        private void EditClass(object sender, MouseButtonEventArgs e)
        {
            ClassChange.Visibility = Visibility.Visible;
            ClassChange.IsEnabled = true;
            Lop temp = new();
            temp.MaLop = ClassInfo.MaLopTxb.Text;
            temp.TenLop = ClassInfo.TenLopTxb.Text;
            temp.MaGV = ClassInfo.TenGvTxb.Text;
            ClassChange.AddData(temp);
            ClassChange.SetCbbDAta(gv);
            ClassChange.SaveBtn.MouseLeftButtonDown += SaveEditClass;
        }

        private void SaveEditClass(object sender, MouseButtonEventArgs e)
        {
            string query = "Update Lop set TenLop = @tenlop, GVCN = @gvcn where MaLop = @malop";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@tenlop", ClassChange.txtTenLophoc.Text);
                cmd.Parameters.AddWithValue("@gvcn", ClassChange.txtGiaoVien.Text);
                cmd.Parameters.AddWithValue("@malop", ClassChange.txtGiaoVien.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
            ClassChange.ClearData();
            ClassChange.Visibility = Visibility.Hidden;
            ClassChange.IsEnabled = false;
            ClassWrp.Children.Clear();
            lops.Clear();
            layDL();
        }

        private void AddClass(object sender, MouseButtonEventArgs e)
        {
            ClassChange.Visibility = Visibility.Visible;
            ClassChange.IsEnabled = true;
            ClassChange.SaveBtn.MouseLeftButtonDown += SaveNewClass;
            ClassChange.SetCbbDAta(gv);
        }

        private void SaveNewClass(object sender, MouseButtonEventArgs e)
        {
            string query = "insert into Lop values(@malop,@tenlop,@gvcn)";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@malop", ClassChange.txtMalophoc.Text);
                cmd.Parameters.AddWithValue("@tenlop", ClassChange.txtTenLophoc.Text);
                cmd.Parameters.AddWithValue("@gvcn", ClassChange.txtGiaoVien.SelectedValue.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection();
            ClassChange.ClearData();
            ClassChange.Visibility = Visibility.Hidden;
            ClassChange.IsEnabled = false;
            ClassWrp.Children.Clear();
            lops.Clear();
            layDL();
        }

        private void layDL()
        {
            string query = "Select * from Lop ";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            try
            {
                cmd = new SqlCommand(query, dc.GetConnection());
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TextBlock space = new TextBlock();
                    space.Width = 5;
                    Lop temp = new Lop();
                    temp.MaLop = reader.GetString(0);
                    temp.TenLop = reader.GetString(1);
                    temp.MaGV = reader.GetString(2);
                    lops.Add(temp);
                    ClassWrp.Children.Add(space);
                    var query1 = from t in gv
                                 where t.MaGV == temp.MaGV

                                 select t.TenGV;
                    ClassTab a = new(query1.FirstOrDefault(), temp.TenLop);
                    a.MouseLeftButtonDown += chooseclass;
                    a.Tag = temp.MaLop;
                    ClassWrp.Children.Add(a);
                }
            }
            catch { }

        }

        private void chooseclass(object sender, MouseButtonEventArgs e)
        {
            string id = (sender as ClassTab).Tag.ToString();
            var query = lops.Where(t => t.MaLop == id).FirstOrDefault();
            var query1 = gv.Where(t => t.MaGV == query.MaGV).Select(t => t.TenGV).FirstOrDefault();
            ClassInfo.addInfo(query, query1.ToString());

        }

        private void GetTeacher()
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
                gv.Add(t);
            }
            reader.Close();
            dc.GetConnection().Close();
        }
    }
}
