using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.SINHVIEN
{
    public class SinhVien : INotifyPropertyChanged
    {
        public required string MASV { get; set; }
        public string? HoTen { get; set; }
        public string? GioiTinh { get; set; }
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? MaNganh { get; set; }
        public string? CoSo { get; set; }
        public string? MaChuongTrinh { get; set; }
        public double? DTBTL { get; set; }
        public int? SOTCTL { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
