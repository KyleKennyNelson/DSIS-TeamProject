using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.GIAOVU
{
    public class DonVi : INotifyPropertyChanged
    {
        public string? MaDV { get; set; }
        public string? TenDonVi { get; set; }
        public string? TruongDonVi { get; set; }

        public string? CoSo { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
