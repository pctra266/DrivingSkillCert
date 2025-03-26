using DrivingSkillCert.DAO;
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
        private RegistrationDAO _registrationDAO;
        private User _currentTeacher;
        private Registration _reg;
        public TeacherRegistrationManagement()
        {
            _registrationDAO = new RegistrationDAO();
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
            _currentTeacher = currentUserObj as User;
            if (_currentTeacher == null)
            {
                MessageBox.Show("Lỗi dữ liệu người dùng.");
                return;
            }

            var registrations = _registrationDAO.GetRegistrationsOfTeacher(_currentTeacher.UserId);

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
        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegistrationsDataGrid.Items.Filter = FilterMethod;
        }

        private bool FilterMethod(object obj)
        {
            var reg = (Registration)obj;
            return reg.User.FullName.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase)
                || reg.Course.CourseName.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_registrationDAO != null && cbStatus.SelectedItem is ComboBoxItem cbItem &&_currentTeacher!=null)
                RegistrationsDataGrid.ItemsSource = _registrationDAO.GetRegistrationsOfTeacher(_currentTeacher.UserId).Where(e => e.Status.Contains(cbItem.Content.ToString()));
        }

        private void RegistrationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(RegistrationsDataGrid.SelectedItem!=null)
            {
                detailPanel.Visibility = Visibility.Visible;  
                _reg = (Registration)RegistrationsDataGrid.SelectedItem;
                txtComment.Text = _reg.Comments;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_reg != null && _registrationDAO!=null) { 
                _reg.Comments = txtComment.Text;
                _registrationDAO.UpdateRegistration(_reg);
                detailPanel.Visibility = Visibility.Collapsed;    
            }
            LoadRegistrations();
        }

        private void RegistrationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RegistrationsDataGrid.SelectedItem != null)
            {
                _reg = (Registration)RegistrationsDataGrid.SelectedItem;
            }
            detailPanel.Visibility = Visibility.Collapsed;
        }
    }
}
