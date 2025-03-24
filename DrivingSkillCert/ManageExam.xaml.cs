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
using Model.Models;

namespace DrivingSkillCert
{
    /// <summary>
    /// Interaction logic for ManageExam.xaml
    /// </summary>
    public partial class ManageExam : Page
    {
        private readonly ExamDAO examDAO;
        private readonly CourseDAO courseDAO;
        private int? editingExamId = null;
        public ManageExam()
        {
            examDAO = new ExamDAO();
            courseDAO = new CourseDAO();
            InitializeComponent();
            LoadCourses();
            LoadExams();
        }

        private void LoadExams()
        {
            var exams = examDAO.GetExams();
            exams.Reverse();
            dgExams.ItemsSource = exams;
        }
        private void LoadCourses()
        {
            cmbCourse.ItemsSource = courseDAO.GetCourses();
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadExams();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Visible;
                cmbCourse.SelectedValue = -1;
                dpDate.Value =  DateTime.Now;
                txtRoom.Text = "";
        }

        private void dgExams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility= Visibility.Visible;
            if (dgExams.SelectedItem is Exam selectedExam) {
                editingExamId = selectedExam.ExamId;
                cmbCourse.SelectedValue = selectedExam.CourseId;
                cbType.SelectedIndex = selectedExam.Type.Equals("Theory")?0:1;
                dpDate.Value = selectedExam.Date;
                txtRoom.Text = selectedExam.Room;
             }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgExams.SelectedItem is Exam selectedExam)
            {
                if (MessageBox.Show($"Are you sure you want to delete exam '{selectedExam.Course.CourseName}'?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        examDAO.DeleteExam(selectedExam.ExamId);
                        LoadExams();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting exam: {ex}");
                    }
                }
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Exam exam = new Exam();
            if (dpDate.Value==null || txtRoom.Text.Count()<1 || cbType.SelectionBoxItem.ToString().Count()<1) {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            exam.Date = dpDate.Value.Value;
            exam.Room = txtRoom.Text;
            exam.CourseId = (int)cmbCourse.SelectedValue;
            exam.Type = cbType.SelectionBoxItem.ToString();

            if (editingExamId==null) {
                examDAO.UpdateExam(exam);
                LoadExams();
                return;
            }
            exam.ExamId = editingExamId.Value;

            examDAO.UpdateExam(exam);
            LoadExams() ;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility= Visibility.Collapsed;
        }

        private void dgExams_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
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
