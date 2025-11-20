using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("GioHang")]
public partial class GioHang
{
    [Key]
    public int MaGioHang { get; set; }

    public int? MaNguoiDung { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaGioHangNavigation")]
    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("GioHangs")]
    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
