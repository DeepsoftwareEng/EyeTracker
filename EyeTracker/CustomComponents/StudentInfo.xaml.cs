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
    /// Interaction logic for StudentInfo.xaml
    /// </summary>
    public partial class StudentInfo : UserControl
    {
        public StudentInfo()
        {
            InitializeComponent();
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
