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
    /// Interaction logic for ChangingClassView.xaml
    /// </summary>
    public partial class ChangingClassView : UserControl
    {
        public ChangingClassView()
        {
            InitializeComponent();
        }
        public void AddData(Lop lop)
        {
            txtMalophoc.Text = lop.MaLop;
            txtTenLophoc.Text = lop.TenLop;
            txtGiaoVien.SelectedValue = lop.MaGV;
        }
        public void ClearData()
        {
            txtMalophoc.Text = txtTenLophoc.Text = string.Empty;
            txtGiaoVien.SelectedIndex = 0;
        }
        public void SetCbbDAta(List<GiaoVien> giaoviens)
        {
            txtGiaoVien.ItemsSource = giaoviens;
            txtGiaoVien.DisplayMemberPath = "TenGV";
            txtGiaoVien.SelectedValuePath = "MaGV";
            txtGiaoVien.SelectedIndex = 0;
        }
    }
}
