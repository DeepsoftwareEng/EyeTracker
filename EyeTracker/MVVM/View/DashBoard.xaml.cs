using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using EyeTracker.MVVM.Model;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using LiveCharts.Defaults;
using System.Xml.Serialization;
using System.Collections;
using System.Threading.Tasks;

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Page
    {
        private DataConnection dc = new DataConnection();
        private SqlCommand cmd;
        List<HocSinhChart> students = new List<HocSinhChart>();
        public SeriesCollection SeriesCollection= new SeriesCollection();
        public string[] Labels { get; set; }
        public Func<double, string> Values { get; set; }
        public DashBoard()
        {
            InitializeComponent();
            GetStudent();
            Values = value => value.ToString("N");
            setLabel();
            setValue();
            DataContext = this;
            DashboardChart.Series = SeriesCollection;
            setCount();
        }
        private void setCount()
        {
            int student = setStudentCount();
            int teacher = setTeacherCount();
            int classes = setClassCount();
            Task t1 = Task.Run(() =>
            {
                for (int i = 0; i <= student; i += 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        StudentCount.Text = i.ToString();
                    });
                    
                }
            });
            Task t2 = Task.Run(() =>
            {
                for (int j = 0; j <= teacher; j += 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        TeacherCount.Text = j.ToString();
                    });
                    
                }
            });
            Task t3 = Task.Run(() =>
            {
                for (int z = 0; z <= classes; z += 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ClassCount.Text = z.ToString();
                    });
                    
                }
            });
        }
        private void GetStudent()
        {
            string query = "select * from HocSinh";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HocSinhChart temp = new HocSinhChart();
                    temp.Mypoia = reader.GetDouble(5);
                    temp.MaLop = reader.GetString(6);
                    students.Add(temp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
        }
        private void setLabel()
        {
            Labels = new string[students.Count];
            for(int i = 0; i < students.Count; i++)
            {
                Labels[i] = students[i].Mypoia.ToString();
            }
        }
        private void setValue()
        {
            var result = students.Select(s => s.Mypoia).Distinct().ToList().OrderByDescending(n => n);
            foreach(var i in result)
            {
                ColumnSeries temp = new ColumnSeries();
                temp.Title = $"{i}";
                temp.ColumnPadding = 0.5;
                temp.Values = new ChartValues<double> { CountValue(i) };
                SeriesCollection.Add(temp);
            }
        }
        private double CountValue(double i)
        {
           return students.Where(s => s.Mypoia == i).Count();
        }
        private int setStudentCount()
        {
            string query = "";
            int temp = 0;
            if(dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            query = "Select count(*) from HocSinh";
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                return int.Parse(cmd.ExecuteScalar().ToString());

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
            return 0;
        }
        private int setTeacherCount()
        {
            string query;
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            query = "Select count(*) from GiaoVien";
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
            return 0;
        }
        private int setClassCount()
        {
            string query;
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            query = "Select count(*) from Lop";
            cmd = new SqlCommand(query, dc.GetConnection());
            try
            {
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dc.GetConnection().Close();
            return 0;
        }
    }
}
