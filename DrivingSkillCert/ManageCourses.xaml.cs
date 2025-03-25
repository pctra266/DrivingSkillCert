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
using DrivingSkillCert.DAO;
using Microsoft.EntityFrameworkCore;
using Model.Models;


namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ManageCourses.xaml
    /// </summary>
    public partial class ManageCourses : Page
    {
        private readonly CourseDAO courseDAO;
        private readonly UserDAO userDAO;
        private int? editingCourseId = null;

        public ManageCourses()
        {

            courseDAO = new CourseDAO();
            userDAO = new UserDAO();

            InitializeComponent();
            LoadCourses();
            LoadTeachers();
        }

        private void LoadCourses()
        {
            try
            {
                dgCourses.ItemsSource = courseDAO.GetCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}");
            }
        }

        private void LoadTeachers()
        {
            try
            {
                cmbTeacher.ItemsSource = userDAO.GetUsersByRole("Teacher");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading teachers: {ex.Message}");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            editingCourseId = null;
            ClearForm();
            detailPanel.Visibility = Visibility.Visible;
            dgCourses.SelectedItem = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCourses.SelectedItem is Course selectedCourse)
            {
                editingCourseId = selectedCourse.CourseId;
                txtCourseName.Text = selectedCourse.CourseName;
                cmbTeacher.SelectedValue = selectedCourse.TeacherId;
                dpStartDate.SelectedDate = selectedCourse.StartDate;
                dpEndDate.SelectedDate = selectedCourse.EndDate;
                detailPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCourses.SelectedItem is Course selectedCourse)
            {
                if (MessageBox.Show($"Are you sure you want to delete course '{selectedCourse.CourseName}'?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        courseDAO.DeleteCourse(selectedCourse.CourseId);
                        LoadCourses();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting course: {ex.Message}");
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCourseName.Text) ||
                    cmbTeacher.SelectedValue == null ||
                    dpStartDate.SelectedDate == null ||
                    dpEndDate.SelectedDate == null)
                {
                    MessageBox.Show("Please fill in all required fields");
                    return;
                }

                var course = new Course
                {
                    CourseName = txtCourseName.Text,
                    TeacherId = (int)cmbTeacher.SelectedValue,
                    StartDate = dpStartDate.SelectedDate.Value,
                    EndDate = dpEndDate.SelectedDate.Value
                };

                if (editingCourseId.HasValue)
                {
                    course.CourseId = editingCourseId.Value;
                    courseDAO.UpdateCourse(course);
                }
                else
                {
                    courseDAO.CreateCourse(course);
                }

                LoadCourses();
                detailPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving course: {ex.Message}");
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
            ClearForm();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtFilter.Text = "";
            LoadCourses();
        }

        private void dgCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
        }

        private void ClearForm()
        {
            txtCourseName.Text = string.Empty;
            cmbTeacher.SelectedIndex = -1;
            dpStartDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
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
        private void btnGoToBankQuestions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ManageBankQuestion());
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgCourses.Items.Filter = FilterMethod;
        }

        private bool FilterMethod(object obj)
        {
            var course = (Course)obj;
            return course.CourseName.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase)
                ||course.Teacher.FullName.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase);
        }
    }
}
