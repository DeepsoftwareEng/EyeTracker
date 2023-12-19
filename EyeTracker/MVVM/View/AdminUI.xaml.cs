﻿using System;
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

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for AdminUI.xaml
    /// </summary>
    public partial class AdminUI : Page
    {
        public AdminUI()
        {
            InitializeComponent();
        }
        private void LogoutImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new LoginUI());
        }
        private void QuanLyGiaoVien_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageContentChange.NavigationService.Navigate(new TeacherManageUI());
        }
    }
}
