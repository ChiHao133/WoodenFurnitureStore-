using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("SanPham")]
public partial class SanPham
{
    [Key]
    [Column("SanPhamID")]
    public int SanPhamId { get; set; }

    [StringLength(500)]
    public string TenSanPham { get; set; } = null!;

    [Column(TypeName = "ntext")]
    public string? MoTa { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Gia { get; set; }

    [StringLength(255)]
    public string? KichThuoc { get; set; }

    public int SoLuongTon { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? HinhAnh { get; set; }

    public bool? IsNoiBat { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [Column("DanhMucID")]
    public int? DanhMucId { get; set; }

    [Column("ChatLieuID")]
    public int? ChatLieuId { get; set; }

    [Column("KhuyenMaiID")]
    public int? KhuyenMaiId { get; set; }

    [ForeignKey("ChatLieuId")]
    [InverseProperty("SanPhams")]
    public virtual ChatLieu? ChatLieu { get; set; }

    [InverseProperty("SanPham")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    [InverseProperty("MaSanPhamNavigation")]
    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    [InverseProperty("SanPham")]
    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    [ForeignKey("DanhMucId")]
    [InverseProperty("SanPhams")]
    public virtual DanhMuc? DanhMuc { get; set; }

    [ForeignKey("KhuyenMaiId")]
    [InverseProperty("SanPhams")]
    public virtual KhuyenMai? KhuyenMai { get; set; }

    [ForeignKey("SanPhamId")]
    [InverseProperty("SanPhams")]
    public virtual ICollection<NguoiDung> Users { get; set; } = new List<NguoiDung>();
}
