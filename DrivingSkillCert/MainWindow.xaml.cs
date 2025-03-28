﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DrivingSkillCert.DAO;
using Model.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (Application.Current.Properties.Contains("LoggedInUser"))
            {
                User currentUser =
               (Model.Models.User)Application.Current.Properties["LoggedInUser"];
                SetButtonVisibilityByRole(currentUser.Role);
            }

        }
        private void SetButtonVisibilityByRole(string role)
        {
            // Ẩn tất cả các nút trước
            btnGoToCourse.Visibility = Visibility.Collapsed;
            btnGoToExam.Visibility = Visibility.Collapsed;
            btnGoToUser.Visibility = Visibility.Collapsed;
            btnGoToCertificate.Visibility = Visibility.Collapsed;
            btnGoToResult.Visibility = Visibility.Collapsed;
            btnGoToRegistration.Visibility = Visibility.Collapsed;
            btnGoToViewCertificate.Visibility = Visibility.Collapsed;
            btnGotoResultTeacherSite.Visibility = Visibility.Collapsed;
            btnGoToStudentExam.Visibility = Visibility.Collapsed;
            btnGotoRegisStudent.Visibility = Visibility.Collapsed;
            btnGotoStudentCourse.Visibility = Visibility.Collapsed;
            btnGotoTeacherCourse.Visibility = Visibility.Collapsed;
            btnGotoRequest.Visibility = Visibility.Collapsed;
            btnGotoApproveRequest.Visibility = Visibility.Collapsed;
            // Hiển thị các nút theo role
            btnGotoNotification.Visibility = Visibility.Visible; // thông báo luôn hiển thị
            switch (role)
            {
                case "Admin":
                    btnGoToCourse.Visibility = Visibility.Visible;
                    btnGoToExam.Visibility = Visibility.Visible;
                    btnGoToResult.Visibility = Visibility.Visible;
                    btnGoToRegistration.Visibility = Visibility.Visible;
                    btnGoToUser.Visibility = Visibility.Visible;
                    btnGoToCertificate.Visibility = Visibility.Visible;
                    
                    break;
                case "Student":
                    btnGoToViewCertificate.Visibility = Visibility.Visible;
                    btnGoToStudentExam.Visibility = Visibility.Visible;
                    btnGotoRegisStudent.Visibility = Visibility.Visible;
                    btnGotoStudentCourse.Visibility = Visibility.Visible;
                    break;
                case "Teacher":
                    btnGotoResultTeacherSite.Visibility = Visibility.Visible;
                    btnGotoTeacherCourse.Visibility = Visibility.Visible;
                    btnGotoRequest.Visibility = Visibility.Visible;
                    break;
                case "TrafficPolice":
                    btnGotoApproveRequest.Visibility = Visibility.Visible;
                    break;
                default:
                    MessageBox.Show("Vai trò không hợp lệ");
                    break;
            }
        }

        private void GoToCourse_Click(object sender, RoutedEventArgs e)
        {
           

            MainFrame.Navigate(new ManageCourses());
        }
        private void GoToExam_Click(object sender, RoutedEventArgs e)
        {
           

            MainFrame.Navigate(new ManageExam());
        }

        private void btnGoToUser_Click(object sender, RoutedEventArgs e)
        {
           

            MainFrame.Navigate(new ManageUser());
        }

        private void btnGoToCertificate_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ManageCertificate());
        }

        private void btnGoToResult_Click(object sender, RoutedEventArgs e)
        {

            MainFrame.Navigate(new ManageResult());
        }

        private void btnGoToRegistration_Click(object sender, RoutedEventArgs e)
        {
          
            MainFrame.Navigate(new ManageRegistration());
        }
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            if (btnProfile.ContextMenu != null)
            {
                btnProfile.ContextMenu.IsOpen = true; // Mở menu khi click
            }
        }
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow();
            changePasswordWindow.Show(); // Mở dưới dạng popup
            this.Close();
        }
        private void PersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            PersonalInfoWindow personalInfoWindow = new PersonalInfoWindow();
            personalInfoWindow.Show(); // Mở dưới dạng popup
            this.Close();

        }
        private void Logout_click(object sender, RoutedEventArgs e)
        {
            Login loginwindow = new Login();
            loginwindow.Show();
            this.Close();
        }


        private void btnGoToViewCertificate_Click(object sender, RoutedEventArgs e)
        {
            UserControl  userControl = new UserControl();
            userControl.Show();
            this.Close();
        }

        private void btnGotoResultTeacherSite_Click(object sender, RoutedEventArgs e)
        {
            ScoringPractical scoringPractical = new ScoringPractical();
            scoringPractical.Show();
            this.Close();
        }

        private void btnGoToStudentExam_Click(object sender, RoutedEventArgs e)
        {
           
            MainFrame.Navigate(new ExamStudent());

        }

        private void btnGotoRegisStudent_Click(object sender, RoutedEventArgs e)
        {
           
            MainFrame.Navigate(new RegistrationCourse());



        }

        private void btnGotoStudentCourse_Click(object sender, RoutedEventArgs e)
        {
          
            MainFrame.Navigate(new StudentCourse());

        }

        private void btnGotoTeacherCourse_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TeacherRegistrationManagement());
        }

        private void btnGotoNotification_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NotificationPage());
        }

        private void btnGotoRequest_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CertificateServiceForTeacher());
        }

        private void btnGotoApproveRequest_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CertificateServiceForTrafficPolice());
        }
    }
}