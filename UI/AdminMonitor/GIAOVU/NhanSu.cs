using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.GIAOVU
{
    public class NhanSu : INotifyPropertyChanged
    {
        public string? MaNV { get; set; }
        public string? HoTen { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? PhuCap { get; set; }

        public string? SDT { get; set; }

        public string? MaDV { get; set; }
        public string? CoSo { get; set; }

        public string? VaiTro { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
