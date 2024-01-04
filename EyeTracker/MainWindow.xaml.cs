using EyeTracker.MVVM.View;
using System.IO;
using System.Windows;


namespace EyeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new LoginUI());
        }
        
    }
}
