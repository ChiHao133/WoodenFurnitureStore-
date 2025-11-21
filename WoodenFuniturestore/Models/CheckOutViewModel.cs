using System.ComponentModel.DataAnnotations;

namespace WoodenFuniturestore.Models
{
    public class CheckOutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTenNhanHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress]
        public string EmailNhanHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone]
        public string SoDienThoaiNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string DiaChiGiaoHang { get; set; }

        public string? GhiChu { get; set; } // Ghi chú cho đơn hàng (tùy chọn)

        // === Phần 2: Thông tin giỏ hàng (để hiển thị) ===

        public List<ChiTietGioHang> CartItems { get; set; } = new List<ChiTietGioHang>();

        public decimal TongTien { get; set; }
        public List<int> SelectedItems { get; set; } = new List<int>();
    }
}
