using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("KhuyenMai")]
public partial class KhuyenMai
{
    [Key]
    [Column("KhuyenMaiID")]
    public int KhuyenMaiId { get; set; }

    [StringLength(255)]
    public string TenSuKien { get; set; } = null!;

    [StringLength(1000)]
    public string? MoTa { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal PhanTramGiam { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime NgayBatDau { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime NgayKetThuc { get; set; }

    [InverseProperty("KhuyenMai")]
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
