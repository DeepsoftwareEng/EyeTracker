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

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainMenuUI.xaml
    /// </summary>
    public partial class MainMenuUI : Page
    {
        private string MaGV;
        private static string binFolderPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        private static string projectFolderPath = Directory.GetParent(binFolderPath).FullName;
        private static string fix = projectFolderPath.Remove(projectFolderPath.Length - 9);
        private static string dataFolderPath = System.IO.Path.Combine(fix, "UserData");
        public MainMenuUI(string magv, string tengv)
        {
            InitializeComponent();
            MaGV = magv;
            MagvTxb.Text += magv;
            TeacherNameTxb.Text = tengv;
            Stream fs = File.Open(dataFolderPath + $"\\TeacherImage\\{magv}.png", FileMode.Open);
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad; // This will allow you to close the stream after EndInit
            bmp.StreamSource = fs;
            bmp.EndInit();
            TeacherImg.ImageSource = bmp;
            fs.Close();
            ManageContentChange.NavigationService.Navigate(new DashBoard(MaGV));
        }

        private void ManageStudentTxb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageContentChange.NavigationService.Navigate(new StudentManageUI(MaGV));
        }

        private void ViewChartTxb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageContentChange.NavigationService.Navigate(new ViewChartUI(MaGV));
        }

        private void LogoutImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new LoginUI());
        }

        private void DashboardTxb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageContentChange.NavigationService.Navigate(new DashBoard(MaGV));
        }

        private void StudentReportBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageContentChange.NavigationService.Navigate(new ReportUI(MaGV));
        }
    }
}
