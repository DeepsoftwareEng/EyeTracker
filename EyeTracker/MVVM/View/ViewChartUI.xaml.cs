﻿using EyeTracker.MVVM.Model;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for ViewChartUI.xaml
    /// </summary>
    public partial class ViewChartUI : Page
    {
        private string Magv;
        public SeriesCollection mypoia { get; set; }
        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        private List<Lop> classes = new List<Lop>();
        private List<HocSinhChart> students = new List<HocSinhChart>();
        public ViewChartUI(string magv)
        {
            InitializeComponent();
            Magv = magv;
            AddData();
            DataContext = this;
        }
        private void GetClasses()
        {
            string query = "Select * from Lop where GVCN = @magv";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@magv", Magv);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Lop temp = new Lop();
                    temp.MaLop = reader.GetString(0);
                    temp.TenLop = reader.GetString(1);
                    temp.MaGV = Magv;
                    classes.Add(temp);
                    Button btn = new Button();
                    btn.Content = temp.TenLop;
                    btn.Tag = temp.MaLop;
                    btn.Click += ChooseClass_Click;
                    ClassListBtn.Children.Add(btn);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
            foreach(var i in classes)
            {
                GetStudent(i.MaLop);
            }
        }

        private void ChooseClass_Click(object sender, RoutedEventArgs e)
        {
            SeriesCollection seriesViews = new SeriesCollection();
            string malop = (sender as Button).Tag.ToString();
            var SameClass = from HocSinhChart in students where HocSinhChart.MaLop == malop orderby HocSinhChart.Mypoia select HocSinhChart;
            List<HocSinhChart> sameclass = SameClass.ToList();
            var result = sameclass.Select(s => s.Mypoia).Distinct();
            foreach (var i in result)
            {
                PieSeries t = new PieSeries();
                t.Title = i.ToString();
                t.Values = new ChartValues<ObservableValue> { new ObservableValue(sameclass.Where(s => s.Mypoia == i).Count())};
                t.LabelPoint = chartPoint => string.Format(i.ToString()+": " + (chartPoint.Participation*100).ToString()+"%");
                t.DataLabels = true;
                seriesViews.Add(t);
            }
            MypoiaChart.IsEnabled = false;
            MypoiaChart.IsEnabled = true;
            MypoiaChart.Series = seriesViews;
        }

        private void GetStudent(string ClassCode)
        {
            string query = "select * from HocSinh where MaLop = @malop";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                cmd.Parameters.AddWithValue("@malop", ClassCode);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HocSinhChart temp = new HocSinhChart();
                    temp.Mypoia = reader.GetDouble(5);
                    temp.MaLop = ClassCode;
                    students.Add(temp);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
        }
        private void AddData()
        {
            GetClasses();
        }
    }
}
