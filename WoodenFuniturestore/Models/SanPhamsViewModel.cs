namespace WoodenFuniturestore.Models
{
    internal class SanPhamsViewModel
    {
        // ID để tạo link chi tiết, thêm vào giỏ hàng...
        public int Id { get; set; }

        public string TenSanPham { get; set; }

        // Chỉ chứa tên file ảnh, ví dụ: "ten-san-pham.jpg"
        public string? HinhAnh { get; set; }

        // Giá gốc của sản phẩm (đã định dạng)
        public decimal GiaGoc { get; set; }

        // Giá cuối cùng sau khi áp dụng khuyến mãi (đã định dạng)
        public decimal GiaBan { get; set; }

        // Một biến bool để View dễ dàng kiểm tra
        public bool CoKhuyenMai { get; set; }

        // Điểm đánh giá trung bình
        public double Rating { get; set; }
    }
}