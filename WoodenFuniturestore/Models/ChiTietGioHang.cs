using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("ChiTietGioHang")]
public partial class ChiTietGioHang
{
    [Key]
    public int MaChiTiet { get; set; }

    public int? MaGioHang { get; set; }

    public int? MaSanPham { get; set; }

    public int? SoLuong { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? GiaBan { get; set; }

    [ForeignKey("MaGioHang")]
    [InverseProperty("ChiTietGioHangs")]
    public virtual GioHang? MaGioHangNavigation { get; set; }

    [ForeignKey("MaSanPham")]
    [InverseProperty("ChiTietGioHangs")]
    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
