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
    /// Interaction logic for MainMenuUI.xaml
    /// </summary>
    public partial class MainMenuUI : Page
    {
        public MainMenuUI()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageContentChange.NavigationService.Navigate(new ClassManageUI());
        }

        private void ManageStudentTxb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageContentChange.NavigationService.Navigate(new StudentManageUI("GV01"));
        }

        private void ViewChartTxb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageContentChange.NavigationService.Navigate(new ViewChartUI());
        }

        private void LogoutImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}