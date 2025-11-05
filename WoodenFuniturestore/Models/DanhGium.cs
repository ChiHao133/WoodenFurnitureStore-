using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

public partial class DanhGium
{
    [Key]
    [Column("DanhGiaID")]
    public int DanhGiaId { get; set; }

    [Column("SanPhamID")]
    public int SanPhamId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    public int? Rating { get; set; }

    [Column(TypeName = "ntext")]
    public string? NoiDung { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayDanhGia { get; set; }

    public bool? IsDuyet { get; set; }

    [ForeignKey("SanPhamId")]
    [InverseProperty("DanhGia")]
    public virtual SanPham SanPham { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("DanhGia")]
    public virtual NguoiDung User { get; set; } = null!;
}
