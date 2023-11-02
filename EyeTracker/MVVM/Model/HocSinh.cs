using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracker.MVVM.Model
{
    internal class HocSinh
    {
        public int MaHocSinh { get; set; }
        public string HoTen { get; set; }
        public DateOnly NgaySinh { get; set; }
        public int NamNhapHoc { get; set; }
        public string DiaChi { get; set; }
        public float DoCanThi { get; set; }
        public string MaLop { get; set; }
        public string MaGV { get; set; }
    }
}
