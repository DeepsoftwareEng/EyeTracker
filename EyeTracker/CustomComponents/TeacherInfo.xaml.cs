using EyeTracker.MVVM.Model;
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

namespace EyeTracker.CustomComponents
{
    /// <summary>
    /// Interaction logic for TeacherInfo.xaml
    /// </summary>
    public partial class TeacherInfo : UserControl
    {
        public TeacherInfo()
        {
            InitializeComponent();
        }
        public void addInfo(GiaoVien t, List<Lop> lops)
        {
            NameTxb.Text = t.TenGV;
            DateTxb.Text = t.NgaySinh.ToString("dd/MM/yyyy");
            foreach(var i in lops)
            {
                ClassLwv.Items.Add(i.TenLop);
            }

        }
        public void Clear()
        {
            NameTxb.Text = string.Empty;
            DateTxb.Text = string.Empty;
            ClassLwv.Items.Clear();
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
