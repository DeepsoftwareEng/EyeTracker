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
    /// Interaction logic for ChangingAccountView.xaml
    /// </summary>
    public partial class ChangingAccountView : UserControl
    {
        public ChangingAccountView()
        {
            InitializeComponent();
            RoleTxb.Items.Add("Admin");
            RoleTxb.Items.Add("Teacher");
        }
        public void AddData(TaiKhoan tk)
        {
            AccountTxb.Text = tk.TenTaiKhoan;
            PasswordTxb.Text = tk.MatKhau;
            RoleTxb.SelectedItem = tk.Chucvu;
        }
        public void ClearData()
        {
            AccountTxb.Text = string.Empty;
            PasswordTxb.Text = string.Empty;
            RoleTxb.SelectedIndex = 0;
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(50, 48, 15));
            (sender as Border).Background = brush;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(74, 58, 16));
            (sender as Border).Background = brush;
        }
    }
}
