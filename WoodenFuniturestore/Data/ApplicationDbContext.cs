using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WoodenFuniturestore.Models;

namespace WoodenFuniturestore.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatLieu> ChatLieus { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ChisHafo\\SQLEXPRESS; Initial Catalog=WoodenFurnitureStore; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatLieu>(entity =>
        {
            entity.HasKey(e => e.ChatLieuId).HasName("PK__ChatLieu__214CED5B82CE1B05");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.ChiTietDonHangId).HasName("PK__ChiTietD__45B33F83B9C44BA2");

            entity.HasOne(d => d.DonHang).WithMany(p => p.ChiTietDonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__DonHa__787EE5A0");

            entity.HasOne(d => d.SanPham).WithMany(p => p.ChiTietDonHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__SanPh__797309D9");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PK__ChiTietG__CDF0A1145F4972F1");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGioHangs).HasConstraintName("FK__ChiTietGi__MaGio__2BFE89A6");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietGioHangs).HasConstraintName("FK__ChiTietGi__MaSan__2DE6D218");
        });

        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.DanhGiaId).HasName("PK__DanhGia__52C0CA257979BFB2");

            entity.Property(e => e.IsDuyet).HasDefaultValue(false);
            entity.Property(e => e.NgayDanhGia).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.SanPham).WithMany(p => p.DanhGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__SanPham__7F2BE32F");

            entity.HasOne(d => d.User).WithMany(p => p.DanhGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__UserID__00200768");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.DanhMucId).HasName("PK__DanhMuc__1C53BA7BF606BE3A");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.DonHangId).HasName("PK__DonHang__D159F4DE84D058CC");

            entity.Property(e => e.NgayDatHang).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.DonHangs).HasConstraintName("FK__DonHang__UserID__75A278F5");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA3C706C335");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.GioHangs).HasConstraintName("FK__GioHang__MaNguoi__2CF2ADDF");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.KhuyenMaiId).HasName("PK__KhuyenMa__820D74779731BF4D");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__NguoiDun__1788CCACC3205E32");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.VaiTro).WithMany(p => p.NguoiDungs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NguoiDung__VaiTr__619B8048");

            entity.HasMany(d => d.SanPhams).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "SanPhamYeuThich",
                    r => r.HasOne<SanPham>().WithMany()
                        .HasForeignKey("SanPhamId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SanPhamYe__SanPh__03F0984C"),
                    l => l.HasOne<NguoiDung>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SanPhamYe__UserI__02FC7413"),
                    j =>
                    {
                        j.HasKey("UserId", "SanPhamId").HasName("PK__SanPhamY__47D94C5347E7E551");
                        j.ToTable("SanPhamYeuThich");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                        j.IndexerProperty<int>("SanPhamId").HasColumnName("SanPhamID");
                    });
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.SanPhamId).HasName("PK__SanPham__05180FF42E95DC72");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsNoiBat).HasDefaultValue(false);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ChatLieu).WithMany(p => p.SanPhams).HasConstraintName("FK__SanPham__ChatLie__70DDC3D8");

            entity.HasOne(d => d.DanhMuc).WithMany(p => p.SanPhams).HasConstraintName("FK__SanPham__DanhMuc__6FE99F9F");

            entity.HasOne(d => d.KhuyenMai).WithMany(p => p.SanPhams).HasConstraintName("FK__SanPham__KhuyenM__71D1E811");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.VaiTroId).HasName("PK__VaiTro__47758136355138D8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
