using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracker.MVVM.Model
{
    public class GiaoVien
    {
        public string MaGV { get; set; }
        public string TenGV { get; set; }
        public DateOnly NgaySinh { get; set; }
        public string TenTaiKhoan { get; set; }
    }
}
