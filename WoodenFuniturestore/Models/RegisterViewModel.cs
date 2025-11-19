using System.ComponentModel.DataAnnotations;

namespace WoodenFuniturestore.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu không khớp.")]
        public string XacNhanMatKhau { get; set; }

        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
    }
}
