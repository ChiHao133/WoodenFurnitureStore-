namespace WoodenFuniturestore.Models
{
    internal class SanPhamsViewModel
    {
        public int SanPhamID { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }
        public decimal GiaGoc { get; set; }
        public decimal GiaBan { get; set; }
        public double Rating { get; set; }
        public bool CoKhuyenMai => GiaBan < GiaGoc;
    }
}