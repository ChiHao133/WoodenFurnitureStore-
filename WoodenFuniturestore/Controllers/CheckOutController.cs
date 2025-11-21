using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WoodenFuniturestore.Models; 
using WoodenFuniturestore.Data; 

[Authorize]
public class CheckOutController : Controller
{
    private readonly ApplicationDbContext _context; // (Thay ApplicationDbContext bằng tên DbContext của bạn)

    public CheckOutController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Hàm trợ giúp lấy UserID
    private int GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int.TryParse(userIdString, out int userId);
        return userId;
    }

    // GET: /ThanhToan/Index
    // Nhận danh sách ID từ URL (do form Giỏ hàng gửi sang)
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] List<int> selectedItems)
    {
        if (selectedItems == null || !selectedItems.Any())
        {
            TempData["CartError"] = "Bạn chưa chọn sản phẩm nào để thanh toán.";
            return RedirectToAction("Index", "GioHang");
        }

        int userId = GetCurrentUserId();
        var user = await _context.NguoiDungs.FindAsync(userId);
        var gioHang = await _context.GioHangs.FirstOrDefaultAsync(g => g.MaNguoiDung == userId);

        if (user == null || gioHang == null) return Unauthorized();

        // Lấy thông tin các món hàng đã chọn
        var cartItems = await _context.ChiTietGioHangs
            .Where(ct => ct.MaGioHang == gioHang.MaGioHang && selectedItems.Contains(ct.MaChiTiet))
            .Include(ct => ct.MaSanPhamNavigation)
            .ToListAsync();

        decimal tongTien = cartItems.Sum(item => item.SoLuong * item.GiaBan) ?? 0;

        // Tạo ViewModel
        var viewModel = new CheckOutViewModel
        {
            // Điền sẵn thông tin
            HoTenNhanHang = user.HoTen,
            EmailNhanHang = user.Email,
            SoDienThoaiNhan = user.SoDienThoai,
            DiaChiGiaoHang = user.DiaChi,

            // Dữ liệu đơn hàng
            CartItems = cartItems,
            TongTien = tongTien,
            SelectedItems = selectedItems // Gán ID để View render ra input ẩn
        };

        return View(viewModel); // Trả về Views/ThanhToan/Index.cshtml
    }

    // POST: /ThanhToan/PlaceOrder
    // Nhận dữ liệu từ form Thanh toán
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(CheckOutViewModel viewModel)
    {
        var selectedItems = viewModel.SelectedItems;
        int userId = GetCurrentUserId();
        var gioHang = await _context.GioHangs.FirstOrDefaultAsync(g => g.MaNguoiDung == userId);

        // Kiểm tra lại dữ liệu
        if (selectedItems == null || !selectedItems.Any())
        {
            ModelState.AddModelError("", "Không tìm thấy sản phẩm nào được chọn. Vui lòng thử lại.");
        }
        if (!ModelState.IsValid)
        {
            // Nếu có lỗi, phải tải lại CartItems để hiển thị tóm tắt đơn hàng
            viewModel.CartItems = await _context.ChiTietGioHangs
                .Where(ct => ct.MaGioHang == gioHang.MaGioHang && selectedItems.Contains(ct.MaChiTiet))
                .Include(ct => ct.MaSanPhamNavigation)
                .ToListAsync();
            return View("Index", viewModel); // Trả về trang Index với lỗi
        }

        // Lấy lại các món hàng TỪ DATABASE để đảm bảo an toàn
        var itemsToOrder = await _context.ChiTietGioHangs
            .Where(ct => ct.MaGioHang == gioHang.MaGioHang && selectedItems.Contains(ct.MaChiTiet))
            .Include(ct => ct.MaSanPhamNavigation) // Tải SanPham để kiểm tra tồn kho
            .ToListAsync();

        // Tính lại tổng tiền (Không tin tưởng TongTien từ client)
        decimal finalTotal = itemsToOrder.Sum(item => item.SoLuong * item.GiaBan)??0;

        // Bắt đầu Transaction (Đảm bảo an toàn)
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                // 1. Tạo Đơn Hàng
                var donHang = new DonHang
                {
                    UserId = userId,
                    NgayDatHang = DateTime.Now,
                    TrangThai = "Chờ xử lý", // Trạng thái mặc định
                    DiaChiGiaoHang = viewModel.DiaChiGiaoHang,
                    SoDienThoaiNhan = viewModel.SoDienThoaiNhan,
                    TongTien = finalTotal
                };
                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync(); // Lưu để lấy DonHangID

                // 2. Chuyển các món hàng sang Chi Tiết Đơn Hàng
                foreach (var item in itemsToOrder)
                {
                    // KIỂM TRA TỒN KHO
                    var sanPham = item.MaSanPhamNavigation;
                    if (sanPham.SoLuongTon < item.SoLuong)
                    {
                        // Nếu hết hàng, hủy giao dịch
                        await transaction.RollbackAsync();
                        TempData["CartError"] = $"Sản phẩm '{sanPham.TenSanPham}' không đủ số lượng (chỉ còn {sanPham.SoLuongTon}).";
                        return RedirectToAction("Index", "GioHang");
                    }

                    // Trừ tồn kho
                    sanPham.SoLuongTon -= (item.SoLuong??0);
                    _context.SanPhams.Update(sanPham);

                    // Tạo Chi Tiết Đơn Hàng
                    var chiTietDonHang = new ChiTietDonHang
                    {
                        DonHangId = donHang.DonHangId,
                        SanPhamId = (item.MaSanPham ?? 0),
                        SoLuong = (item.SoLuong ?? 0),
                        DonGia = (item.GiaBan ?? 0) // Lấy giá từ giỏ hàng (giá đã chốt)
                    };
                    _context.ChiTietDonHangs.Add(chiTietDonHang);

                    // 3. Xóa món hàng khỏi giỏ
                    _context.ChiTietGioHangs.Remove(item);
                }

                // 4. Lưu tất cả thay đổi (Thêm CĐH, Cập nhật SP, Xóa CGH)
                await _context.SaveChangesAsync();

                // 5. Hoàn tất giao dịch
                await transaction.CommitAsync();

                // Chuyển đến trang Cảm ơn
                return RedirectToAction("DatHangThanhCong", new { id = donHang.DonHangId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // (Log lỗi ex)
                ModelState.AddModelError("", "Đã có lỗi nghiêm trọng xảy ra khi đặt hàng. " + ex.Message);
                viewModel.CartItems = itemsToOrder;
                return View("Index", viewModel);
            }
        }
    }

    // GET: /ThanhToan/DatHangThanhCong/5
    [HttpGet]
    public async Task<IActionResult> DatHangThanhCong(int id)
    {
        // Bạn có thể lấy chi tiết đơn hàng nếu muốn
        // var donHang = await _context.DonHangs.FindAsync(id);
        // if (donHang == null) return NotFound();

        // Đơn giản nhất là chỉ cần ID
        ViewBag.DonHangID = id;
        return View();
    }
}