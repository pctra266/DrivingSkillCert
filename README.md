# SafeDrivingCert - Ứng dụng Quản lý Đăng ký và Thi Chứng chỉ Kỹ năng Lái xe An toàn

## 📌 Giới thiệu
SafeDrivingCert là một ứng dụng hỗ trợ học sinh trung học phổ thông, giảng viên dạy kỹ năng lái xe an toàn và cảnh sát giao thông trong việc quản lý đăng ký, tổ chức và thi cấp chứng chỉ kỹ năng lái xe an toàn. Hệ thống đảm bảo tính minh bạch, hiệu quả và dễ sử dụng.

## 👨‍💻 Thành viên nhóm
- **Phạm Công Trà** - Leader
- **Nguyễn Hải Nam**
- **Đinh Phúc Sơn**

## 🛠️ Công nghệ sử dụng
- **Nền tảng**: WPF (Windows Presentation Foundation)
- **Cơ sở dữ liệu**: SQL Server + EntityFrameworkCore
- **Mô hình kiến trúc**: Three Layers / MVC / MVVM
- **Giao diện**: Thân thiện, trực quan

## 🚀 Tính năng chính
### 1. Quản lý thông tin người dùng
- Học sinh: Quản lý thông tin cá nhân, lớp, trường.
- Giảng viên: Quản lý khóa học, học sinh và kết quả học tập.
- Cảnh sát giao thông: Theo dõi và phê duyệt các kỳ thi, chứng chỉ.

### 2. Đăng ký tham gia khóa học và thi
- Học sinh đăng ký khóa học và kỳ thi trực tuyến.
- Giảng viên xác nhận danh sách học sinh.
- Cảnh sát giao thông theo dõi trạng thái phê duyệt chứng chỉ.

### 3. Quản lý khóa học
- Giảng viên tạo và quản lý khóa học (giáo viên, lịch học, nội dung).
- Theo dõi tiến trình và kết quả học tập.

### 4. Quản lý thi và cấp chứng chỉ
- Lập danh sách thi, phân công giám sát.
- Ghi nhận điểm số và trạng thái đạt chứng chỉ.
- Cấp chứng chỉ cho học sinh đạt yêu cầu.

### 5. Thông báo và giao tiếp
- Gửi thông báo tự động về lịch học, lịch thi, kết quả, chứng chỉ.
- Hỗ trợ giao tiếp giữa học sinh, giảng viên và cảnh sát giao thông.

### 6. Báo cáo và thống kê
- Báo cáo số lượng học sinh đăng ký, tỷ lệ tham gia và đạt chứng chỉ.
- Thống kê kết quả theo lớp/trường/khóa học.

## 🎭 Vai trò và chức năng người dùng
### 1. Học sinh
- Đăng ký khóa học và thi trực tuyến.
- Theo dõi tiến trình học và kết quả thi.
- Nhận chứng chỉ sau khi đạt yêu cầu.

### 2. Giảng viên
- Tạo và quản lý khóa học.
- Ghi nhận điểm số, đánh giá kỹ năng học sinh.
- Phối hợp với cảnh sát giao thông tổ chức thi.

### 3. Cảnh sát giao thông
- Xác nhận và giám sát kỳ thi.
- Quản lý việc cấp chứng chỉ.
- Đảm bảo tuân thủ quy định pháp luật.

### 4. Quản trị viên hệ thống
- Quản lý tài khoản người dùng.
- Cấu hình hệ thống và bảo mật.

## 🏗️ Hướng dẫn cài đặt
### Yêu cầu hệ thống
- **Hệ điều hành**: Windows 10/11
- **Công cụ phát triển**: Visual Studio 2022 trở lên
- **Cơ sở dữ liệu**: SQL Server
- **Framework**: .NET Core / .NET 6+

### Cài đặt và chạy dự án
1. Clone repository:
   ```sh
   git clone https://github.com/your-repo-url.git
   cd SafeDrivingCert
   ```
2. Cấu hình chuỗi kết nối cơ sở dữ liệu trong `appsettings.json`.
3. Chạy lệnh để apply database migrations:
   ```sh
   dotnet ef database update
   ```
4. Chạy ứng dụng:
   ```sh
   dotnet run
   ```

## 📑 Báo cáo dự án
Dự án bao gồm các tài liệu:
- **Mã nguồn ứng dụng**: Chứa trong repository này.
- **Tài liệu thiết kế và báo cáo phân tích**: Mô tả chi tiết về hệ thống.
- **Hình ảnh giao diện và chức năng**: Tổng hợp ảnh chụp minh họa ứng dụng.

## 📜 License
Dự án được phát triển như một bài tập môn học PRN212 và không dùng cho mục đích thương mại.

---
🚗 **SafeDrivingCert** - Giúp việc học và thi kỹ năng lái xe an toàn dễ dàng hơn!
