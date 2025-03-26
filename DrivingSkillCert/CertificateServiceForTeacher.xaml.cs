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
    /// Interaction logic for CertificateServiceForTeacher.xaml
    /// </summary>
    public partial class CertificateServiceForTeacher : Page
    {
        private DrivingSkillCertContext _context = new DrivingSkillCertContext();
        public CertificateServiceForTeacher()
        {
            InitializeComponent();
            LoadRegistrations();
        }
        private void LoadRegistrations()
        {
            var teacher = Application.Current.Properties["LoggedInUser"] as User;
            if (teacher == null || teacher.Role != "Teacher")
            {
                MessageBox.Show("Bạn không có quyền truy cập.");
                return;
            }

            var registrations = (
    from reg in _context.Registrations
    join c in _context.Courses on reg.CourseId equals c.CourseId
    join student in _context.Users on reg.UserId equals student.UserId
    join exam in _context.Exams on c.CourseId equals exam.CourseId
    join r in _context.Results on exam.ExamId equals r.ExamId
    where reg.Status == "Approved"
          && reg.IsDelete == false
          && exam.IsDelete == false
          && c.TeacherId == teacher.UserId
    group new { r.Score, exam.Type } by new
    {
        reg.UserId,
        student.FullName,
        reg.CourseId,
        c.CourseName
    } into g
    select new
    {
        UserId = g.Key.UserId,
        StudentName = g.Key.FullName,
        CourseId = g.Key.CourseId,
        CourseName = g.Key.CourseName,
        TheoryScore = g.Where(x => x.Type == "Theory").Max(x => (decimal?)x.Score) ?? 0, // Lấy điểm cao nhất của lý thuyết
        PracticeScore = g.Where(x => x.Type == "Practice").Max(x => (decimal?)x.Score) ?? 0 // Lấy điểm cao nhất của thực hành
    }).ToList(); // Lấy danh sách tất cả sinh viên


            RegistrationsDataGrid.ItemsSource = registrations;
        }
        private void btnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RegistrationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một đăng ký.");
                return;
            }

            dynamic selected = RegistrationsDataGrid.SelectedItem;
            int studentId = selected.UserId;
            int courseId = selected.CourseId;

            CertificateServiceForTeacher service = new CertificateServiceForTeacher();
            service.SendCertificateRequest(studentId, courseId);
        }

        public void SendCertificateRequest(int studentId, int courseId)
        {
            // Lấy thông tin Teacher từ phiên đăng nhập
            var teacher = Application.Current.Properties["LoggedInUser"] as User;
            if (teacher == null || teacher.Role != "Teacher")
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.");
                return;
            }

            // Lấy thông tin học viên và khóa học từ database
            var student = _context.Users.FirstOrDefault(u => u.UserId == studentId && u.Role == "Student" && !u.IsDelete == true);
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId && !c.IsDelete == true);
            if (student == null || course == null)
            {
                MessageBox.Show("Không tìm thấy học viên hoặc khóa học.");
                return;
            }

            // Tìm TrafficPolice (giả sử chỉ có một TrafficPolice hoặc dùng tiêu chí lọc phù hợp)
            var trafficPolice = _context.Users.FirstOrDefault(u => u.Role == "TrafficPolice" && !u.IsDelete == true);
            if (trafficPolice == null)
            {
                MessageBox.Show("Không tìm thấy nhân viên cảnh sát giao thông.");
                return;
            }

            // Tạo thông báo gửi tới TrafficPolice với định dạng: "CERT_REQ|StudentID|CourseID|StudentName|CourseName"
            string message = $"CERT_REQ|{student.UserId}|{course.CourseId}|{student.FullName}|{course.CourseName}";
            Notification notification = new Notification
            {
                UserId = trafficPolice.UserId,
                Message = message,
                SentDate = DateTime.Now,
                IsRead = false,
                IsDelete = false
            };


            _context.Notifications.Add(notification);
            _context.SaveChanges();
            MessageBox.Show("Yêu cầu cấp chứng chỉ đã được gửi tới Cảnh sát giao thông.");
        }
    }
}
