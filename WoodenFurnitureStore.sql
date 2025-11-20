CREATE DATABASE WoodenFurnitureStore
USE WoodenFurnitureStore;
GO
-- Bảng VaiTro: Phân biệt Admin và Khách hàng
CREATE TABLE VaiTro (
    VaiTroID INT PRIMARY KEY IDENTITY(1,1),
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE -- 'Admin', 'KhachHang'
);

 -- Bảng NguoiDung: Lưu trữ thông tin tài khoản [cite: 10, 19]
CREATE TABLE NguoiDung (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    MatKhau NVARCHAR(255) NOT NULL, -- Sẽ lưu dạng hash
    SoDienThoai VARCHAR(20),
    DiaChi NVARCHAR(500),
    NgaySinh DATE,
    VaiTroID INT NOT NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (VaiTroID) REFERENCES VaiTro(VaiTroID)
);

-- Bảng DanhMuc: Quản lý danh mục sản phẩm (ví dụ: Bàn, Ghế, Tủ) [cite: 8]
CREATE TABLE DanhMuc (
    DanhMucID INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(255) NOT NULL,
    MoTa NVARCHAR(1000),
    DanhMucChaID INT, -- Hỗ trợ danh mục đa cấp
    FOREIGN KEY (DanhMucChaID) REFERENCES DanhMuc(DanhMucID)
);

-- Bảng ChatLieu: Quản lý chất liệu (ví dụ: Gỗ Sồi, Gỗ Xoan Đào) [cite: 8]
CREATE TABLE ChatLieu (
    ChatLieuID INT PRIMARY KEY IDENTITY(1,1),
    TenChatLieu NVARCHAR(255) NOT NULL
);

-- Bảng KhuyenMai: Quản lý các sự kiện và khuyến mãi [cite: 25, 26]
CREATE TABLE KhuyenMai (
    KhuyenMaiID INT PRIMARY KEY IDENTITY(1,1),
    TenSuKien NVARCHAR(255) NOT NULL,
    MoTa NVARCHAR(1000),
    PhanTramGiam DECIMAL(5, 2) NOT NULL DEFAULT 0, -- 27]
    NgayBatDau DATETIME NOT NULL,
    NgayKetThuc DATETIME NOT NULL
);

-- Bảng SanPham: Trung tâm của hệ thống, quản lý sản phẩm 6]
CREATE TABLE SanPham (
    SanPhamID INT PRIMARY KEY IDENTITY(1,1),
    TenSanPham NVARCHAR(500) NOT NULL,
    MoTa NTEXT,
    Gia DECIMAL(18, 2) NOT NULL,
    KichThuoc NVARCHAR(255),
    SoLuongTon INT NOT NULL DEFAULT 0, -- Cần để quản lý kho
    HinhAnh VARCHAR(1000), -- Đường dẫn tới ảnh
    IsNoiBat BIT DEFAULT 0, -- Sản phẩm nổi bật [cite: 9]
    IsActive BIT DEFAULT 1, -- Dùng cho chức năng xóa mềm [cite: 7]
    NgayTao DATETIME DEFAULT GETDATE(),
    
    -- Khóa ngoại
    DanhMucID INT,
    ChatLieuID INT,
    KhuyenMaiID INT, -- Có thể NULL nếu không khuyến mãi
    
    FOREIGN KEY (DanhMucID) REFERENCES DanhMuc(DanhMucID),
    FOREIGN KEY (ChatLieuID) REFERENCES ChatLieu(ChatLieuID),
    FOREIGN KEY (KhuyenMaiID) REFERENCES KhuyenMai(KhuyenMaiID)
);
-- Bảng DonHang: Lưu thông tin mỗi lần khách đặt hàng [cite: 13, 28]
CREATE TABLE DonHang (
    DonHangID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    NgayDatHang DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2) NOT NULL,
    TrangThai NVARCHAR(100) NOT NULL, -- Ví dụ: 'Chờ xử lý', 'Đang giao', 'Hoàn tất' [cite: 29]
    DiaChiGiaoHang NVARCHAR(500),
    SoDienThoaiNhan VARCHAR(20),
    
    FOREIGN KEY (UserID) REFERENCES NguoiDung(UserID)
);

-- Bảng ChiTietDonHang: Lưu các sản phẩm trong một đơn hàng
CREATE TABLE ChiTietDonHang (
    ChiTietDonHangID INT PRIMARY KEY IDENTITY(1,1),
    DonHangID INT NOT NULL,
    SanPhamID INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL, -- Giá tại thời điểm mua
    
    FOREIGN KEY (DonHangID) REFERENCES DonHang(DonHangID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID)
);

-- Bảng DanhGia (Phản hồi): Lưu đánh giá của khách hàng về sản phẩm [cite: 9, 16]
CREATE TABLE DanhGia (
    DanhGiaID INT PRIMARY KEY IDENTITY(1,1),
    SanPhamID INT NOT NULL,
    UserID INT NOT NULL,
    Rating INT CHECK (Rating >= 1 AND Rating <= 5), -- Thang điểm 1-5 sao
    NoiDung NTEXT,
    NgayDanhGia DATETIME DEFAULT GETDATE(),
    IsDuyet BIT DEFAULT 0, -- Admin duyệt trước khi hiển thị công khai 
    
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID),
    FOREIGN KEY (UserID) REFERENCES NguoiDung(UserID)
);

-- Bảng SanPhamYeuThich: Lưu các sản phẩm yêu thích của người dùng 
CREATE TABLE SanPhamYeuThich (
    UserID INT NOT NULL,
    SanPhamID INT NOT NULL,
    
    PRIMARY KEY (UserID, SanPhamID), -- Khóa chính ghép
    FOREIGN KEY (UserID) REFERENCES NguoiDung(UserID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID)
);

CREATE TABLE GioHang (
    MaGioHang INT PRIMARY KEY IDENTITY,
    MaNguoiDung INT,
    NgayTao DATETIME DEFAULT GETDATE()
);

CREATE TABLE ChiTietGioHang (
    MaChiTiet INT PRIMARY KEY IDENTITY,
    MaGioHang INT FOREIGN KEY REFERENCES GioHang(MaGioHang),
    MaSanPham INT,
    SoLuong INT,
    GiaBan DECIMAL(18,2)
);

ALTER TABLE GioHang
ADD FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(UserID);

ALTER TABLE ChiTietGioHang
ADD FOREIGN KEY (MaSanPham) REFERENCES SanPham(SanPhamID);

INSERT INTO VaiTro (TenVaiTro) VALUES 
(N'Admin'),
(N'KhachHang');


INSERT INTO NguoiDung (HoTen, Email, MatKhau, SoDienThoai, DiaChi, NgaySinh, VaiTroID)
VALUES 
(N'Nguyen Van A', 'a@example.com', '123', '0901234567', N'123 Đường A, TP.HCM', '1990-01-01', 1),
(N'Tran Thi B', 'b@example.com', '123', '0912345678', N'456 Đường B, Hà Nội', '1995-05-15', 2);

INSERT INTO DanhMuc (TenDanhMuc, MoTa)
VALUES 
(N'Bàn', N'Các loại bàn gỗ cao cấp'),
(N'Ghế', N'Ghế gỗ, ghế sofa'),
(N'Tủ', N'Tủ quần áo, tủ trang trí');

INSERT INTO ChatLieu (TenChatLieu)
VALUES 
(N'Gỗ Sồi'),
(N'Gỗ Xoan Đào'),
(N'Gỗ Lim');

INSERT INTO KhuyenMai (TenSuKien, MoTa, PhanTramGiam, NgayBatDau, NgayKetThuc)
VALUES 
(N'Tết sale', N'Giảm giá dịp Tết', 15.00, '2025-10-10', '2025-11-10'),
(N'Giảm giá hè', N'Khuyến mãi mùa hè', 10.00, '2025-06-01', '2025-06-30'),
(N'Tết sale', N'Giảm giá dịp Tết', 15.00, '2025-01-10', '2025-02-10');

update SanPham set KhuyenMaiID='3'
select *from SanPham
INSERT INTO SanPham (TenSanPham, MoTa, Gia, KichThuoc, SoLuongTon, HinhAnh, IsNoiBat, IsActive, DanhMucID, ChatLieuID, KhuyenMaiID)
VALUES 
(N'Tủ quần áo gỗ Lim', N'Tủ 3 cánh, thiết kế cổ điển', 5500000, N'200x60x220cm', 8, 'tuquanao.jpg', 1, 1, 3, 3, NULL),

(N'Bàn ăn gỗ Sồi 6 ghế', N'Bàn ăn cho gia đình 6 người', 7200000, N'180x90x75cm', 5, 'banan.jpg', 1, 1, 1, 1, 1),

(N'Ghế thư giãn Xoan Đào', N'Ghế ngồi đọc sách, có đệm', 3200000, N'70x80x100cm', 12, 'ghethugian.jpg', 0, 1, 2, 2, NULL),

(N'Tủ giày gỗ Sồi', N'Tủ giày 2 tầng, tiết kiệm không gian', 2100000, N'100x35x90cm', 15, 'tugiay.jpg', 0, 1, 3, 1, NULL),

(N'Bàn học sinh gỗ công nghiệp', N'Bàn học cho trẻ em, có ngăn kéo', 1800000, N'100x50x75cm', 20, 'banhocsinh.jpg', 0, 1, 1, NULL, NULL),

(N'Ghế sofa đơn màu xám', N'Ghế sofa đơn, phong cách hiện đại', 2800000, N'90x80x85cm', 10, 'ghesofadon.jpg', 1, 1, 2, 2, 2),

(N'Tủ sách 5 tầng gỗ Lim', N'Tủ sách đứng, phù hợp phòng khách', 4300000, N'80x30x180cm', 6, 'tusach.jpg', 1, 1, 3, 3, NULL),
(N'Bàn trà gỗ Xoan Đào', N'Bàn trà phòng khách, kiểu Nhật', 2600000, N'100x50x45cm', 7, 'bantra.jpg', 1, 1, 1, 2, NULL),

(N'Ghế ăn gỗ Sồi', N'Ghế ăn đơn giản, phù hợp mọi không gian', 950000, N'45x45x90cm', 30, 'gheanngo.jpg', 0, 1, 2, 1, NULL),

(N'Tủ trang trí gỗ Lim', N'Tủ trưng bày đồ decor, 4 ngăn', 3900000, N'120x40x160cm', 4, 'tutrangtri.jpg', 1, 1, 3, 3, 2),

(N'Bàn làm việc chân sắt mặt gỗ', N'Bàn làm việc phong cách công nghiệp', 2100000, N'120x60x75cm', 10, 'banchansat.jpg', 0, 1, 1, NULL, NULL),

(N'Ghế đôn gỗ tròn', N'Ghế đôn nhỏ, tiện lợi', 550000, N'35x35x45cm', 25, 'ghedon.jpg', 0, 1, 2, 1, NULL),

(N'Tủ đầu giường 2 ngăn kéo', N'Tủ nhỏ để cạnh giường ngủ', 1700000, N'50x40x50cm', 12, 'tudaugiuong.jpg', 0, 1, 3, 2, NULL),

(N'Bàn học gấp gọn thông minh', N'Bàn học có thể gấp lại, tiết kiệm diện tích', 1950000, N'100x50x75cm', 18, 'bangapgon.jpg', 1, 1, 1, NULL, 1),

(N'Ghế bập bênh gỗ Lim', N'Ghế thư giãn kiểu cổ điển', 3200000, N'60x100x90cm', 6, 'ghebapbenh.jpg', 1, 1, 2, 3, NULL);

INSERT INTO DonHang (UserID, TongTien, TrangThai, DiaChiGiaoHang, SoDienThoaiNhan)
VALUES 
(2, 6000000, N'Chờ xử lý', N'789 Đường C, Đà Nẵng', '0934567890');
INSERT INTO ChiTietDonHang (DonHangID, SanPhamID, SoLuong, DonGia)
VALUES 
(1, 1, 1, 2500000),
(1, 2, 1, 3500000);

INSERT INTO ChiTietDonHang (DonHangID, SanPhamID, SoLuong, DonGia)
VALUES 
(1, 1, 1, 2500000),
(1, 2, 1, 3500000);

INSERT INTO DanhGia (SanPhamID, UserID, Rating, NoiDung)
VALUES 
(1, 2, 5, N'Sản phẩm rất đẹp và chắc chắn'),
(2, 2, 4, N'Ghế ngồi thoải mái, giao hàng nhanh');
INSERT INTO SanPhamYeuThich (UserID, SanPhamID)
VALUES 
(2, 1),
(2, 2);

select *from DanhMuc
select *from KhuyenMai
alter table DanhMuc drop column DanhMucChaID
alter table DanhMuc drop FK__DanhMuc__DanhMuc__6477ECF3
update KhuyenMai set NgayKetThuc='2025-12-12'