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
    /// Interaction logic for StudentCourse.xaml
    /// </summary>
    public partial class StudentCourse : Page
    {
        private DrivingSkillCertContext _context = new DrivingSkillCertContext();
        public StudentCourse()
        {
            InitializeComponent();
            LoadRegisteredCourses();

        }
        private void LoadRegisteredCourses()
        {
            // Lấy thông tin người dùng hiện tại từ Application.Current.Properties
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

            // Sử dụng LINQ để join bảng Registrations và Courses
            var registeredCourses = (from reg in _context.Registrations
                                     join c in _context.Courses on reg.CourseId equals c.CourseId
                                     where reg.UserId == currentUser.UserId
                                           && reg.IsDelete == false
                                           && c.IsDelete == false
                                     select new
                                     {
                                         c.CourseId,
                                         c.CourseName,
                                         TeacherFullName = c.Teacher.FullName,
                                         reg.Status,
                                         c.StartDate,
                                         c.EndDate
                                     }).ToList();

            RegisteredCoursesDataGrid.ItemsSource = registeredCourses;
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
