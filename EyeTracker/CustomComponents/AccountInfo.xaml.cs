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
    /// Interaction logic for AccountInfo.xaml
    /// </summary>
    public partial class AccountInfo : UserControl
    {
        public AccountInfo()
        {
            InitializeComponent();
        }
        public void addinfo(TaiKhoan tk)
        {
            AccountTxb.Text = tk.TenTaiKhoan;
            PasswordTxb.Text = tk.MatKhau;
            RoleTxb.Text = tk.Chucvu;
            EmailTxt.Text = tk.Email;
        }
        public void ClearData()
        {
            AccountTxb.Text = string.Empty;
            PasswordTxb.Text = string.Empty;
            RoleTxb.Text = string.Empty;
            EmailTxt.Text = string.Empty;
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(169, 169, 169));
            (sender as Border).Background = brush;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            (sender as Border).Background = brush;
        }
    }
}
