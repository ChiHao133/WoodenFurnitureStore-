using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("NguoiDung")]
[Index("Email", Name = "UQ__NguoiDun__A9D1053434EEBC55", IsUnique = true)]
public partial class NguoiDung
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string MatKhau { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? SoDienThoai { get; set; }

    [StringLength(500)]
    public string? DiaChi { get; set; }

    public DateOnly? NgaySinh { get; set; }

    [Column("VaiTroID")]
    public int VaiTroId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    [InverseProperty("User")]
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    [InverseProperty("MaNguoiDungNavigation")]
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    [ForeignKey("VaiTroId")]
    [InverseProperty("NguoiDungs")]
    public virtual VaiTro VaiTro { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
