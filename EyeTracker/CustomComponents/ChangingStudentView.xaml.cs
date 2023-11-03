using EyeTracker.MVVM.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EyeTracker.CustomComponents
{
    /// <summary>
    /// Interaction logic for ChangingStudentView.xaml
    /// </summary>
    public partial class ChangingStudentView : UserControl
    {
        public ChangingStudentView()
        {
            InitializeComponent();
        }
        public void addInfo(HocSinh t)
        {
            NameTxb.Text = t.HoTen;
            DateTxb.Text = t.NgaySinh.ToString("dd/MM/yyyy");
            EnrollTxb.Text = t.NamNhapHoc.ToString("dd/MM/yyyy");
            AddressTxb.Text = t.DiaChi.ToString();
            MyopiaTxb.Text = t.DoCanThi.ToString();
            ClassTxb.Text = t.MaLop.ToString();
            TeacherTxb.Text = t.MaGV.ToString();
        }
        public void Clear() 
        {
            NameTxb.Text = string.Empty;
            DateTxb.Text = string.Empty;
            EnrollTxb.Text = string.Empty;
            AddressTxb.Text = string.Empty;
            MyopiaTxb.Text = string.Empty;
            ClassTxb.Text = string.Empty;
            TeacherTxb.Text = string.Empty;
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
