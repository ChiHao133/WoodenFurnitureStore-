namespace WoodenFuniturestore.Models
{
    public class SanPhamDetailViewModel
    {
        public SanPham SanPham { get; set; }
        public List<SanPhamsViewModel>? SanPhamLienQuan { get; set; }
        public List<DanhGium> DanhSachDanhGia { get; set; }
        public double RatingTrungBinh { get; set; }
        public int SoLuongDanhGia { get; set; }
    }
}
