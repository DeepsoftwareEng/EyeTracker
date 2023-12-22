using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using EyeTracker.MVVM.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace EyeTracker.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReportUI.xaml
    /// </summary>
    public partial class ReportUI : Page
    {
        private readonly DataConnection dc = new DataConnection();
        private readonly string magv;
        private SqlCommand cmd;
        private List<HocSinh> hocSinhs = new();
        private List<Lop> lops = new();
        private GiaoVien giaovien = new GiaoVien();
        private string ReportType;
        private string filepath;
        private static string binFolderPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        private static string projectFolderPath = Directory.GetParent(binFolderPath).FullName;
        private static string fix = projectFolderPath.Remove(projectFolderPath.Length - 9);
        private static string LogFolderPath = System.IO.Path.Combine(fix, "Log");
        public ReportUI(string magv)
        {
            InitializeComponent();
            this.magv = magv;
            filepath = LogFolderPath + $"{magv}.txt";
            LoadData();
            LoadFunction();
        }
        private void LoadFunction()
        {
            StudentDetailBtn.Click += ViewStudentReport;
            ClassDetail.Click += ViewClassReport;
            MypoiaDetailBtn.Click += ViewMypoiaReport;
            ExportBtn.MouseLeftButtonDown += ExportBtn_MouseLeftButtonDown;
            ExportBtn.MouseEnter += ExportBtn_MouseEnter;
            ExportBtn.MouseLeave += ExportBtn_MouseLeave;
        }

        private void ExportBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(169,169,169));
            (sender as System.Windows.Controls.Border).Background = brush;
        }
        private void ExportBtn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(211, 211, 211));
            (sender as System.Windows.Controls.Border).Background = brush;
        }
        private void ExportBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(ReportDtg.Items.Count == 0)
            {
                MessageBox.Show("Bảng thống kê trống");
            }
            else
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.DefaultExt = ".xlsx";
                savefile.Filter = "Excel Files|*xlsx;*xls;*xlsm";
                savefile.FileName = $"DataReport - {ReportType} - {DateTime.Now.Date}.xlsx";
                if (savefile.ShowDialog() == true)
                {
                    try
                    {
                        var workbook = new XLWorkbook();

                        workbook.Style.Font.FontSize = 11;
                        workbook.Style.Font.FontName = "Times New Roman";

                        var sheet1 = workbook.Worksheets.Add("Sheet1");
                        int rowWrite = 1;

                        Type t;
                        PropertyInfo[] p;
                        IXLCell cell;
                        cell = sheet1.Cell(rowWrite, 1);
                        cell.Value = "Attandance workers list";
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 14;
                        rowWrite++;
                        for (int col = 0; col < ReportDtg.Columns.Count; col++)
                        {
                            cell = sheet1.Cell(rowWrite, col + 1);
                            cell.Value = ReportDtg.Columns[col].Header.ToString();
                            cell.Style.Fill.BackgroundColor = XLColor.Aqua;
                            cell.Style.Font.Bold = true;
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                        rowWrite++;

                        foreach (var item in ReportDtg.Items)
                        {
                            for (int col = 0; col < ReportDtg.Columns.Count; col++)
                            {
                                TextBlock Value = ReportDtg.Columns[col].GetCellContent(item) as TextBlock;
                                cell = sheet1.Cell(rowWrite, col + 1);
                                cell.Value = Value.Text;
                                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#F2F2F2");
                                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            }
                            rowWrite++;
                        }
                        rowWrite++;
                        sheet1.Columns().AdjustToContents();
                        workbook.SaveAs(savefile.FileName);
                        MessageBox.Show("Done", "Message", MessageBoxButton.OK);
                        workbook.Dispose();
                        using (StreamWriter sw = File.AppendText(filepath))
                        {
                            sw.WriteLine($"{magv} - {DateTime.Now}: Xuat danh sach {ReportType}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void ViewMypoiaReport(object sender, RoutedEventArgs e)
        {
            ReportType = "Độ cận thị";
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(169, 169, 169));
            SolidColorBrush brush2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(211, 211, 211));
            (sender as Button).Background = brush;
            StudentDetailBtn.Background = ClassDetail.Background = brush2;
            ReportDtg.Columns.Clear();
            ReportDtg.Items.Clear();
            DataGridTextColumn tenlop = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Tên lớp",
                Binding = new Binding("TenLop")
            };
            DataGridTextColumn gvcn = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Giáo viên chủ nhiệm",
                Binding = new Binding("GVCN")
            };
            DataGridTextColumn sohocsinhcanthi = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Số học sinh bị cận thị",
                Binding = new Binding("Amount")
            };
            ReportDtg.Columns.Add(tenlop);
            ReportDtg.Columns.Add(gvcn);
            ReportDtg.Columns.Add(sohocsinhcanthi); 
            foreach (var i in lops)
            {
                var sameclass = hocSinhs.Where(s => s.MaLop == i.MaLop).ToList();
                ReportDtg.Items.Add(new { TenLop = i.TenLop, GVCN = giaovien.TenGV, Amount =  hocSinhs.Count - hocSinhs.Where(s=>s.DoCanThi ==0).Count()});
            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine($"{magv} - {DateTime.Now}: xem thong ke do can thi");
            }
        }

        private void ViewClassReport(object sender, RoutedEventArgs e)
        {
            ReportType = "Lớp";
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(169, 169, 169));
            SolidColorBrush brush2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(211, 211, 211));
            (sender as Button).Background = brush;
            StudentDetailBtn.Background = MypoiaDetailBtn.Background = brush2;
            ReportDtg.Columns.Clear();
            ReportDtg.Items.Clear();
            DataGridTextColumn malop = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Mã lớp",
                Binding = new Binding("MaLop")
            };
            DataGridTextColumn tenlop = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Tên lớp",
                Binding = new Binding("TenLop")
            };
            DataGridTextColumn gvcn = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Giáo viên chủ nhiệm",
                Binding = new Binding("GVCN")
            };
            DataGridTextColumn siso = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Sĩ số",
                Binding = new Binding("Amount")
            };
            ReportDtg.Columns.Add(malop);
            ReportDtg.Columns.Add(tenlop);
            ReportDtg.Columns.Add(gvcn);
            ReportDtg.Columns.Add(siso);
            foreach (var i in lops)
            {
                ReportDtg.Items.Add(new {MaLop = i.MaLop, TenLop = i.TenLop,GVCN = giaovien.TenGV, Amount = hocSinhs.Where(s => s.MaLop == i.MaLop).Count() });
            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine($"{magv} - {DateTime.Now}: xem thong ke lop");
            }
        }

        private void ViewStudentReport(object sender, RoutedEventArgs e)
        {
            ReportType = "Học sinh";
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(169,169,169));
            SolidColorBrush brush2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(211, 211, 211));
            (sender as Button).Background = brush;
            ClassDetail.Background = MypoiaDetailBtn.Background = brush2;
            ReportDtg.Columns.Clear();
            ReportDtg.Items.Clear();
            DataGridTextColumn mahs = new DataGridTextColumn() {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Mã học sinh",
                Binding = new Binding("MaHocSinh")
            };
            DataGridTextColumn hoten = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Họ tên học sinh",
                Binding = new Binding("HoTen")
            };
            DataGridTextColumn ngaysinh = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Ngày sinh",
                Binding = new Binding("NgaySinh")
            };
            DataGridTextColumn namnhaphoc = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Năm nhập học",
                Binding = new Binding("NamNhapHoc")
            };
            DataGridTextColumn diachi = new DataGridTextColumn()
            {
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                Header = "Địa chỉ",
                Binding = new Binding("DiaChi")
            };
            DataGridTextColumn docanthi = new DataGridTextColumn()
            {
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Header = "Độ cận thị",
                Binding = new Binding("DoCanThi")
            };
            DataGridTextColumn lop = new DataGridTextColumn()
            {
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                Header = "Lớp",
                Binding = new Binding("Lop")
            };
            ReportDtg.Columns.Add(mahs);
            ReportDtg.Columns.Add(hoten);
            ReportDtg.Columns.Add(ngaysinh);
            ReportDtg.Columns.Add(namnhaphoc);
            ReportDtg.Columns.Add(diachi);
            ReportDtg.Columns.Add(docanthi);
            ReportDtg.Columns.Add(lop);
            foreach(var i in hocSinhs)
            {
                ReportDtg.Items.Add(new {MaHocSinh = i.MaHocSinh, HoTen = i.HoTen, NgaySinh = i.NgaySinh.ToString(), NamNhapHoc = i.NamNhapHoc.ToString(), DiaChi = i.DiaChi, DoCanThi = i.DoCanThi, Lop = lops.Where(c => c.MaLop == i.MaLop).Select(c=> c.TenLop).FirstOrDefault()});
            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine($"{magv} - {DateTime.Now}: xem thong ke hoc sinh");
            }
        }

        private void LoadData()
        {
            string query = "Select * from HocSinh where MaGV = @MaGV";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            try
            {
                cmd = new SqlCommand(query, dc.GetConnection());
                cmd.Parameters.AddWithValue("@MaGV", magv);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HocSinh temp = new HocSinh();
                    temp.MaHocSinh = reader.GetInt32(0);
                    temp.HoTen = reader.GetString(1);
                    temp.NgaySinh = DateOnly.FromDateTime(reader.GetDateTime(2));
                    temp.NamNhapHoc = DateOnly.FromDateTime(reader.GetDateTime(3));
                    temp.DiaChi = reader.GetString(4);
                    temp.DoCanThi = reader.GetDouble(5);
                    temp.MaLop = reader.GetString(6);
                    temp.MaGV = reader.GetString(7);
                    hocSinhs.Add(temp);
                }
                dc.GetConnection().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            query = "Select * from Lop  where GVCN = @MaGV";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            try
            {
                cmd = new SqlCommand(query, dc.GetConnection());
                cmd.Parameters.AddWithValue("@MaGV", magv);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Lop temp = new();
                    temp.MaLop = reader.GetString(0);
                    temp.TenLop = reader.GetString(1);
                    lops.Add(temp);
                }
                dc.GetConnection().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            query = "Select * from GiaoVien where MaGV = @MaGV";
            if (dc.GetConnection().State == System.Data.ConnectionState.Closed)
                dc.GetConnection().Open();
            try
            {
                cmd = new SqlCommand(query, dc.GetConnection());
                cmd.Parameters.AddWithValue("@MaGV", magv);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    giaovien.TenGV = reader.GetString(1);
                }
                dc.GetConnection().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
