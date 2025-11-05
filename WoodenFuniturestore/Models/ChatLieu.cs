using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WoodenFuniturestore.Models;

[Table("ChatLieu")]
public partial class ChatLieu
{
    [Key]
    [Column("ChatLieuID")]
    public int ChatLieuId { get; set; }

    [StringLength(255)]
    public string TenChatLieu { get; set; } = null!;

    [InverseProperty("ChatLieu")]
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
