using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("DonHang")]
public partial class DonHang
{
    [Key]
    [Column("DonHangID")]
    public int DonHangId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayDatHang { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TongTien { get; set; }

    [StringLength(100)]
    public string TrangThai { get; set; } = null!;

    [StringLength(500)]
    public string? DiaChiGiaoHang { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? SoDienThoaiNhan { get; set; }

    [InverseProperty("DonHang")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    [ForeignKey("UserId")]
    [InverseProperty("DonHangs")]
    public virtual NguoiDung? User { get; set; }
}
