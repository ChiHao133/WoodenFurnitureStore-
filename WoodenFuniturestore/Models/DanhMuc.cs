using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("DanhMuc")]
public partial class DanhMuc
{
    [Key]
    [Column("DanhMucID")]
    public int DanhMucId { get; set; }

    [StringLength(255)]
    public string TenDanhMuc { get; set; } = null!;

    [StringLength(1000)]
    public string? MoTa { get; set; }

    [Column("DanhMucChaID")]
    public int? DanhMucChaId { get; set; }

    [ForeignKey("DanhMucChaId")]
    [InverseProperty("InverseDanhMucCha")]
    public virtual DanhMuc? DanhMucCha { get; set; }

    [InverseProperty("DanhMucCha")]
    public virtual ICollection<DanhMuc> InverseDanhMucCha { get; set; } = new List<DanhMuc>();

    [InverseProperty("DanhMuc")]
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
