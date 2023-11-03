using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for StudentTab.xaml
    /// </summary>
    public partial class StudentTab : UserControl
    {
        
        public StudentTab()
        {
            InitializeComponent();
        }

        private void StudentTab_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(169,169,169));
            mask.Background = brush;
        }

        private void StudentTab_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(211,211, 211));
            mask.Background = brush;
        }
        public void RemoveImage()
        {
            StudentImg.Source = null;
        }
    }
}
