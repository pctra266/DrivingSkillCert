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
    /// Interaction logic for TeacherRegistrationManagement.xaml
    /// </summary>
    /// 

    public partial class TeacherRegistrationManagement : Page
    {
        private DrivingSkillCertContext _context = new DrivingSkillCertContext();
        public TeacherRegistrationManagement()
        {
            InitializeComponent();
            LoadRegistrations();
        }
        private void LoadRegistrations()
        {
            var currentUserObj = Application.Current.Properties["LoggedInUser"];
            if (currentUserObj == null)
            {
                MessageBox.Show("Bạn chưa đăng nhập. Vui lòng đăng nhập lại.");
                return;
            }
            User currentTeacher = currentUserObj as User;
            if (currentTeacher == null)
            {
                MessageBox.Show("Lỗi dữ liệu người dùng.");
                return;
            }

            var registrations = (from reg in _context.Registrations
                                 join c in _context.Courses on reg.CourseId equals c.CourseId
                                 join student in _context.Users on reg.UserId equals student.UserId
                                 where c.TeacherId == currentTeacher.UserId
                                       && reg.IsDelete == false
                                       && c.IsDelete == false
                                 select new
                                 {
                                     reg.RegistrationId,
                                     StudentName = student.FullName,
                                     student.Email,
                                     CourseName = c.CourseName,
                                     reg.Status,
                                     reg.Comments
                                 }).ToList();

            RegistrationsDataGrid.ItemsSource = registrations;
        }
        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (RegistrationsDataGrid.SelectedItem != null)
            {
                dynamic selectedReg = RegistrationsDataGrid.SelectedItem;
                int registrationId = selectedReg.RegistrationId;

                var registration = _context.Registrations.FirstOrDefault(r => r.RegistrationId == registrationId);
                if (registration != null)
                {
                    registration.Status = "Approved";
                    _context.SaveChanges();
                    var course = _context.Courses.FirstOrDefault(c => c.CourseId == registration.CourseId);
                    string courseName = course?.CourseName ?? "khóa học";

                    SendNotificationToStudent(registration.UserId, courseName, "duyệt em nhé");

                    MessageBox.Show("Đã duyệt đăng ký thành công.");
                    LoadRegistrations();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đăng ký để duyệt.");
            }
        }
        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            if (RegistrationsDataGrid.SelectedItem != null)
            {
                dynamic selectedReg = RegistrationsDataGrid.SelectedItem;
                int registrationId = selectedReg.RegistrationId;
                var registration = _context.Registrations.FirstOrDefault(r => r.RegistrationId == registrationId);
                if (registration != null)
                {
                    registration.Status = "Rejected";
                    _context.SaveChanges();
                    var course = _context.Courses.FirstOrDefault(c => c.CourseId == registration.CourseId);
                    string courseName = course?.CourseName ?? "khóa học";

                    // Gửi thông báo tới học sinh
                    SendNotificationToStudent(registration.UserId, courseName, "từ chối em nhé");

                    MessageBox.Show("Đã từ chối đăng ký thành công.");
                    LoadRegistrations();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đăng ký để từ chối.");
            }
        }
        private void SendNotificationToStudent(int studentId, string courseName, string status)
        {
            string message = $"Đơn đăng ký khóa học '{courseName}' của bạn đã được {status}.";
            var notification = new Notification
            {
                UserId = studentId,
                Message = message,
                SentDate = DateTime.Now,
                IsRead = false,
                IsDelete = false
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }



        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Window.GetWindow(this).Close();
            }
        }
    }
}
