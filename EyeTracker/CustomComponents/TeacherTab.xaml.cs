using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace EyeTracker.CustomComponents
{
    /// <summary>
    /// Interaction logic for TeacherTab.xaml
    /// </summary>
    public partial class TeacherTab : UserControl
    {
        public TeacherTab()
        {
            InitializeComponent();
        }
        private void StudentTab_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(169, 169, 169));
            mask.Background = brush;
        }

        private void StudentTab_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            mask.Background = brush;
        }
        public void RemoveImage()
        {
            TeacherImg.ImageSource = null;
        }
    }
}
