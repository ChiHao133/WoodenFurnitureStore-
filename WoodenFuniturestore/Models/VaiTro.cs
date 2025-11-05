using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("VaiTro")]
[Index("TenVaiTro", Name = "UQ__VaiTro__1DA55814BED26C7E", IsUnique = true)]
public partial class VaiTro
{
    [Key]
    [Column("VaiTroID")]
    public int VaiTroId { get; set; }

    [StringLength(50)]
    public string TenVaiTro { get; set; } = null!;

    [InverseProperty("VaiTro")]
    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
