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
    /// Interaction logic for RegistrationCourse.xaml
    /// </summary>
    public partial class RegistrationCourse : Page
    {
        private DrivingSkillCertContext _context = new DrivingSkillCertContext();
        public RegistrationCourse()
        {
            InitializeComponent();
            LoadCourses();
        }
        private void LoadCourses()
        {
            var courses = (from c in _context.Courses
                           where c.IsDelete == false
                           select new
                           {
                               CourseId = c.CourseId,
                               c.CourseName,
                               // Lấy tên giáo viên từ Navigation Property Teacher
                               TeacherFullName = c.Teacher.FullName,
                               c.StartDate,
                               c.EndDate
                           }).ToList();

            CoursesDataGrid.ItemsSource = courses;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var currentUserObj = Application.Current.Properties["LoggedInUser"];
            if (currentUserObj == null)
            {
                MessageBox.Show("Bạn chưa đăng nhập. Vui lòng đăng nhập lại.");
                return;
            }

            User currentUser = currentUserObj as User;
            if (currentUser == null)
            {
                MessageBox.Show("Lỗi dữ liệu người dùng. Vui lòng đăng nhập lại.");
                return;
            }

            if (CoursesDataGrid.SelectedItem != null)
            {
                dynamic selectedCourse = CoursesDataGrid.SelectedItem;
                int courseId = selectedCourse.CourseId;

                var existingRegistration = _context.Registrations
                    .FirstOrDefault(r => r.CourseId == courseId && r.UserId == currentUser.UserId && r.IsDelete == false);

                if (existingRegistration == null)
                {
                    Registration newRegistration = new Registration
                    {
                        CourseId = courseId,
                        UserId = currentUser.UserId,
                        Status = "Pending",
                        Comments = "",
                        IsDelete = false
                    };

                    _context.Registrations.Add(newRegistration);
                    _context.SaveChanges();

                    MessageBox.Show("Đăng ký khóa học thành công. Vui lòng chờ duyệt đơn!");
                }
                else
                {
                    MessageBox.Show("Bạn đã đăng ký khóa học này rồi.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khóa học từ danh sách.");
            }
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
