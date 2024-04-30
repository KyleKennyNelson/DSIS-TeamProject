using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.GIAOVU
{
    public class HocPhan : INotifyPropertyChanged
    {
        public string? MaHP { get; set; }
        public string? TenHP { get; set; }
        public int? SoTinChi {  get; set; }
        public int? SoTietLiThuyet { get; set; }
        public int? SoTietThucHanh { get; set; }
        public int? SoSinhVienThamDu { get; set; }
        public string? MaDV { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
