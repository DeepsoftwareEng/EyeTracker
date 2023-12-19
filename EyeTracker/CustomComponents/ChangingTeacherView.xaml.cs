using EyeTracker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace EyeTracker.CustomComponents
{
    /// <summary>
    /// Interaction logic for ChangingTeacherView.xaml
    /// </summary>
    public partial class ChangingTeacherView : UserControl
    {
        public ChangingTeacherView()
        {
            InitializeComponent();
        }
        public void SetClasses(List<Lop> lops)
        {
            foreach (var i in lops)
            {
                ClassChooseCb.Items.Add(i.TenLop);
            }
        }
        public void SetData(GiaoVien teacher,List<Lop> lopcn ,List<Lop> dsLop)
        {
            NameTxb.Text = teacher.TenGV;
            DateTxb.Text = teacher.NgaySinh.ToString("dd/MM/yyyy");
            AccountTxb.Text = teacher.TenTaiKhoan.ToString();
            foreach(var i in lopcn)
            {
                ClassLvw.Items.Add(i.TenLop);
            }
            foreach(var i in dsLop)
            {
                ClassChooseCb.Items.Add(i.TenLop);
            }
        }
        public void RemoveData()
        {
            NameTxb.Text = string.Empty;
            DateTxb.Text = string.Empty;
            ClassLvw.Items.Clear();
            ClassChooseCb.Items.Clear();
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(36, 157, 159));
            (sender as Border).Background = brush;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(0, 255, 255));
            (sender as Border).Background = brush;
        }

    }
}
