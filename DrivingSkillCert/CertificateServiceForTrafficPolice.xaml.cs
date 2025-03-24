using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for CertificateServiceForTrafficPolice.xaml
    /// </summary>
    public partial class CertificateServiceForTrafficPolice : Page
    {
        private DrivingSkillCertContext _context = new DrivingSkillCertContext();
        public CertificateServiceForTrafficPolice()
        {
            InitializeComponent();
            LoadCertificateRequests();
        }
        private void LoadCertificateRequests()
        {
            // Lấy các thông báo có Message bắt đầu bằng "CERT_REQ|"
            var notifications = _context.Notifications
                .Where(n => n.Message.StartsWith("CERT_REQ|") && !n.IsDelete==true)
                .OrderByDescending(n => n.SentDate)
                .ToList();

            // Parse nội dung thông báo để lấy các thông tin cần thiết
            var requests = new List<CertificateRequestViewModel>();

            foreach (var notif in notifications)
            {
                // Message format: CERT_REQ|StudentID|CourseID|StudentName|CourseName
                string[] parts = notif.Message.Split('|');
                if (parts.Length >= 5)
                {
                    if (int.TryParse(parts[1], out int studentId) &&
                        int.TryParse(parts[2], out int courseId))
                    {
                        requests.Add(new CertificateRequestViewModel
                        {
                            NotificationID = notif.NotificationId,
                            StudentID = studentId,
                            CourseID = courseId,
                            StudentName = parts[3],
                            CourseName = parts[4]
                        });
                    }
                }
            }

            RequestsDataGrid.ItemsSource = requests;
        }

        // Sự kiện khi TrafficPolice click nút "Cấp chứng chỉ"
        private void btnIssueCertificate_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu cấp chứng chỉ.");
                return;
            }

            var selectedRequest = RequestsDataGrid.SelectedItem as CertificateRequestViewModel;
            if (selectedRequest == null)
            {
                MessageBox.Show("Yêu cầu không hợp lệ.");
                return;
            }

            IssueCertificate(selectedRequest);
        }

        private void IssueCertificate(CertificateRequestViewModel request)
        {
            // Kiểm tra quyền của TrafficPolice từ phiên đăng nhập
            var trafficPolice = Application.Current.Properties["LoggedInUser"] as User;
            if (trafficPolice == null || trafficPolice.Role != "TrafficPolice")
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.");
                return;
            }

            // Lấy thông tin học viên và khóa học từ database
            var student = _context.Users.FirstOrDefault(u => u.UserId == request.StudentID && u.Role == "Student" && !u.IsDelete==true);
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == request.CourseID && !c.IsDelete == true);
            if (student == null || course == null)
            {
                MessageBox.Show("Không tìm thấy học viên hoặc khóa học.");
                return;
            }

            // Lấy thông tin Teacher phụ trách khóa học
            var teacher = _context.Users.FirstOrDefault(u => u.UserId == course.TeacherId && !u.IsDelete == true);
            if (teacher == null)
            {
                MessageBox.Show("Không tìm thấy giáo viên phụ trách khóa học.");
                return;
            }

            // Tạo chứng chỉ cho học viên
            Certificate certificate = new Certificate
            {
                UserId = student.UserId,
                IssuedDate = DateOnly.FromDateTime(DateTime.Now),
                ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(5)), // Ví dụ: chứng chỉ hiệu lực 5 năm
                CertificateCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                IsDelete = false
            };
            _context.Certificates.Add(certificate);
            _context.SaveChanges();

            // Tạo thông báo gửi cho học viên
            string studentMessage = $"Chứng chỉ cho khóa học {course.CourseName} đã được cấp.";
            Notification studentNotification = new Notification
            {
                UserId = student.UserId,
                Message = studentMessage,
                SentDate = DateTime.Now,
                IsRead = false,
                IsDelete = false
            };
            _context.Notifications.Add(studentNotification);

            // Tạo thông báo gửi cho Teacher
            string teacherMessage = $"Chứng chỉ cho học viên {student.FullName} (Khóa: {course.CourseName}) đã được cấp.";
            Notification teacherNotification = new Notification
            {
                UserId = teacher.UserId,
                Message = teacherMessage,
                SentDate = DateTime.Now,
                IsRead = false,
                IsDelete = false
            };
            _context.Notifications.Add(teacherNotification);

            _context.SaveChanges();
            MessageBox.Show("Chứng chỉ đã được cấp thành công.");

            // Tải lại danh sách yêu cầu sau khi xử lý
            LoadCertificateRequests();
        }
    }
    /// <summary>
    /// ViewModel dùng để hiển thị yêu cầu cấp chứng chỉ được parse từ thông báo.
    /// </summary>
    public class CertificateRequestViewModel
    {
        public int NotificationID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
    }


}


